namespace OrleanSpaces.Primitives;

internal static class TypeChecker
{
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