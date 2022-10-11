using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.Tests;

internal static class AnalyzerHelper
{
    internal static ReferenceAssemblies Assemblies { get; }

    static AnalyzerHelper()
    {
        Assemblies = ReferenceAssemblies.Net.Net60.AddAssemblies(
            ImmutableArray.Create(typeof(Constants).Assembly.Location.Replace(".dll", string.Empty)));
    }
}
