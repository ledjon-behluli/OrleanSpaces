using System.Collections.Concurrent;

namespace OrleanSpaces.Primitives;

internal static class TypeChecker
{
    private static readonly ConcurrentDictionary<Type, bool> cache = new();

    public static bool IsSimpleType(Type type)
    {
        return cache.GetOrAdd(type, x =>
            x.IsPrimitive ||
            x.IsEnum ||
            x == typeof(string) ||
            x == typeof(decimal) ||
            x == typeof(DateTime) ||
            x == typeof(DateTimeOffset) ||
            x == typeof(TimeSpan) ||
            x == typeof(Guid));
    }
}