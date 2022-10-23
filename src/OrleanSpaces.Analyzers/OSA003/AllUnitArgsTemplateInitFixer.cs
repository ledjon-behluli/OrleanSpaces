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

        if (int.TryParse(context.Diagnostics.First().Properties.GetValueOrDefault("numOfSpaceUnits"), out int numOfSpaceUnits))
        {
            if (numOfSpaceUnits > 0)
            {
                CodeAction action = CodeAction.Create(
                    title: $"Create factory which exposes a cached '{numOfSpaceUnits}-tuple' reference.",
                    equivalenceKey: AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id,
                    createChangedDocument: ct =>
                    {
                        var newNode = MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("SpaceTemplateFactory"),
                            IdentifierName($"Tuple_{numOfSpaceUnits}"));

                        var newRoot = root.ReplaceNode(node, newNode);
                        var factoryNode = GenerateSpaceTemplateFactory(numOfSpaceUnits);

                        newRoot?.InsertNodesAfter(root, new SyntaxNode[] { factoryNode });
                    
                        return Task.FromResult(newRoot == null ? context.Document :
                            context.Document.WithSyntaxRoot(newRoot));
                    });

                context.RegisterCodeFix(action, context.Diagnostics);
            }
        }
    }

    private static StructDeclarationSyntax GenerateSpaceTemplateFactory(int argCount)
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

        return StructDeclaration(
                    Identifier(
                        TriviaList(),
                        "SpaceTemplateFactory",
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
}