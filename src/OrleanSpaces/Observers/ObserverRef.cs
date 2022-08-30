using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Observers;

public sealed class ObserverRef
{
    public Guid Id { get; }
    [NotNull] public ISpaceObserver Observer { get; }

    public ObserverRef(Guid id, ISpaceObserver observer)
    {
        Id = id;
        Observer = observer ?? throw new ArgumentNullException(nameof(observer));
    }
}