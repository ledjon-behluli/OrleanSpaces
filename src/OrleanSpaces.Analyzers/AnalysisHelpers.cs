using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers;

internal static class AnalysisHelpers
{
    /// <summary>
    /// Determines if <paramref name="current"/> is of type <paramref name="candidate"/>.
    /// </summary>
    /// <param name="current">The actual symbol type at hand.</param>
    /// <param name="candidate">The symbol type interested in.</param>
    public static bool IsOfType(ITypeSymbol? current, ITypeSymbol? candidate) =>
        IsAnyOfType(current, new List<ITypeSymbol?>() { candidate });

    /// <summary>
    /// Determines if <paramref name="current"/> is of any of the types in <paramref name="candidates"/>.
    /// </summary>
    /// <param name="current">The actual symbol type at hand.</param>
    /// <param name="candidates">A collection of symbol types interested in.</param>
    public static bool IsAnyOfType(ITypeSymbol? current, IEnumerable<ITypeSymbol?> candidates)
    {
        if (current == null || candidates == null || candidates.Count() == 0)
        {
            return false;
        }

        return candidates.Any(candidate => candidate != null && SymbolEqualityComparer.Default.Equals(candidate, current));
    }
}