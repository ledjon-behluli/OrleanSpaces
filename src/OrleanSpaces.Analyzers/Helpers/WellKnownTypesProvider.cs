using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers.Helpers;

internal class WellKnownTypeProvider
{
    private static readonly ConditionalWeakTable<Compilation, Lazy<WellKnownTypeProvider>> cache = new();
    private readonly ConcurrentDictionary<string, INamedTypeSymbol?> fullNamedTypeMap;

    private WellKnownTypeProvider(Compilation compilation)
    {
        Compilation = compilation;
        fullNamedTypeMap = new ConcurrentDictionary<string, INamedTypeSymbol?>(StringComparer.Ordinal);
    }

    public static WellKnownTypeProvider GetOrCreate(Compilation compilation)
    {
        var cachedValue = cache.GetValue(compilation, static compilation =>
            new Lazy<WellKnownTypeProvider>(() => new WellKnownTypeProvider(compilation)));

        return cachedValue.Value;
    }

    public Compilation Compilation { get; }

    public INamedTypeSymbol? GetTypeByFullName(string fullName)
    {
        return fullNamedTypeMap.GetOrAdd(fullName, x => Compilation.GetBestTypeByMetadataName(x));
    }
}