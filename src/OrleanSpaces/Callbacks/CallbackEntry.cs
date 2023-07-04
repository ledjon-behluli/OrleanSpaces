using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal readonly struct CallbackEntry<T>
    where T : ISpaceTuple
{
    public readonly Func<T, Task> Callback;
    public readonly bool HasContinuation;

    public CallbackEntry(Func<T, Task> callback, bool hasContinuation)
    {
        Callback = callback;
        HasContinuation = hasContinuation;
    }
}