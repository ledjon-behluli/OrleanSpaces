using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Observers;

public abstract class DynamicObserver : ISpaceObserver
{
    private In interest = In.Nothing;

    protected void Interested(In @in) => interest = @in;

    internal async ValueTask HandleAsync(ITuple tuple, CancellationToken cancellationToken = default)
    {
        if (tuple is SpaceTuple spaceTuple && interest.HasFlag(In.Expansions))
        {
            await OnExpansionAsync(spaceTuple, cancellationToken);
            return;
        }

        if (tuple is SpaceTemplate template && interest.HasFlag(In.Contractions))
        {
            await OnContractionAsync(template, cancellationToken);
            return;
        }

        if (tuple is SpaceUnit && interest.HasFlag(In.Flattening))
        {
            await OnFlatteningAsync(cancellationToken);
            return;
        }
    }

    public virtual Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task OnFlatteningAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    [Flags]
    protected enum In
    {
        Nothing = 1 << 0,
        Expansions = 1 << 1,
        Contractions = 1 << 2,
        Flattening = 1 << 3,
        Everything = Expansions | Contractions | Flattening
    }
}