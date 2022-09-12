using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Primitives;
using System.Collections.Concurrent;

[MemoryDiagnoser]
[BaselineColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class TypeCheckerBenchmarks
{
    private static readonly ConcurrentDictionary<Type, bool> cache = new();

    [Benchmark(Baseline = true)]
    public void IsSimpleType()
    {
        for (int i = 0; i < 100_000; i++)
        {
            TypeChecker.IsSimpleType(typeof(bool));
            TypeChecker.IsSimpleType(typeof(byte));
            TypeChecker.IsSimpleType(typeof(sbyte));
            TypeChecker.IsSimpleType(typeof(char));
            TypeChecker.IsSimpleType(typeof(double));
            TypeChecker.IsSimpleType(typeof(float));
            TypeChecker.IsSimpleType(typeof(int));
            TypeChecker.IsSimpleType(typeof(uint));
            TypeChecker.IsSimpleType(typeof(nint));
            TypeChecker.IsSimpleType(typeof(nuint));
            TypeChecker.IsSimpleType(typeof(long));
            TypeChecker.IsSimpleType(typeof(ulong));
            TypeChecker.IsSimpleType(typeof(short));
            TypeChecker.IsSimpleType(typeof(ushort));
            TypeChecker.IsSimpleType(typeof(string));
            TypeChecker.IsSimpleType(typeof(decimal));
            TypeChecker.IsSimpleType(typeof(DateTime));
            TypeChecker.IsSimpleType(typeof(DateTimeOffset));
            TypeChecker.IsSimpleType(typeof(TimeSpan));
            TypeChecker.IsSimpleType(typeof(Guid));
        }
    }

    [Benchmark]
    public void IsSimpleTypeCached()
    {
        for (int i = 0; i < 100_000; i++)
        {
            CheckCached(typeof(bool));
            CheckCached(typeof(byte));
            CheckCached(typeof(sbyte));
            CheckCached(typeof(char));
            CheckCached(typeof(double));
            CheckCached(typeof(float));
            CheckCached(typeof(int));
            CheckCached(typeof(uint));
            CheckCached(typeof(nint));
            CheckCached(typeof(nuint));
            CheckCached(typeof(long));
            CheckCached(typeof(ulong));
            CheckCached(typeof(short));
            CheckCached(typeof(ushort));
            CheckCached(typeof(string));
            CheckCached(typeof(decimal));
            CheckCached(typeof(DateTime));
            CheckCached(typeof(DateTimeOffset));
            CheckCached(typeof(TimeSpan));
            CheckCached(typeof(Guid));
        }

        static bool CheckCached(Type type) =>
            cache.GetOrAdd(type, TypeChecker.IsSimpleType(type));
    }
}