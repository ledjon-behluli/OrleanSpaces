using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers.Helpers;

internal enum SymbolVisibility
{
    Public,
    Internal,
    Private,
}

internal static class SymbolExtensions
{
    public static SymbolVisibility GetResultantVisibility(this ISymbol symbol)
    {
        var visibility = SymbolVisibility.Public;

        switch (symbol.Kind)
        {
            case SymbolKind.Alias:
                return SymbolVisibility.Private;
            case SymbolKind.Parameter:
                return symbol.ContainingSymbol.GetResultantVisibility();
            case SymbolKind.TypeParameter:
                return SymbolVisibility.Private;
        }

        while (symbol != null && symbol.Kind != SymbolKind.Namespace)
        {
            switch (symbol.DeclaredAccessibility)
            {
                case Accessibility.NotApplicable:
                case Accessibility.Private:
                    return SymbolVisibility.Private;
                case Accessibility.Internal:
                case Accessibility.ProtectedAndInternal:
                    visibility = SymbolVisibility.Internal;
                    break;
            }

            symbol = symbol.ContainingSymbol;
        }

        return visibility;
    }
}