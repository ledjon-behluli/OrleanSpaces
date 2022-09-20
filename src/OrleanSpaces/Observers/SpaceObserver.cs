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
    private ObservableType type = ObservableType.Nothing;

    /// <summary>
    /// Configures the derived class to listen to specific observable.
    /// </summary>
    /// <param name="type">The observable type interested in.</param>
    /// <remarks><i>Combinations are possible via bit-wise operations on <see cref="ObservableType"/>.</i></remarks>
    protected void ListenTo(ObservableType type) => this.type = type;

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
    protected enum ObservableType
    {
        /// <summary>
        /// Specifies that the observer is not interested on any kind of observables.
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// Specifies that the observer is interested on space expansions.
        /// </summary>
        Expansions = 1,
        /// <summary>
        /// Specifies that the observer is interested on space contractions.
        /// </summary>
        Contractions = 2,
        /// <summary>
        /// Specifies that the observer is interested on space flattenings.
        /// </summary>
        Flattenings = 4,
        /// <summary>
        /// Specifies that the observer is interested in all kinds of observables.
        /// </summary>
        Everything = Expansions | Contractions | Flattenings
    }

    /// <summary>
    /// Specifies that the observer is not interested on any kind of observables.
    /// </summary>
    protected static readonly ObservableType Nothing = ObservableType.Nothing;   
    /// <summary>
    /// Specifies that the observer is interested on space expansions.
    /// </summary>
    protected static readonly ObservableType Expansions = ObservableType.Expansions;
    /// <summary>
    /// Specifies that the observer is interested on space contractions.
    /// </summary>
    protected static readonly ObservableType Contractions = ObservableType.Contractions;
    /// <summary>
    /// Specifies that the observer is interested on space flattenings.
    /// </summary>
    protected static readonly ObservableType Flattenings = ObservableType.Flattenings;
    /// <summary>
    /// Specifies that the observer is interested in all kinds of observables.
    /// </summary>
    protected static readonly ObservableType Everything = ObservableType.Everything;
}