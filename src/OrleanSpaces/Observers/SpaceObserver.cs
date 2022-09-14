using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver
{
    Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken = default);
    Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken = default);
    Task OnFlatteningAsync(CancellationToken cancellationToken = default);
}

public abstract class SpaceObserver : ISpaceObserver
{
    private Interest interest = Interest.InNothing;

    protected void Show(Interest interest) => this.interest = interest;

    internal async ValueTask HandleAsync(ITuple tuple, CancellationToken cancellationToken)
    {
        if (tuple is SpaceTuple spaceTuple && interest.HasFlag(Interest.InExpansions))
        {
            await OnExpansionAsync(spaceTuple, cancellationToken);
            return;
        }

        if (tuple is SpaceTemplate template && interest.HasFlag(Interest.InContractions))
        {
            await OnContractionAsync(template, cancellationToken);
            return;
        }

        if (tuple is SpaceUnit && interest.HasFlag(Interest.InFlattening))
        {
            await OnFlatteningAsync(cancellationToken);
            return;
        }
    }

    public virtual Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnFlatteningAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    [Flags]
    protected enum Interest
    {
        InNothing = 1 << 0,
        InExpansions = 1 << 1,
        InContractions = 1 << 2,
        InFlattening = 1 << 3,
        InEverything = InExpansions | InContractions | InFlattening
    }
}