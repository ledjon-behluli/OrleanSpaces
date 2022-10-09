using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers.Helpers;

internal static class CompilationExtensions
{
    public static INamedTypeSymbol? GetTypeByFullName(this Compilation compilation, string? fullName)
    {
        if (string.IsNullOrEmpty(fullName))
        {
            return null;
        }

        var provider = WellKnownTypeProvider.GetOrCreate(compilation);
        return provider.GetTypeByFullName(fullName!);
    }

    public static bool IsClrType(this ITypeSymbol ts, Compilation compilation, Type clrType)
        => ts.OriginalDefinition.Equals(compilation.GetTypeByFullName(clrType.FullName), SymbolEqualityComparer.Default);

    public static INamedTypeSymbol? GetBestTypeByMetadataName(this Compilation compilation, string fullyQualifiedMetadataName)
    {
        // Try to get the unique type with this name, ignoring accessibility
        var type = compilation.GetTypeByMetadataName(fullyQualifiedMetadataName);

        // Otherwise, try to get the unique type with this name originally defined in 'compilation'
        type ??= compilation.Assembly.GetTypeByMetadataName(fullyQualifiedMetadataName);

        // Otherwise, try to get the unique accessible type with this name from a reference
        if (type is null)
        {
            foreach (var module in compilation.Assembly.Modules)
            {
                foreach (var referencedAssembly in module.ReferencedAssemblySymbols)
                {
                    var currentType = referencedAssembly.GetTypeByMetadataName(fullyQualifiedMetadataName);
                    if (currentType is null)
                    {
                        continue;
                    }

                    switch (currentType.GetResultantVisibility())
                    {
                        case SymbolVisibility.Public:
                        case SymbolVisibility.Internal when referencedAssembly.GivesAccessTo(compilation.Assembly):
                            break;

                        default:
                            continue;
                    }

                    if (type is not null)
                    {
                        // Multiple visible types with the same metadata name are present
                        return null;
                    }

                    type = currentType;
                }
            }
        }

        return type;
    }
}
