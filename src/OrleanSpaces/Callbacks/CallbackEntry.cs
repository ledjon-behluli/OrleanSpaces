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

internal sealed class CallbackEntry<T, TTuple>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
{
    public Func<TTuple, Task> Callback { get; }
    public bool IsContinuable { get; }

    public CallbackEntry(Func<TTuple, Task> callback, bool isContinuable)
    {
        Callback = callback;
        IsContinuable = isContinuable;
    }
}