using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using OrleanSpaces.Analyzers.OSA001;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers.OSA002;

/// <summary>
/// Code fix provider for <see cref="TupleRefOverInitAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NotSupportedTupleFieldTypeFixer)), Shared]
internal sealed class NotSupportedTupleFieldTypeFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Id);
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        var node = root?.FindNode(context.Span);
        if (node == null)
        {
            return;
        }

        CodeAction action = CodeAction.Create(
            title: "Remove unsupported argument.",
            equivalenceKey: NotSupportedTupleFieldTypeAnalyzer.Diagnostic.Id,
            createChangedDocument: ct =>
            {
                var newRoot = root?.RemoveNode(node, SyntaxRemoveOptions.KeepNoTrivia);

                return Task.FromResult(newRoot == null ? context.Document :
                    context.Document.WithSyntaxRoot(newRoot));
            });

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}