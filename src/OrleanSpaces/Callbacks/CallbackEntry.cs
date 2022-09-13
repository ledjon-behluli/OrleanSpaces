using OrleanSpaces.Primitives;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackEntry
{
    public Func<SpaceTuple, Task> Callback { get; }
    public bool IsDestructive { get; }

    public CallbackEntry(Func<SpaceTuple, Task> callback, bool isDestructive)
    {
        Callback = callback;
        IsDestructive = isDestructive;
    }
}