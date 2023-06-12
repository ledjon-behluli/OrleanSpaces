using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackEntry
{
    public Func<ISpaceElement, Task> Callback { get; }
    public bool IsContinuable { get; }

    public CallbackEntry(Func<ISpaceElement, Task> callback, bool isContinuable)
    {
        Callback = callback;
        IsContinuable = isContinuable;
    }
}