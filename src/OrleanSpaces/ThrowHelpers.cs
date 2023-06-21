using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;

namespace OrleanSpaces;

internal static class ThrowHelpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ChannelNotBeingConsumed<T>(T consumable, [CallerMemberName] string? methodName = null)
        where T : IConsumable
    {
        if (!consumable.IsBeingConsumed)
        {
            throw new InvalidOperationException(
                $"The method '{methodName}' is not available due to '{consumable.GetType().Name}' not having an active consumer. " +
                "This due to the client application not having started the generic host.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EmptyTuple<T>(T tuple) where T : ISpaceTuple
    {
        if (tuple.Length == 0)
        {
            throw new ArgumentException("Empty tuple is not allowed to be writen in the tuple space.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvalidFieldType(int position) => throw new ArgumentException($"The field at position = {position} is not a valid type.");

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void NullField(int position) => throw new ArgumentException($"The field at position = {position} can not be null.");
}