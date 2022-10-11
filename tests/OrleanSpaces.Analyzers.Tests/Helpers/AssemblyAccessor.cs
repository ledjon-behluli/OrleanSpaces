using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.Tests;

public static class AssemblyAccessor
{
    public static ReferenceAssemblies Instace { get; }

    static AssemblyAccessor()
    {
        Instace = ReferenceAssemblies.Net.Net60.AddAssemblies(
            ImmutableArray.Create(typeof(Constants).Assembly.Location.Replace(".dll", string.Empty)));
    }
}