using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackEntry
{
    public Func<SpaceTuple, Task> Callback { get; }
    public bool IsContinuable { get; }

    public CallbackEntry(Func<SpaceTuple, Task> callback, bool isContinuable)
    {
        Callback = callback;
        IsContinuable = isContinuable;
    }
}