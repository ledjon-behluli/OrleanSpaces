using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
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

        CodeAction action = CodeAction.Create(
           title: $"Use '{typeName}'",
           equivalenceKey: PreferSpecializedOverGenericAnalyzer.Diagnostic.Id,
           createChangedDocument: _ =>
           {
               var localDeclaration = node.TryGetNode<LocalDeclarationStatementSyntax>();
               if (localDeclaration is null)
               {
                   return Task.FromResult(context.Document.WithSyntaxRoot(root));
               }

               var variableDeclaration = localDeclaration.Declaration;
               var newVariableDeclaration = variableDeclaration.WithType(ParseTypeName($"{typeName} "));
               var newRoot = root.ReplaceNode(variableDeclaration, newVariableDeclaration).NormalizeWhitespace();

               var c = newRoot.ToFullString();

               return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
           });

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}
