using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers.SpaceTuple;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefaultCtorCodeFixProvider)), Shared]
internal sealed class DefaultCtorCodeFixProvider : CodeFixProvider
{
    private const string title = "Prefer using 'SpaceTuple.Null' over default constructor.";

    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DefaultCtorAnalyzer.DiagnosticId);
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        Diagnostic diagnostic = context.Diagnostics.First();
        TextSpan span = diagnostic.Location.SourceSpan;

        ObjectCreationExpressionSyntax syntax = root
            .FindToken(span.Start).Parent
            .AncestorsAndSelf()
            .OfType<ObjectCreationExpressionSyntax>()
            .First();

        CodeAction action = CodeAction.Create(
            title: title,
            equivalenceKey: null,
            createChangedDocument: ct => FixAsync(context.Document, syntax, ct));

        context.RegisterCodeFix(action, diagnostic);
    }

    private async Task<Solution> FixAsync(Document document, ObjectCreationExpressionSyntax syntax, CancellationToken cancellationToken)
    {
        // Compute new uppercase name.
        var identifierToken = syntax.Identifier;
        var newName = identifierToken.Text.ToUpperInvariant();

        // Get the symbol representing the type to be renamed.
        var semanticModel = await document.GetSemanticModelAsync(cancellationToken);
        var typeSymbol = semanticModel.GetDeclaredSymbol(syntax, cancellationToken);

        // Produce a new solution that has all references to that type renamed, including the declaration.
        var originalSolution = document.Project.Solution;
        var optionSet = originalSolution.Workspace.Options;
        var newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, typeSymbol, newName, optionSet, cancellationToken).ConfigureAwait(false);

        // Return the new solution with the now-uppercase type name.
        return newSolution;
    }
}