using System.Runtime.CompilerServices;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Helpers;

internal static class TemplateHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool SequentialEquals<T, TTemplate>(this TTemplate left, TTemplate right)
       where T : unmanaged
       where TTemplate : ISpaceTemplate<T>
    {
        int length = left.Length;
        if (length != right.Length)
        {
            return false;
        }

        for (int i = 0; i < length; i++)
        {
            ref readonly T? iLeft = ref left[i];
            ref readonly T? iRight = ref right[i];

            if ((iLeft is null && iRight is not null) ||
                (iLeft is not null && iRight is null) ||
                (iLeft is { } l && !l.Equals(iRight)))
            {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Matches<T, TTuple, TTemplate>(this TTemplate template, TTuple tuple)
        where T : unmanaged
        where TTemplate : ISpaceTemplate<T>
        where TTuple : ISpaceTuple<T>, IEquatable<TTuple>, ISpaceFactory<T, TTuple>
    {
        int length = template.Length;
        if (length != tuple.Length)
        {
            return false;
        }

        T[] fields = new T[length];
        for (int i = 0; i < length; i++)
        {
            fields[i] = template[i] is { } value ? value : tuple[i];
        }

        TTuple templateTuple = TTuple.Create(fields);
        bool result = templateTuple.Equals(tuple);

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToString<T>(T?[] fields) where T : unmanaged
        => fields is null ? "()" : $"({string.Join(", ", fields.Select(field => field is null ? "{NULL}" : field.ToString()))})";
}
