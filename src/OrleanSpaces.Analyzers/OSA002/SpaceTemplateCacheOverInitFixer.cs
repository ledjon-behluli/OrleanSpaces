using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Rename;

namespace OrleanSpaces.Analyzers.OSA002;

/// <summary>
/// Code fix provider for <see cref="SpaceTemplateCacheOverInitAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SpaceTemplateCacheOverInitFixer)), Shared]
internal sealed class SpaceTemplateCacheOverInitFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id);
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

        if (!int.TryParse(context.Diagnostics.First().Properties.GetValueOrDefault("numOfSpaceUnits"), out int numOfSpaceUnits) ||
            numOfSpaceUnits < 1)
        {
            return;
        }

        var newNode = MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName("SpaceTemplateCache"),
            IdentifierName($"Tuple_{numOfSpaceUnits}"));

        var cacheNodeResult = await GetSpaceTemplateCacheNodeAsync(context.Document.Project.Solution, context.Document, context.CancellationToken);
        var cacheNode = cacheNodeResult.Node;

        // SpaceTemplateCache does not exist - Create & Add appropriate member
        if (cacheNode == null)
        {
            var fixInFileAction = CodeAction.Create(
                title: $"Cache value as a '{numOfSpaceUnits}-tuple' static readonly reference in this file.",
                equivalenceKey: SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id,
                createChangedDocument: _ =>
                {
                    var newRoot = root.ReplaceNode(node, newNode);
                    if (newRoot != null)
                    {
                        var namespaceNode = newRoot.TryGetNamespaceNode();
                        if (namespaceNode != null)
                        {
                            cacheNode = CreateSpaceTemplateCacheNode(new int[] { numOfSpaceUnits });
                            newRoot = newRoot.InsertNodesAfter(namespaceNode, new SyntaxNode[] { cacheNode });

                            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
                        }
                    }

                    return Task.FromResult(context.Document);
                });

            var fixInNewFileAction = CodeAction.Create(
                title: $"Cache value as a '{numOfSpaceUnits}-tuple' static readonly reference in a new file.",
                equivalenceKey: SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id,
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
                                cacheNode = CreateSpaceTemplateCacheNode(new int[] { numOfSpaceUnits });

                                if (namespaceNode is NamespaceDeclarationSyntax nd)
                                {
                                    compilationUnit = WrapSpaceTemplateCacheInNamespace(cacheNode, nd);
                                }

                                if (namespaceNode is FileScopedNamespaceDeclarationSyntax fsnd)
                                {
                                    compilationUnit = WrapSpaceTemplateCacheInNamespace(cacheNode, fsnd);
                                }

                                if (namespaceNode is GlobalStatementSyntax)
                                {
                                    compilationUnit = WrapSpaceTemplateCacheInNamespace(cacheNode);
                                }

                                if (compilationUnit != null)
                                {
                                    newSolution = newSolution.AddAdditionalDocument(
                                        documentId: DocumentId.CreateNewId(context.Document.Project.Id),
                                        folders: context.Document.Folders,
                                        name: "SpaceTemplateCache.cs",
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
                    title: "Cache value as a static readonly reference.",
                    nestedActions: ImmutableArray.Create(fixInFileAction, fixInNewFileAction),
                    isInlinable: false),
                context.Diagnostics);

            return;
        }

        // SpaceTemplateCache already exists
        SpaceTemplateCacheFieldVisitor visitor = new();
        visitor.Visit(cacheNode);

        // Appropriate member already exists
        if (visitor.TuplesPresent.Contains(numOfSpaceUnits))   
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: $"Use 'SpaceTemplateCache.Tuple_{numOfSpaceUnits}'",
                    equivalenceKey: SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id,
                    createChangedDocument: _ =>
                    {
                        var newRoot = root.ReplaceNode(node, newNode);

                        return Task.FromResult(newRoot == null ? context.Document :
                            context.Document.WithSyntaxRoot(newRoot));

                    }), context.Diagnostics);

            return;
        }

        // Appropriate member does not exist
        visitor.TuplesPresent.Add(numOfSpaceUnits);
        int[] args = visitor.TuplesPresent.ToArray();

        // Add appropriate member to SpaceTemplateCache (part of the document under analysis)
        if (cacheNodeResult.IsPartOfDocument)
        {
            context.RegisterCodeFix(
               CodeAction.Create(
                   title: $"Add and use 'SpaceTemplateCache.Tuple_{numOfSpaceUnits}'",
                   equivalenceKey: SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id,
                   createChangedDocument: async ct =>
                   {
                       var documentEditor = await DocumentEditor.CreateAsync(context.Document, ct);

                       // Cache Node
                       var newCacheNode = CreateSpaceTemplateCacheNode(args);
                       documentEditor.ReplaceNode(cacheNode, newCacheNode);

                       // Node
                       documentEditor.ReplaceNode(node, newNode);

                       // Result
                       var newDocument = documentEditor.GetChangedDocument();
                       return newDocument;

                   }), context.Diagnostics);

            return;
        }

        // Add appropriate member to SpaceTemplateCache (part of another document)
        context.RegisterCodeFix(
            CodeAction.Create(
                title: $"Add and use 'SpaceTemplateCache.Tuple_{numOfSpaceUnits}'",
                equivalenceKey: SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id,
                createChangedSolution: async ct =>
                {
                    var solution = context.Document.Project.Solution;
                    var solutionEditor = new SolutionEditor(solution);

                    // Cache Node
                    var cacheNodeDocumentId = solution.GetDocumentId(cacheNode.SyntaxTree);
                    var newCacheNode = CreateSpaceTemplateCacheNode(args);
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

    private static async Task<SpaceTemplateCacheNodeResult> GetSpaceTemplateCacheNodeAsync(Solution solution, Document document, CancellationToken cancellationToken)
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

            var cacheDeclaration = root.DescendantNodes().OfType<StructDeclarationSyntax>().FirstOrDefault(syntax => syntax.Identifier.ValueText == "SpaceTemplateCache");
            if (cacheDeclaration != null)
            {
                return new(cacheDeclaration, _document.Id.Equals(document.Id));
            }
        }

        return new();
    }

    private static StructDeclarationSyntax CreateSpaceTemplateCacheNode(int[] args)
    {
        args = args.OrderBy(x => x).ToArray();
        string diagnosticId = SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id;

        List<FieldDeclarationSyntax> fieldDeclarations = new();
        List<PropertyDeclarationSyntax> propertyDeclarations = new();

        for (int i = 0; i < args.Length; i++)
        {
            SeparatedSyntaxList<ArgumentSyntax> syntaxList;

            if (args[i] == 1)
            {
                syntaxList = SingletonSeparatedList(CreateArgumentNode());
            }
            else
            {
                int syntaxNodeOrTokenCount = 2 * args[i] - 1;
                var syntaxNodeOrTokens = new SyntaxNodeOrToken[syntaxNodeOrTokenCount];

                for (int j = 0; j < syntaxNodeOrTokenCount; j++)
                {
                    syntaxNodeOrTokens[j] = j % 2 == 0 ? CreateArgumentNode() : Token(TriviaList(), SyntaxKind.CommaToken, TriviaList(Space));
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
                            "SpaceTemplate",
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
                            "SpaceTemplate",
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
                                    Whitespace("    ")),
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
                                        Whitespace("    ")),
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
                                    Whitespace("    ")),
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
                                        Whitespace("    ")),
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

        IEnumerable<MemberDeclarationSyntax> memberDeclarations = fieldDeclarations
            .Cast<MemberDeclarationSyntax>()
            .Concat(propertyDeclarations.Cast<MemberDeclarationSyntax>());

        return StructDeclaration(
            Identifier(
                TriviaList(),
                "SpaceTemplateCache",
                TriviaList(
                    CarriageReturnLineFeed)))
            .WithModifiers(
                TokenList(
                    new[]{
                        Token(
                            TriviaList(),
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
                    TriviaList(),
                    SyntaxKind.OpenBraceToken,
                    TriviaList(
                        CarriageReturnLineFeed)))
            .WithMembers(
                List(memberDeclarations))
            .WithLeadingTrivia(ElasticCarriageReturnLineFeed);

        static ArgumentSyntax CreateArgumentNode() =>
            Argument(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("SpaceUnit"),
                    IdentifierName("Null")));
    }

    private static CompilationUnitSyntax WrapSpaceTemplateCacheInNamespace(StructDeclarationSyntax structDeclaration, NamespaceDeclarationSyntax namespaceDeclaration) =>
        CompilationUnit()
            .WithUsings(
                SingletonList(
                    UsingDirective(
                        QualifiedName(
                            IdentifierName(FullyQualifiedNames.SpaceTemplate.Split('.')[0]),
                            IdentifierName(FullyQualifiedNames.SpaceTemplate.Split('.')[1])))
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

    private static CompilationUnitSyntax WrapSpaceTemplateCacheInNamespace(StructDeclarationSyntax structDeclaration, FileScopedNamespaceDeclarationSyntax namespaceDeclaration) => 
            CompilationUnit()
                .WithUsings(
                    SingletonList(
                        UsingDirective(
                            QualifiedName(
                                IdentifierName(FullyQualifiedNames.SpaceTemplate.Split('.')[0]),
                                IdentifierName(FullyQualifiedNames.SpaceTemplate.Split('.')[1])))
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

    private static CompilationUnitSyntax WrapSpaceTemplateCacheInNamespace(StructDeclarationSyntax structDeclaration) =>
            CompilationUnit()
                .WithUsings(
                    SingletonList(
                        UsingDirective(
                            QualifiedName(
                                IdentifierName(FullyQualifiedNames.SpaceTemplate.Split('.')[0]),
                                IdentifierName(FullyQualifiedNames.SpaceTemplate.Split('.')[1])))
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

    private class SpaceTemplateCacheNodeResult
    {
        public StructDeclarationSyntax? Node { get; }

        /// <summary>
        /// Indicates wether <see cref="Node"/> is part of the document the analyzer raised the issue for.
        /// </summary>
        public bool IsPartOfDocument { get; }

        public SpaceTemplateCacheNodeResult() : this(null, false) { }

        public SpaceTemplateCacheNodeResult(StructDeclarationSyntax? node, bool isInNewFile)
        {
            Node = node;
            IsPartOfDocument = isInNewFile;
        }
    }

    private class SpaceTemplateCacheFieldVisitor : CSharpSyntaxWalker
    {
        /// <summary>
        /// Represents how many <see cref="VariableDeclarationSyntax"/> of type 'new SpaceTemplate(SpaceUnit.Null, ... , SpaceUnit.Null)' are present.
        /// </summary>
        public List<int> TuplesPresent { get; private set; } = new();

        public override void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var identifierName = node.ChildNodes().OfType<IdentifierNameSyntax>().SingleOrDefault();
            if (identifierName?.ToString() == "SpaceTemplate")
            {
                ValidateCachedFieldExistance();
                return;
            }

            var qualifiedName = node.ChildNodes().OfType<QualifiedNameSyntax>().SingleOrDefault();
            if (qualifiedName?.ChildNodes().OfType<IdentifierNameSyntax>().First().ToString() == "SpaceTemplate")
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