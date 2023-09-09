using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
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

        var localDeclaration = node.TryGetParentNode<LocalDeclarationStatementSyntax>();
        if (localDeclaration is null)
        {
            return;
        }

        SyntaxNode? newRoot;

        var variableDeclaration = localDeclaration.Declaration;
        var expression = variableDeclaration.Variables[0].Initializer?.Value;

        if (expression is ObjectCreationExpressionSyntax { } objectCreation)
        {
            var arguments = objectCreation.ArgumentList?.Arguments ?? new();

            if (variableDeclaration.Type.IsVar)
            {
                // example: var tuple = new SpaceTuple(1);
                ObjectCreationExpressionSyntax newObjectCreation = CreateObjectExpression(typeName, arguments);
                newRoot = root.ReplaceNode(objectCreation, newObjectCreation);
            }
            else
            {
                // example: SpaceTuple tuple = new SpaceTuple(1);
                var documentEditor = await DocumentEditor.CreateAsync(context.Document, context.CancellationToken);

                ObjectCreationExpressionSyntax newObjectCreation = CreateObjectExpression(typeName, arguments);
                documentEditor.ReplaceNode(objectCreation, newObjectCreation);

                var identifierName = variableDeclaration.TryGetChildNode<IdentifierNameSyntax>();
                if (identifierName is null)
                {
                    return;
                }

                var newIdentifierName = IdentifierName(typeName);
                documentEditor.ReplaceNode(identifierName, newIdentifierName);

                newRoot = await documentEditor.GetChangedDocument().GetSyntaxRootAsync(context.CancellationToken);
            }
        }
        else
        {
            // example: SpaceTuple tuple = new(1);
            var implicitObjectCreation = expression as ImplicitObjectCreationExpressionSyntax;
            var arguments = implicitObjectCreation?.ArgumentList?.Arguments ?? new();

            var identifierName = variableDeclaration.TryGetChildNode<IdentifierNameSyntax>();
            if (identifierName is null)
            {
                return;
            }

            var newIdentifierName = IdentifierName(typeName);
            newRoot = root.ReplaceNode(identifierName, newIdentifierName);
        }

        CodeAction action = CodeAction.Create(
           title: $"Use '{typeName}'",
           equivalenceKey: PreferSpecializedOverGenericAnalyzer.Diagnostic.Id,
           createChangedDocument: _ => Task.FromResult(newRoot is null ?
               context.Document.WithSyntaxRoot(root) :
               context.Document.WithSyntaxRoot(newRoot)));

        context.RegisterCodeFix(action, context.Diagnostics);
    }

    private static ObjectCreationExpressionSyntax CreateObjectExpression(
        string typeName, SeparatedSyntaxList<ArgumentSyntax> arguments) =>
            ObjectCreationExpression(
                IdentifierName(typeName))
            .WithArgumentList(
                ArgumentList(arguments));
}
