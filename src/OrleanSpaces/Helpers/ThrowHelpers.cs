using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Helpers;

internal static class ThrowHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EmptyTuple<T>(T tuple) where T : ISpaceTuple
    {
        if (tuple.IsEmpty)
        {
            throw new ArgumentException("Empty tuple is not allowed to be writen in the tuple space");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvalidFieldType(int position) => throw new ArgumentException($"The field at position = {position} is not a valid type");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NullField(int position) => throw new ArgumentNullException($"The field at position = {position} can not be null");
}