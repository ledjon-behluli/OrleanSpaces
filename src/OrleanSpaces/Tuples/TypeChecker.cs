using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples;

internal static class TypeChecker
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSimpleType(Type type)
    {
        return
            type.IsPrimitive ||
            type.IsEnum ||
            type == typeof(string) ||
            type == typeof(decimal) ||
            type == typeof(DateTime) ||
            type == typeof(DateTimeOffset) ||
            type == typeof(TimeSpan) ||
            type == typeof(Guid);
    }
}