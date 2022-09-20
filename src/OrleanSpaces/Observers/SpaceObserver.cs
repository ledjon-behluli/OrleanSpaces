using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver
{
    /// <summary>
    /// Occurs whenever a <see cref="SpaceTuple"/> is written in the space.
    /// </summary>
    /// <param name="tuple">The written tuple.</param>
    /// <param name="cancellationToken">A token used to propagate cancellations.</param>
    Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken = default);

    /// <summary>
    /// Occurs whenever a <see cref="SpaceTuple"/> is removed from the space.
    /// </summary>
    /// <param name="template">The template that was used to remove the tuple.</param>
    ///<param name="cancellationToken">A token used to propagate cancellations.</param>
    Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken = default);

    /// <summary>
    /// Occurs whenever the space contains zero <see cref="SpaceTuple"/>'s.
    /// </summary>
    /// <param name="cancellationToken">A token used to propagate cancellations.</param>
    Task OnFlatteningAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// A base class which provides dynamic observation capabilities.
/// </summary>
public abstract class SpaceObserver : ISpaceObserver
{
    private EventType type = EventType.Nothing;

    /// <summary>
    /// Configures the derived class to listen to specific events.
    /// </summary>
    /// <param name="type">The event type interested in.</param>
    /// <remarks><i>Combinations are possible via bitwise operations on <see cref="EventType"/>.</i></remarks>
    protected void ListenTo(EventType type) => this.type = type;

    internal async ValueTask NotifyAsync(ITuple tuple, CancellationToken cancellationToken)
    {
        if (tuple is SpaceTuple spaceTuple && type.HasFlag(Expansions))
        {
            await OnExpansionAsync(spaceTuple, cancellationToken);
            return;
        }

        if (tuple is SpaceTemplate template && type.HasFlag(Contractions))
        {
            await OnContractionAsync(template, cancellationToken);
            return;
        }

        if (tuple is SpaceUnit && type.HasFlag(Flattenings))
        {
            await OnFlatteningAsync(cancellationToken);
            return;
        }
    }

    public virtual Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnFlatteningAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    [Flags]
    protected enum EventType
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