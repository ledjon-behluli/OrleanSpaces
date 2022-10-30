using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers.OSA003;

/// <summary>
/// Code fix provider for <see cref="AllUnitArgsTemplateInitAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AllUnitArgsTemplateInitFixer)), Shared]
internal sealed class AllUnitArgsTemplateInitFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id);
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

        StructDeclarationSyntax? cacheDeclaration = await GetSpaceTemplateCache(context.Document, context.CancellationToken);
        SpaceTemplateCachedFieldChecker checker = new(numOfSpaceUnits);        
        CodeAction? action = null;

        var newNode = MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("SpaceTemplateCache"),
                IdentifierName($"Tuple_{numOfSpaceUnits}"));

        checker.Visit(cacheDeclaration);

        if (checker.CachedFieldExists)
        {
            action = CodeAction.Create(
                title: $"Use 'SpaceTemplateCache.Tuple_{numOfSpaceUnits}'",
                equivalenceKey: AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id,
                createChangedDocument: _ =>
                {
                    var newRoot = root.ReplaceNode(node, newNode);

                    return Task.FromResult(newRoot == null ? context.Document :
                        context.Document.WithSyntaxRoot(newRoot));
                });
        }
        else
        {
            action = CodeAction.Create(
                title: $"Create wrapper around a cached '{numOfSpaceUnits}-tuple' reference.",
                equivalenceKey: AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id,
                createChangedDocument: _ =>
                {
                    var newRoot = root.ReplaceNode(node, newNode);
                    if (newRoot != null)
                    {
                        var (namespaceNode, @namespace) = newRoot.GetNamespaceParts();
                        if (namespaceNode != null)
                        {
                            var cacheNode = GenerateSpaceTemplateCache(numOfSpaceUnits);
                            newRoot = newRoot.InsertNodesAfter(namespaceNode, new SyntaxNode[]
                            {
                                cacheNode.WithLeadingTrivia(ElasticCarriageReturnLineFeed)
                            });

                            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
                        }
                    }

                    return Task.FromResult(context.Document);
                });
        }

        context.RegisterCodeFix(action, context.Diagnostics);
    }

    private static async Task<StructDeclarationSyntax?> GetSpaceTemplateCache(Document document, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken);
        if (root == null)
        {
            return null;
        }

        var cacheDeclaration = root.DescendantNodes().OfType<StructDeclarationSyntax>()
            .FirstOrDefault(syntax => syntax.Identifier.ToString() == "SpaceTemplateCache");

        return cacheDeclaration;
    }

    private static StructDeclarationSyntax GenerateSpaceTemplateCache(int argCount)
    {
        SeparatedSyntaxList<ArgumentSyntax> syntaxList;

        if (argCount == 1)
        {
            syntaxList = SingletonSeparatedList(CreateSpaceUnitArgumentSyntax());
        }
        else
        {
            int syntaxNodeOrTokenCount = 2 * argCount - 1;
            var syntaxNodeOrTokens = new SyntaxNodeOrToken[syntaxNodeOrTokenCount];

            for (int i = 0; i < syntaxNodeOrTokenCount; i++)
            {
                syntaxNodeOrTokens[i] = i % 2 == 0 ? CreateSpaceUnitArgumentSyntax() : Token(TriviaList(), SyntaxKind.CommaToken, TriviaList(Space));
            }

            syntaxList = SeparatedList<ArgumentSyntax>(syntaxNodeOrTokens);
        }

        string fieldName = $"tuple_{argCount}";
        string propertyName = $"Tuple_{argCount}";
        string diagnosticId = AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id;

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
                    List(
                        new MemberDeclarationSyntax[]{
                            FieldDeclaration(
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
                                                        Space)))))))
                            .WithModifiers(
                                TokenList(
                                    new []{
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
                                        CarriageReturnLineFeed))),
                            PropertyDeclaration(
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
                                        Space)))
                            .WithModifiers(
                                TokenList(
                                    new []{
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
                                        CarriageReturnLineFeed)))}));
    }

    private static ArgumentSyntax CreateSpaceUnitArgumentSyntax() =>
        Argument(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("SpaceUnit"),
                IdentifierName("Null")));

    private class SpaceTemplateCachedFieldChecker : CSharpSyntaxWalker
    {
        private readonly int numOfSpaceUnits;

        public bool CachedFieldExists { get; private set; }

        public SpaceTemplateCachedFieldChecker(int numOfSpaceUnits)
        {
            this.numOfSpaceUnits = numOfSpaceUnits;
        }

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
                    .ChildNodes().OfType<VariableDeclaratorSyntax>().First()
                    .ChildNodes().OfType<EqualsValueClauseSyntax>().First()
                    .ChildNodes().OfType<BaseObjectCreationExpressionSyntax>().First();

                if (baseObjectCreation != null)
                {
                    int? count = baseObjectCreation.ArgumentList?.Arguments.Count;
                    if (count.HasValue)
                    {
                        count = count.Value == 0 ? 1 : count.Value;
                        if (count == numOfSpaceUnits)
                        {
                            CachedFieldExists = true;
                        }
                    }
                }
            }
        }
    }
}