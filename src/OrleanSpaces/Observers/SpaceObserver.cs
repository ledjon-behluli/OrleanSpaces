using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

/// <summary>
/// A base class which provides dynamic observation capabilities.
/// </summary>
public abstract class SpaceObserver<T> : ISpaceObserver<T>
    where T : ISpaceTuple
{
    protected internal EventType type = EventType.Nothing;

    /// <summary>
    /// Configures the derived class to listen to specific events.
    /// </summary>
    /// <param name="type">The event type interested in.</param>
    /// <remarks><i>Combinations are possible via bitwise operations on <see cref="EventType"/>.</i></remarks>
    protected void ListenTo(EventType type) => this.type = type;

    internal async ValueTask NotifyAsync(TupleAction<T> action, CancellationToken cancellationToken)
    {
        if (action.Type == TupleActionType.Added && type.HasFlag(Expansions))
        {
            await OnExpansionAsync(action.Tuple, cancellationToken);
            return;
        }

        if (action.Type == TupleActionType.Removed && type.HasFlag(Contractions))
        {
            await OnContractionAsync(action.Tuple, cancellationToken);
            return;
        }

        if (action.Type == TupleActionType.Cleaned && type.HasFlag(Contractions))
        {
            await OnFlatteningAsync(cancellationToken);
            return;
        }
    }

    public virtual Task OnExpansionAsync(T tuple, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnContractionAsync(T tuple, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnFlatteningAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    [Flags]
    protected internal enum EventType
    {
        /// <summary>
        /// Specifies that the observer is not interested in any kind of events.
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// Specifies that the observer is interested in space expansion events.
        /// </summary>
        Expansions = 1,
        /// <summary>
        /// Specifies that the observer is interested in space contraction events.
        /// </summary>
        Contractions = 2,
        /// <summary>
        /// Specifies that the observer is interested in space flattening events.
        /// </summary>
        Flattenings = 4,
        /// <summary>
        /// Specifies that the observer is interested in all kinds of events.
        /// </summary>
        Everything = Expansions | Contractions | Flattenings
    }

    /// <returns><see cref="EventType.Nothing"/></returns>
    protected static readonly EventType Nothing = EventType.Nothing;
    /// <returns><see cref="EventType.Expansions"/></returns>
    protected static readonly EventType Expansions = EventType.Expansions;
    /// <returns><see cref="EventType.Contractions"/></returns>
    protected static readonly EventType Contractions = EventType.Contractions;
    /// <returns><see cref="EventType.Flattenings"/></returns>
    protected static readonly EventType Flattenings = EventType.Flattenings;
    /// <returns><see cref="EventType.Everything"/></returns>
    protected static readonly EventType Everything = EventType.Everything;
}