using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Immutable;
using System.Composition;

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

        var cacheNode = await GetSpaceTemplateCacheNode(context.Document, context.CancellationToken);
        if (cacheNode != null)  // SpaceTemplateCache already exists
        {
            SpaceTemplateCacheFieldVisitor visitor = new();
            visitor.Visit(cacheNode);

            if (visitor.TuplesPresent.Contains(numOfSpaceUnits))   // Appropriate ref field already exists
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
            }
            else   // Add appropriate ref field
            {
                visitor.TuplesPresent.Add(numOfSpaceUnits);
                var newCacheNode = CreateSpaceTemplateCache(visitor.TuplesPresent.ToArray());

                context.RegisterCodeFix(
                    CodeAction.Create(
                        title: $"Add and use 'SpaceTemplateCache.Tuple_{numOfSpaceUnits}'",
                        equivalenceKey: SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id,
                        createChangedDocument: async ct =>
                        {
                            var documentEditor = await DocumentEditor.CreateAsync(context.Document, ct);

                            documentEditor.ReplaceNode(cacheNode, newCacheNode);
                            documentEditor.ReplaceNode(node, newNode);

                            var newDocument = documentEditor.GetChangedDocument();
                            return newDocument;

                        }), context.Diagnostics);
            }
        }
        else   // SpaceTemplateCache does not exist - Create & Add appropriate ref field
        {
            var fixInFileAction = CodeAction.Create(
                title: $"Create wrapper around a cached '{numOfSpaceUnits}-tuple' reference.",
                equivalenceKey: $"{SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id}-1",
                createChangedDocument: _ =>
                {
                    var newRoot = root.ReplaceNode(node, newNode);
                    if (newRoot != null)
                    {
                        var (namespaceNode, @namespace) = newRoot.GetNamespaceParts();
                        if (namespaceNode != null)
                        {
                            var cacheNode = CreateSpaceTemplateCache(new int[] { numOfSpaceUnits });
                            newRoot = newRoot.InsertNodesAfter(namespaceNode, new SyntaxNode[] { cacheNode });

                            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
                        }
                    }

                    return Task.FromResult(context.Document);
                });

            var fixInNewFileAction = CodeAction.Create(
                title: $"Create wrapper around a cached '{numOfSpaceUnits}-tuple' reference in a new file.",
                equivalenceKey: $"{SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id}-2",
                createChangedDocument: async ct =>
                {
                    var solutionFilePath = context.Document.Project.Solution.FilePath;
                    if (solutionFilePath != null)
                    {
                        using var workspace = MSBuildWorkspace.Create();

                        var solution = await workspace.OpenSolutionAsync(solutionFilePath: solutionFilePath, cancellationToken: ct);

                        var newSolution = solution.AddDocument(
                            documentId: DocumentId.CreateNewId(context.Document.Project.Id, "SpaceTemplateCache_DEBUG.cs"),
                            name: "SpaceTemplateCache.cs",
                            text: CreateSpaceTemplateCache(new int[] { numOfSpaceUnits }).ToFullString());

                        if (workspace.CanApplyChange(ApplyChangesKind.AddDocument))
                        {
                            if (workspace.TryApplyChanges(newSolution))
                            {
                                var newRoot = root.ReplaceNode(node, newNode);
                                return context.Document.WithSyntaxRoot(newRoot);
                            }
                        }
                    }

                    return context.Document;
                });

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: $"Create wrapper around a cached '{numOfSpaceUnits}-tuple' reference.",
                    nestedActions: ImmutableArray.Create(fixInFileAction, fixInNewFileAction),
                    isInlinable: true),
                context.Diagnostics);
        }
    }

    //private async Task<Solution> CreateChangedSolutionAsync(CodeFixContext context, Diagnostic diagnostic, CancellationToken cancellation)
    //{
    //    var projectDoc = context.Document.Project.AdditionalDocuments
    //        .FirstOrDefault(doc => doc.FilePath.EndsWith(".csproj"));

    //    if (projectDoc == null)
    //        return null;

    //    var documentSyntax = Parser.ParseText((await projectDoc.GetTextAsync()).ToString());
    //    var newNode = documentSyntax.Accept(new AddPropertyVisitor("AndroidUseIntermediateDesignerFile", "True"));

    //    var text = await projectDoc.GetTextAsync(cancellation);
    //    var newDoc = context.Document.Project.Solution
    //        .WithAdditionalDocumentText(projectDoc.Id, SourceText.From(
    //            newNode.ToFullString(), text.Encoding))
    //        .GetProject(context.Document.Project.Id)
    //        .GetDocument(context.Document.Id);

    //    return newDoc.Project.Solution;
    //}

    private static async Task<StructDeclarationSyntax?> GetSpaceTemplateCacheNode(Document document, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        if (root == null)
        {
            return null;
        }

        var cacheDeclaration = root.DescendantNodes().OfType<StructDeclarationSyntax>()
            .FirstOrDefault(syntax => syntax.Identifier.ValueText == "SpaceTemplateCache");

        return cacheDeclaration;
    }

    private static StructDeclarationSyntax CreateSpaceTemplateCache(int[] args)
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
                syntaxList = SingletonSeparatedList(CreateReplacementNode());
            }
            else
            {
                int syntaxNodeOrTokenCount = 2 * args[i] - 1;
                var syntaxNodeOrTokens = new SyntaxNodeOrToken[syntaxNodeOrTokenCount];

                for (int j = 0; j < syntaxNodeOrTokenCount; j++)
                {
                    syntaxNodeOrTokens[j] = j % 2 == 0 ? CreateReplacementNode() : Token(TriviaList(), SyntaxKind.CommaToken, TriviaList(Space));
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

            if (i == 0)      // first field declaration
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
    }

    private static ArgumentSyntax CreateReplacementNode() =>
        Argument(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("SpaceUnit"),
                IdentifierName("Null")));

    private class SpaceTemplateCacheFieldVisitor : CSharpSyntaxWalker
    {
        /// <summary>
        /// Represents how many <see cref="VariableDeclarationSyntax"/> of type 'new SpaceTemplate(SpaceUnit.Null, ..., SpaceUnit.Null)' are present.
        /// </summary>
        public List<int> TuplesPresent { get; private set; } = new();

        public override string ToString() => string.Join(", ", TuplesPresent);

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