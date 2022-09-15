using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Observers;

public sealed class ObserverRef : IEquatable<ObserverRef>
{
    public Guid Id { get; }
    [NotNull] public ISpaceObserver Observer { get; }

    public ObserverRef(Guid id, ISpaceObserver observer)
    {
        Id = id;
        Observer = observer ?? throw new ArgumentNullException(nameof(observer));
    }

    public static bool operator ==(ObserverRef first, ObserverRef second)
    {
        if (first is null || second is null)
            return false;

        return first.Equals(second);
    }

    public static bool operator !=(ObserverRef first, ObserverRef second) => !(first == second);

    public override bool Equals(object obj) =>
        obj is ObserverRef overrides && Equals(overrides);

    public bool Equals(ObserverRef other)
    {
        if (Id != other.Id)
            return false;

        return ReferenceEquals(Observer, other.Observer);
    }

    public override int GetHashCode() => Id.GetHashCode();
}