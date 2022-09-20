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
    private ObservationType type = ObservationType.Nothing;

    /// <summary>
    /// Configures the derived class to listen to specific observations.
    /// </summary>
    /// <param name="type">The observation type interested in.</param>
    /// <remarks><i>Combinations are possible via bit-wise operations on <see cref="ObservationType"/>.</i></remarks>
    protected void ListenTo(ObservationType type) => this.type = type;

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

    /// <inheritdoc />
    public virtual Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) => Task.CompletedTask;
    
    /// <inheritdoc />
    public virtual Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc />
    public virtual Task OnFlatteningAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    [Flags]
    protected enum ObservationType
    {
        Nothing = 0,
        Expansions = 1,
        Contractions = 2,
        Flattenings = 4,
        Everything = Expansions | Contractions | Flattenings
    }

    protected static readonly ObservationType Nothing = ObservationType.Nothing;
    protected static readonly ObservationType Expansions = ObservationType.Expansions;
    protected static readonly ObservationType Contractions = ObservationType.Contractions;
    protected static readonly ObservationType Flattenings = ObservationType.Flattenings;
    protected static readonly ObservationType Everything = ObservationType.Everything;
}