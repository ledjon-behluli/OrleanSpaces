using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
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

        var node = root?.FindNode(context.Span);
        if (node == null)
        {
            return;
        }

        if (int.TryParse(
            context.Diagnostics.First().Properties.GetValueOrDefault("numOfSpaceUnits"),
            out int numOfSpaceUnits))
        {
            if (numOfSpaceUnits > 0)
            {
                CodeAction action = CodeAction.Create(
                title: $"Create readonly struct which exposes a '{numOfSpaceUnits}-tuple' as reference.",
                equivalenceKey: AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id,
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
    }
}