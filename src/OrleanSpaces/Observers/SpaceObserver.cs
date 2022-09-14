using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Observers;

public abstract class SpaceObserver
{
    private readonly Dictionary<SpaceEvent, bool> events = new()
    {
        { SpaceEvent.TupleAdded, false },
        { SpaceEvent.TupleRemoved, false },
        { SpaceEvent.SpaceEmptied, false }
    };

    protected void Observe(SpaceEvent @event) => events[@event] = true;
    protected void ObserveAll()
    {
        foreach (var type in events.Keys)
        {
            Observe(type);
        }
    }

    protected void Interested(In @in) { }

    internal async ValueTask HandleAsync(ITuple tuple, CancellationToken cancellationToken)
    {
        if (tuple is SpaceTuple spaceTuple && events[SpaceEvent.TupleAdded])
        {
            await OnAddedAsync(spaceTuple, cancellationToken);
            return;
        }

        if (tuple is SpaceTemplate template && events[SpaceEvent.TupleRemoved])
        {
            await OnRemovedAsync(template, cancellationToken);
            return;
        }

        if (tuple is SpaceUnit && events[SpaceEvent.SpaceEmptied])
        {
            await OnEmptyAsync(cancellationToken);
            return;
        }
    }

    public virtual Task OnAddedAsync(SpaceTuple tuple, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnRemovedAsync(SpaceTemplate template, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnEmptyAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    protected enum SpaceEvent
    {
        TupleAdded = 0,
        TupleRemoved = 1,
        SpaceEmptied = 2
    }

    [Flags]
    protected enum In
    {
        TupleAdditions = 0,
        TupleRemovals = 1,
        SpaceEmptying = 2
    }
}