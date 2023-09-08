using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers.OSA004;

/// <summary>
/// Code fix provider for <see cref="PreferSpecializedOverGenericAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PreferSpecializedOverGenericFixer)), Shared]
internal sealed class PreferSpecializedOverGenericFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(PreferSpecializedOverGenericAnalyzer.Diagnostic.Id);
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root is null)
        {
            return;
        }

        var node = root.FindNode(context.Span);
        if (node is null)
        {
            return;
        }

        string? typeName = context.Diagnostics.FirstOrDefault()?.Properties.GetValueOrDefault("SpecializedTypeName");
        if (typeName is null)
        {
            return;
        }

        var localDeclaration = node.TryGetNode<LocalDeclarationStatementSyntax>();
        if (localDeclaration is null)
        {
            return;
        }

        VariableDeclarationSyntax newVariableDeclaration;

        var variableDeclaration = localDeclaration.Declaration;
        var variable = variableDeclaration.Variables[0];

        if (variable.Initializer?.Value is ObjectCreationExpressionSyntax { } objectCreation)
        {
            var arguments = objectCreation.ArgumentList?.Arguments ?? new();

            if (variableDeclaration.Type.IsVar)
            {
                // example: var tuple = new SpaceTuple(1);
                newVariableDeclaration =
                    VariableDeclaration(
                       IdentifierName(
                           Identifier(
                               TriviaList(),
                               SyntaxKind.VarKeyword,
                               "var",
                               "var",
                               TriviaList())))
                       .WithVariables(
                           SingletonSeparatedList(
                               variable.WithInitializer(
                                   EqualsValueClause(
                                       ObjectCreationExpression(
                                           IdentifierName(typeName))
                                       .WithArgumentList(
                                           ArgumentList(arguments))))))
                       .NormalizeWhitespace();
            }
            else
            {
                // example: SpaceTuple tuple = new SpaceTuple(1);
                newVariableDeclaration =
                     VariableDeclaration(
                             IdentifierName(typeName))
                             .WithVariables(
                                 SingletonSeparatedList(
                                     variable.WithInitializer(
                                         EqualsValueClause(
                                             ObjectCreationExpression(
                                                 IdentifierName(typeName))
                                             .WithArgumentList(
                                                 ArgumentList(arguments))))));

                newVariableDeclaration = newVariableDeclaration
                     .WithType(ParseTypeName(typeName))
                     .NormalizeWhitespace();
            }
        }
        else
        {
            var implicitobjectCreation = variable.Initializer?.Value as ImplicitObjectCreationExpressionSyntax;
            var arguments = implicitobjectCreation?.ArgumentList?.Arguments ?? new();

            // example: SpaceTuple tuple = new(1);
            newVariableDeclaration =
                VariableDeclaration(
                    IdentifierName(typeName))
                    .WithVariables(
                        SingletonSeparatedList(
                            variable.WithInitializer(
                                EqualsValueClause(
                                    ImplicitObjectCreationExpression()
                                    .WithArgumentList(
                                        ArgumentList(arguments))))));

            newVariableDeclaration = newVariableDeclaration
                 .WithType(ParseTypeName(typeName))
                 .NormalizeWhitespace();
        }

        var newRoot = root.ReplaceNode(variableDeclaration, newVariableDeclaration).NormalizeWhitespace();

        CodeAction action = CodeAction.Create(
           title: $"Use '{typeName}'",
           equivalenceKey: PreferSpecializedOverGenericAnalyzer.Diagnostic.Id,
           createChangedDocument: _ => Task.FromResult(context.Document.WithSyntaxRoot(newRoot)));

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}
