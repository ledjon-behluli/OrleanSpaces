using OrleanSpaces.Tuples;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Helpers;

internal static class ImmutableHelpers<T> where T : struct, ISpaceTuple
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(ref ImmutableArray<T> array, T tuple)
       => array = array.Add(tuple);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Remove(ref ImmutableArray<T> array, T tuple)
        => array = array.Remove(tuple);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clear(ref ImmutableArray<T> array)
        => array = ImmutableArray<T>.Empty;
}