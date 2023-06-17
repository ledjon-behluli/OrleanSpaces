using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

/// <summary>
/// A base class which provides dynamic observation capabilities.
/// </summary>
public abstract class SpaceObserver<TTuple, TTemplate> : ISpaceObserver<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    protected internal EventType type = EventType.Nothing;

    /// <summary>
    /// Configures the derived class to listen to specific events.
    /// </summary>
    /// <param name="type">The event type interested in.</param>
    /// <remarks><i>Combinations are possible via bitwise operations on <see cref="EventType"/>.</i></remarks>
    protected void ListenTo(EventType type) => this.type = type;

    internal async ValueTask NotifyAsync<T>(T item, CancellationToken cancellationToken) where T : struct
    {
        if (item is TTuple tuple && type.HasFlag(Expansions))
        {
            await OnExpansionAsync(tuple, cancellationToken);
            return;
        }

        if (item is TTemplate template && type.HasFlag(Contractions))
        {
            await OnContractionAsync(template, cancellationToken);
            return;
        }

        await OnFlatteningAsync(cancellationToken);
    }

    public virtual Task OnExpansionAsync(TTuple tuple, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnContractionAsync(TTemplate template, CancellationToken cancellationToken) => Task.CompletedTask;
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