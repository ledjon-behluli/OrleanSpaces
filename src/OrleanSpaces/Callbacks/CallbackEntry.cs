using OrleanSpaces.Primitives;

namespace OrleanSpaces.Callbacks;

internal class CallbackEntry
{
    public Func<SpaceTuple, Task> Callback { get; }
    public bool Destructive { get; }

    public CallbackEntry(Func<SpaceTuple, Task> callback, bool destructive)
    {
        Callback = callback;
        Destructive = destructive;
    }
}
