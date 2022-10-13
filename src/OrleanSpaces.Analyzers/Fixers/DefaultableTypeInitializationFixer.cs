using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using OrleanSpaces.Analyzers.Analyzers;
using System.Collections.Immutable;
using System.Composition;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OrleanSpaces.Analyzers.Fixers;

/// <summary>
/// Code fix provider for <see cref="DefaultableTypeInitializationAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefaultableTypeInitializationFixer)), Shared]
internal sealed class DefaultableTypeInitializationFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DefaultableTypeInitializationAnalyzer.Diagnostic.Id);
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        var node = root?.FindNode(context.Span);
        if (node == null)
        {
            return;
        }

        string? typeName = context.Diagnostics.First().Properties.GetValueOrDefault("typeName");
        if (typeName == null)
        {
            return;
        }

        CodeAction action = CodeAction.Create(
            title: $"Use '{typeName}.Null'",
            equivalenceKey: DefaultableTypeInitializationAnalyzer.Diagnostic.Id,
            createChangedDocument: ct =>
            {
                var newNode = MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(typeName),
                    IdentifierName("Null"));

                var newRoot = root?.ReplaceNode(node, newNode);

                return Task.FromResult(newRoot == null ? context.Document :
                    context.Document.WithSyntaxRoot(newRoot));
            });

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}