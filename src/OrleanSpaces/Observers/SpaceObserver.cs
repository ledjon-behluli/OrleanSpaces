using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Observers;

public abstract class SpaceObserver
{
    private readonly Dictionary<EventType, bool> choices = new()
    {
        { EventType.TupleAdded, false },
        { EventType.TupleRemoved, false },
        { EventType.SpaceEmptied, false }
    };

    protected void Observe(EventType type) => choices[type] = true;
    protected void ObserveAll()
    {
        foreach (var type in choices.Keys)
        {
            Observe(type);
        }
    }

    internal async Task Handle(ITuple tuple, CancellationToken cancellationToken)
    {
        if (tuple is SpaceTuple spaceTuple && choices[EventType.TupleAdded])
        {
            await OnAddedAsync(spaceTuple, cancellationToken);
            return;
        }

        if (tuple is SpaceTemplate template && choices[EventType.TupleRemoved])
        {
            await OnRemovedAsync(template, cancellationToken);
            return;
        }

        if (tuple is SpaceUnit && choices[EventType.SpaceEmptied])
        {
            await OnEmptyAsync(cancellationToken);
            return;
        }
    }

    public virtual Task OnAddedAsync(SpaceTuple tuple, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public virtual Task OnEmptyAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    public virtual Task OnRemovedAsync(SpaceTemplate template, CancellationToken cancellationToken)
        => Task.CompletedTask;

    protected enum EventType
    {
        TupleAdded,
        TupleRemoved,
        SpaceEmptied
    }
}