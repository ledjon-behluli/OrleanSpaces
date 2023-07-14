using System.Composition;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;
using System.Data.Common;

namespace OrleanSpaces.Analyzers.OSA002;

/// <summary>
/// Code fix provider for <see cref="TemplateCacheOverInitAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TemplateCacheOverInitFixer)), Shared]
internal sealed class TemplateCacheOverInitFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(TemplateCacheOverInitAnalyzer.Diagnostic.Id);
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root == null)
        {
            return;
        }

        var node = root.FindNode(context.Span);
        if (node == null)
        {
            return;
        }

        var props = context.Diagnostics.First().Properties;
        if (!int.TryParse(props.GetValueOrDefault("NumOfNulls"), out int numOfNulls))
        {
            return;
        }

        string templateTypeName = string.Empty;
        if (props.TryGetValue("TemplateTypeName", out string? typeName))
        {
            templateTypeName = typeName ?? string.Empty;
        }

        if (string.IsNullOrEmpty(templateTypeName))
        {
            return;
        }

        var newNode = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName($"{templateTypeName}Cache"),
            IdentifierName($"Tuple_{numOfNulls}"));

        var cacheNodeResult = await GetTemplateCacheNodeAsync(context.Document.Project.Solution, context.Document, templateTypeName, context.CancellationToken);
        var cacheNode = cacheNodeResult.Node;

        // {X}TemplateCache does not exist - Create & Add appropriate member
        if (cacheNode == null)
        {
            var fixInFileAction = CodeAction.Create(
                title: $"Cache value as a '{numOfNulls}-tuple' static readonly reference in this file",
                equivalenceKey: TemplateCacheOverInitAnalyzer.Diagnostic.Id,
                createChangedDocument: _ =>
                {
                    var newRoot = root.ReplaceNode(node, newNode);
                    if (newRoot != null)
                    {
                        var namespaceNode = newRoot.TryGetNamespaceNode();
                        if (namespaceNode != null)
                        {
                            cacheNode = CreateTemplateCacheNode(args: new int[] { numOfNulls }, isRightShifted: false, templateTypeName);
                            newRoot = newRoot.InsertNodesAfter(namespaceNode, new SyntaxNode[] { cacheNode });

                            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
                        }
                    }

                    return Task.FromResult(context.Document);
                });

            var fixInNewFileAction = CodeAction.Create(
                title: $"Cache value as a '{numOfNulls}-tuple' static readonly reference in a new file",
                equivalenceKey: TemplateCacheOverInitAnalyzer.Diagnostic.Id,
                createChangedSolution: _ =>
                {
                    var newRoot = root.ReplaceNode(node, newNode);
                    if (newRoot != null)
                    {
                        var namespaceNode = newRoot.TryGetNamespaceNode();
                        if (namespaceNode != null)
                        {
                            var newSolution = context.Document.Project.Solution.WithDocumentSyntaxRoot(context.Document.Id, newRoot);
                            if (newSolution != null)
                            {
                                CompilationUnitSyntax? compilationUnit = null;
                                cacheNode = CreateTemplateCacheNode(args: new int[] { numOfNulls }, isRightShifted: false, templateTypeName);

                                if (namespaceNode is NamespaceDeclarationSyntax nd)
                                {
                                    compilationUnit = WrapTemplateCacheInNamespace(cacheNode, nd, templateTypeName);
                                }

                                if (namespaceNode is FileScopedNamespaceDeclarationSyntax fsnd)
                                {
                                    compilationUnit = WrapTemplateCacheInNamespace(cacheNode, fsnd, templateTypeName);
                                }

                                if (namespaceNode is GlobalStatementSyntax)
                                {
                                    compilationUnit = WrapTemplateCacheInNamespace(cacheNode, templateTypeName);
                                }

                                if (compilationUnit != null)
                                {
                                    newSolution = newSolution.AddAdditionalDocument(
                                        documentId: DocumentId.CreateNewId(context.Document.Project.Id),
                                        folders: context.Document.Folders,
                                        name: $"{templateTypeName}Cache.cs",
                                        text: SourceText.From(compilationUnit.ToFullString()));

                                    return Task.FromResult(newSolution);
                                }
                            }
                        }
                    }

                    return Task.FromResult(context.Document.Project.Solution);
                });

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Cache value as a static readonly reference",
                    nestedActions: ImmutableArray.Create(fixInFileAction, fixInNewFileAction),
                    isInlinable: false),
                context.Diagnostics);

            return;
        }

        // {X}TemplateCache already exists
        TemplateCacheFieldVisitor visitor = new(templateTypeName);
        visitor.Visit(cacheNode);

        // Appropriate member already exists
        if (visitor.TuplesPresent.Contains(numOfNulls))   
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: $"Use '{templateTypeName}Cache.Tuple_{numOfNulls}'",
                    equivalenceKey: TemplateCacheOverInitAnalyzer.Diagnostic.Id,
                    createChangedDocument: _ =>
                    {
                        var newRoot = root.ReplaceNode(node, newNode);

                        return Task.FromResult(newRoot == null ? context.Document :
                            context.Document.WithSyntaxRoot(newRoot));

                    }), context.Diagnostics);

            return;
        }

        // Appropriate member does not exist
        visitor.TuplesPresent.Add(numOfNulls);
        int[] args = visitor.TuplesPresent.ToArray();

        // Add appropriate member to {X}TemplateCache (part of the document under analysis)
        if (cacheNodeResult.IsPartOfDocument)
        {
            context.RegisterCodeFix(
               CodeAction.Create(
                   title: $"Add and use '{templateTypeName}Cache.Tuple_{numOfNulls}'",
                   equivalenceKey: TemplateCacheOverInitAnalyzer.Diagnostic.Id,
                   createChangedDocument: async ct =>
                   {
                       var documentEditor = await DocumentEditor.CreateAsync(context.Document, ct);

                       // Cache Node
                       var newCacheNode = CreateTemplateCacheNode(args: args, isRightShifted: false, templateTypeName);
                       documentEditor.ReplaceNode(cacheNode, newCacheNode);

                       // Node
                       documentEditor.ReplaceNode(node, newNode);

                       // Result
                       var newDocument = documentEditor.GetChangedDocument();
                       return newDocument;

                   }), context.Diagnostics);

            return;
        }

        // Add appropriate member to {X}TemplateCache (part of another document)
        context.RegisterCodeFix(
            CodeAction.Create(
                title: $"Add and use '{templateTypeName}Cache.Tuple_{numOfNulls}'",
                equivalenceKey: TemplateCacheOverInitAnalyzer.Diagnostic.Id,
                createChangedSolution: async ct =>
                {
                    var solution = context.Document.Project.Solution;
                    var solutionEditor = new SolutionEditor(solution);

                    // Cache Node
                    var cacheNodeDocumentId = solution.GetDocumentId(cacheNode.SyntaxTree);
                    var newCacheNode = CreateTemplateCacheNode(args: args, isRightShifted: cacheNode.TryGetNamespaceNode() is NamespaceDeclarationSyntax, templateTypeName);
                    var cacheNodeDocumentEditor = await solutionEditor.GetDocumentEditorAsync(cacheNodeDocumentId, context.CancellationToken);
                    cacheNodeDocumentEditor.ReplaceNode(cacheNode, newCacheNode);

                    // Node
                    var nodeDocumentId = solution.GetDocumentId(node.SyntaxTree);
                    var nodeDocumentEditor = await solutionEditor.GetDocumentEditorAsync(nodeDocumentId, context.CancellationToken);
                    nodeDocumentEditor.ReplaceNode(node, newNode);

                    // Result
                    var newSolution = solutionEditor.GetChangedSolution();
                    return newSolution;

                }), context.Diagnostics);
    }

    private static async Task<TemplateCacheNodeResult> GetTemplateCacheNodeAsync(Solution solution, Document document, string templateTypeName, CancellationToken cancellationToken)
    {
        var project = document.Project;
        var projectReferences = project.AllProjectReferences;
        var referencedProjects = solution.Projects.Where(x => projectReferences.Contains(new ProjectReference(x.Id)));
        var referencedProjectsAndSelf = referencedProjects.ToList();
        
        referencedProjectsAndSelf.Add(project);   // add self

        foreach (var _document in referencedProjectsAndSelf.SelectMany(x => x.Documents))
        {
            if (!_document.SupportsSyntaxTree)
            {
                continue;
            }

            var root = await _document.GetSyntaxRootAsync(cancellationToken);
            if (root == null)
            {
                continue;
            }

            var cacheDeclaration = root.DescendantNodes().OfType<StructDeclarationSyntax>().FirstOrDefault(syntax => syntax.Identifier.ValueText == $"{templateTypeName}Cache");
            if (cacheDeclaration != null)
            {
                return new(cacheDeclaration, _document.Id.Equals(document.Id));
            }
        }

        return new();
    }

    private static StructDeclarationSyntax CreateTemplateCacheNode(int[] args, bool isRightShifted, string templateTypeName)
    {
        args = args.OrderBy(x => x).ToArray();
        string diagnosticId = TemplateCacheOverInitAnalyzer.Diagnostic.Id;
        string whiteSpace = isRightShifted ? "    " : "";

        List<FieldDeclarationSyntax> fieldDeclarations = new();
        List<PropertyDeclarationSyntax> propertyDeclarations = new();

        for (int i = 0; i < args.Length; i++)
        {
            SeparatedSyntaxList<ArgumentSyntax> syntaxList;

            if (args[i] == 1)
            {
                syntaxList = SingletonSeparatedList(Argument(LiteralExpression(SyntaxKind.NullLiteralExpression)));
            }
            else
            {
                int syntaxNodeOrTokenCount = 2 * args[i] - 1;
                var syntaxNodeOrTokens = new SyntaxNodeOrToken[syntaxNodeOrTokenCount];

                for (int j = 0; j < syntaxNodeOrTokenCount; j++)
                {
                    syntaxNodeOrTokens[j] = j % 2 == 0 ? 
                        Argument(LiteralExpression(SyntaxKind.NullLiteralExpression)) :
                        Token(TriviaList(), SyntaxKind.CommaToken, TriviaList(Space));
                }

                syntaxList = SeparatedList<ArgumentSyntax>(syntaxNodeOrTokens);
            }

            string fieldName = $"tuple_{args[i]}";
            string propertyName = $"Tuple_{args[i]}";

            var fieldDeclaration = FieldDeclaration(
                VariableDeclaration(
                    IdentifierName(
                        Identifier(
                            TriviaList(),
                            templateTypeName,
                            TriviaList(
                                Space))))
                .WithVariables(
                    SingletonSeparatedList(
                        VariableDeclarator(
                            Identifier(
                                TriviaList(),
                                fieldName,
                                TriviaList(
                                    Space)))
                        .WithInitializer(
                            EqualsValueClause(
                                ImplicitObjectCreationExpression()
                                .WithArgumentList(
                                    ArgumentList(syntaxList)))
                            .WithEqualsToken(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.EqualsToken,
                                    TriviaList(
                                        Space)))))));

            var propertyDeclaration = PropertyDeclaration(
                RefType(
                    IdentifierName(
                        Identifier(
                            TriviaList(),
                            templateTypeName,
                            TriviaList(
                                Space))))
                .WithRefKeyword(
                    Token(
                        TriviaList(),
                        SyntaxKind.RefKeyword,
                        TriviaList(
                            Space)))
                .WithReadOnlyKeyword(
                    Token(
                        TriviaList(),
                        SyntaxKind.ReadOnlyKeyword,
                        TriviaList(
                            Space))),
                Identifier(
                    TriviaList(),
                    propertyName,
                    TriviaList(
                        Space)));

            if (i == 0)      // first declaration
            {
                fieldDeclaration = fieldDeclaration
                    .WithModifiers(
                        TokenList(
                            new[]{
                            Token(
                                TriviaList(
                                    Trivia(
                                        PragmaWarningDirectiveTrivia(
                                            Token(SyntaxKind.DisableKeyword),
                                            true)
                                        .WithErrorCodes(
                                            SingletonSeparatedList<ExpressionSyntax>(
                                                IdentifierName(diagnosticId)))))
                                .NormalizeWhitespace(),
                                SyntaxKind.None,
                                TriviaList()),
                            Token(
                                TriviaList(
                                    Whitespace("    " + whiteSpace)),
                                SyntaxKind.PrivateKeyword,
                                TriviaList(
                                    Space)),
                            Token(
                                TriviaList(),
                                SyntaxKind.StaticKeyword,
                                TriviaList(
                                    Space)),
                            Token(
                                TriviaList(),
                                SyntaxKind.ReadOnlyKeyword,
                                TriviaList(
                                    Space))}))
                    .WithSemicolonToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.SemicolonToken,
                            TriviaList(
                                CarriageReturnLineFeed)));

                propertyDeclaration = propertyDeclaration
                    .WithModifiers(
                        TokenList(
                            new[]{
                                Token(
                                    TriviaList(
                                        Trivia(
                                            PragmaWarningDirectiveTrivia(
                                                Token(SyntaxKind.RestoreKeyword),
                                                true)
                                            .WithErrorCodes(
                                                SingletonSeparatedList<ExpressionSyntax>(
                                                    IdentifierName(diagnosticId)))))
                                    .NormalizeWhitespace(),
                                    SyntaxKind.None,
                                    TriviaList(
                                        CarriageReturnLineFeed)),
                                Token(
                                    TriviaList(
                                        Whitespace("    " + whiteSpace)),
                                    SyntaxKind.PublicKeyword,
                                    TriviaList(
                                        Space)),
                                Token(
                                    TriviaList(),
                                    SyntaxKind.StaticKeyword,
                                    TriviaList(
                                        Space))}))
                    .WithExpressionBody(
                        ArrowExpressionClause(
                            RefExpression(
                                IdentifierName(fieldName))
                            .WithRefKeyword(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.RefKeyword,
                                    TriviaList(
                                        Space))))
                        .WithArrowToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.EqualsGreaterThanToken,
                                TriviaList(
                                    Space))))
                    .WithSemicolonToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.SemicolonToken,
                            TriviaList(
                                CarriageReturnLineFeed)));
            }
            else
            {
                fieldDeclaration = fieldDeclaration
                    .WithModifiers(
                        TokenList(
                            new[]{
                            Token(
                                TriviaList(
                                    Whitespace("    " + whiteSpace)),
                                SyntaxKind.PrivateKeyword,
                                TriviaList(
                                    Space)),
                            Token(
                                TriviaList(),
                                SyntaxKind.StaticKeyword,
                                TriviaList(
                                    Space)),
                            Token(
                                TriviaList(),
                                SyntaxKind.ReadOnlyKeyword,
                                TriviaList(
                                    Space))}))
                    .WithSemicolonToken(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.SemicolonToken,
                                    TriviaList(
                                        CarriageReturnLineFeed)));

                propertyDeclaration = propertyDeclaration
                    .WithModifiers(
                        TokenList(
                            new[]{
                                Token(
                                    TriviaList(
                                        Whitespace("    " + whiteSpace)),
                                    SyntaxKind.PublicKeyword,
                                    TriviaList(
                                        Space)),
                                Token(
                                    TriviaList(),
                                    SyntaxKind.StaticKeyword,
                                    TriviaList(
                                        Space))}))
                    .WithExpressionBody(
                        ArrowExpressionClause(
                            RefExpression(
                                IdentifierName(fieldName))
                            .WithRefKeyword(
                                Token(
                                    TriviaList(),
                                    SyntaxKind.RefKeyword,
                                    TriviaList(
                                        Space))))
                        .WithArrowToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.EqualsGreaterThanToken,
                                TriviaList(
                                    Space))))
                    .WithSemicolonToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.SemicolonToken,
                            TriviaList(
                                CarriageReturnLineFeed)));
            }

            fieldDeclarations.Add(fieldDeclaration);
            propertyDeclarations.Add(propertyDeclaration);
        }

        IEnumerable <MemberDeclarationSyntax> memberDeclarations = fieldDeclarations
            .Cast<MemberDeclarationSyntax>()
            .Concat(propertyDeclarations.Cast<MemberDeclarationSyntax>());

        return StructDeclaration(
            Identifier(
                TriviaList(),
                $"{templateTypeName}Cache",
                TriviaList(
                    CarriageReturnLineFeed)))
            .WithModifiers(
                TokenList(
                    new[]{
                        Token(
                            TriviaList(Whitespace(whiteSpace)),
                            SyntaxKind.PublicKeyword,
                            TriviaList(
                                Space)),
                        Token(
                            TriviaList(),
                            SyntaxKind.ReadOnlyKeyword,
                            TriviaList(
                                Space))}))
            .WithKeyword(
                Token(
                    TriviaList(),
                    SyntaxKind.StructKeyword,
                    TriviaList(
                        Space)))
            .WithOpenBraceToken(
                Token(
                    TriviaList(Whitespace(whiteSpace)),
                    SyntaxKind.OpenBraceToken,
                    TriviaList(
                        CarriageReturnLineFeed)))
            .WithMembers(
                List(memberDeclarations))
            .WithLeadingTrivia(ElasticCarriageReturnLineFeed);
    }

    private static CompilationUnitSyntax WrapTemplateCacheInNamespace(StructDeclarationSyntax structDeclaration, NamespaceDeclarationSyntax namespaceDeclaration, string templateTypeName) =>
        CompilationUnit()
            .WithUsings(
                SingletonList(
                    UsingDirective(CreateQualifiedNameSyntax(templateTypeName))
                    .WithUsingKeyword(
                        Token(
                            TriviaList(),
                            SyntaxKind.UsingKeyword,
                            TriviaList(
                                Space)))
                    .WithSemicolonToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.SemicolonToken,
                            TriviaList(
                                CarriageReturnLineFeed)))))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(namespaceDeclaration
                        .WithNamespaceKeyword(
                            Token(
                                TriviaList(
                                    CarriageReturnLineFeed),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList(
                                    Space)))
                        .WithOpenBraceToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.OpenBraceToken,
                                TriviaList(
                                    CarriageReturnLineFeed)))
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                structDeclaration))
                .WithCloseBraceToken(
                    Token(
                        TriviaList(
                            Whitespace("    ")),
                        SyntaxKind.CloseBraceToken,
                        TriviaList(
                            CarriageReturnLineFeed)))))
                .NormalizeWhitespace();

    private static CompilationUnitSyntax WrapTemplateCacheInNamespace(StructDeclarationSyntax structDeclaration, FileScopedNamespaceDeclarationSyntax namespaceDeclaration, string templateTypeName) => 
            CompilationUnit()
                .WithUsings(
                    SingletonList(
                        UsingDirective(CreateQualifiedNameSyntax(templateTypeName))
                        .WithUsingKeyword(
                            Token(
                                TriviaList(),
                                SyntaxKind.UsingKeyword,
                                TriviaList(
                                    Space)))
                        .WithSemicolonToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.SemicolonToken,
                                TriviaList(
                                    CarriageReturnLineFeed)))))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(namespaceDeclaration
                        .WithNamespaceKeyword(
                            Token(
                                TriviaList(
                                    CarriageReturnLineFeed),
                                SyntaxKind.NamespaceKeyword,
                                TriviaList(
                                    Space)))
                        .WithSemicolonToken(
                            Token(
                                TriviaList(),
                                SyntaxKind.SemicolonToken,
                                TriviaList(
                                    CarriageReturnLineFeed)))
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                structDeclaration))));

    private static CompilationUnitSyntax WrapTemplateCacheInNamespace(StructDeclarationSyntax structDeclaration, string templateTypeName) =>
        CompilationUnit()
            .WithUsings(
                SingletonList(
                    UsingDirective(CreateQualifiedNameSyntax(templateTypeName))
                    .WithUsingKeyword(
                        Token(
                            TriviaList(),
                            SyntaxKind.UsingKeyword,
                            TriviaList(
                                Space)))
                    .WithSemicolonToken(
                        Token(
                            TriviaList(),
                            SyntaxKind.SemicolonToken,
                            TriviaList(
                                CarriageReturnLineFeed)))))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    structDeclaration));

    private static QualifiedNameSyntax CreateQualifiedNameSyntax(string templateTypeName)
        => templateTypeName == FullyQualifiedNames.SpaceTemplate ?
                QualifiedName(
                    IdentifierName("OrleanSpaces"),
                    IdentifierName("Tuples")) :
                QualifiedName(
                    QualifiedName(
                        IdentifierName("OrleanSpaces"),
                        IdentifierName("Tuples")),
                        IdentifierName("Specialized"));

    private class TemplateCacheNodeResult
    {
        public StructDeclarationSyntax? Node { get; }

        /// <summary>
        /// Indicates wether <see cref="Node"/> is part of the document the analyzer raised the issue for.
        /// </summary>
        public bool IsPartOfDocument { get; }

        public TemplateCacheNodeResult() : this(null, false) { }

        public TemplateCacheNodeResult(StructDeclarationSyntax? node, bool isInNewFile)
        {
            Node = node;
            IsPartOfDocument = isInNewFile;
        }
    }

    private class TemplateCacheFieldVisitor : CSharpSyntaxWalker
    {
        /// <summary>
        /// Represents how many <see cref="VariableDeclarationSyntax"/> of type 'new {X}Template(null, ... , null)' are present.
        /// </summary>
        public List<int> TuplesPresent { get; private set; } = new();

        private readonly string templateTypeName;

        public TemplateCacheFieldVisitor(string templateTypeName)
            => this.templateTypeName = templateTypeName;

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var identifierName = node.ChildNodes().OfType<IdentifierNameSyntax>().SingleOrDefault();
            if (identifierName?.ToString() == templateTypeName)
            {
                ValidateCachedFieldExistance();
                return;
            }

            var qualifiedName = node.ChildNodes().OfType<QualifiedNameSyntax>().SingleOrDefault();
            if (qualifiedName?.ChildNodes().OfType<IdentifierNameSyntax>().First().ToString() == templateTypeName)
            {
                ValidateCachedFieldExistance();
                return;
            }

            void ValidateCachedFieldExistance()
            {
                var baseObjectCreation = node
                    .ChildNodes().OfType<VariableDeclaratorSyntax>().FirstOrDefault()
                    .ChildNodes().OfType<EqualsValueClauseSyntax>().FirstOrDefault()
                    .ChildNodes().OfType<BaseObjectCreationExpressionSyntax>().FirstOrDefault();

                if (baseObjectCreation != null)
                {
                    int? count = baseObjectCreation.ArgumentList?.Arguments.Count;
                    if (count.HasValue)
                    {
                        count = count.Value == 0 ? 1 : count.Value;
                        TuplesPresent.Add((int)count);
                    }
                }
            }
        }
    }
}