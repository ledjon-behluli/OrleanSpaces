using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackEntry<T>
    where T : ISpaceTuple
{
    public Func<T, Task> Callback { get; }
    public bool IsContinuable { get; }

    public CallbackEntry(Func<T, Task> callback, bool isContinuable)
    {
        Callback = callback;
        IsContinuable = isContinuable;
    }
}