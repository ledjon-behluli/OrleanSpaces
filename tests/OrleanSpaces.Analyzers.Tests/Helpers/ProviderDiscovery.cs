using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio.Composition;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.Tests;

public static class ProviderDiscovery
{
    private static readonly Lazy<IExportProviderFactory> factory;

    static ProviderDiscovery()
    {
        factory = new Lazy<IExportProviderFactory>(
            () =>
            {
                var discovery = new AttributedPartDiscovery(Resolver.DefaultInstance, isNonPublicSupported: true);
                var parts = Task.Run(() => discovery.CreatePartsAsync(typeof(DefaultableCodeFixProvider).Assembly)).GetAwaiter().GetResult();
                var catalog = ComposableCatalog.Create(Resolver.DefaultInstance).AddParts(parts);

                var configuration = CompositionConfiguration.Create(catalog);
                var runtimeComposition = RuntimeComposition.CreateRuntimeComposition(configuration);

                return runtimeComposition.CreateExportProviderFactory();
            },
            LazyThreadSafetyMode.ExecutionAndPublication);
    }

    public static IEnumerable<CodeFixProvider> GetCodeFixProviders(string language)
    {
        var exportProvider = factory.Value.CreateExportProvider();
        var exports = exportProvider.GetExports<CodeFixProvider, LanguageMetadata>();
        return exports.Where(export => export.Metadata.Languages.Contains(language)).Select(export => export.Value);
    }

    private class LanguageMetadata
    {
        public LanguageMetadata(IDictionary<string, object> data)
        {
            if (!data.TryGetValue(nameof(ExportCodeFixProviderAttribute.Languages), out var languages))
            {
                languages = Array.Empty<string>();
            }

            Languages = ((string[])languages).ToImmutableArray();
        }

        public ImmutableArray<string> Languages { get; }
    }
}
