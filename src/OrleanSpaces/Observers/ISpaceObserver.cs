using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

public interface ISpaceObserver<T>
    where T : ISpaceTuple
{
    /// <summary>
    /// Occurs whenever a <see cref="T"/> is written in the space.
    /// </summary>
    /// <param name="tuple">The written tuple.</param>
    /// <param name="cancellationToken">A token used to propagate cancellations.</param>
    Task OnExpansionAsync(T tuple, CancellationToken cancellationToken = default);

    /// <summary>
    /// Occurs whenever a <see cref="T"/> is removed from the space.
    /// </summary>
    /// <param name="tuple">The tuple that was removed.</param>
    /// <param name="cancellationToken">A token used to propagate cancellations.</param>
    Task OnContractionAsync(T tuple, CancellationToken cancellationToken = default);

    /// <summary>
    /// Occurs whenever the space contains zero <see cref="T"/>'s.
    /// </summary>
    /// <param name="cancellationToken">A token used to propagate cancellations.</param>
    Task OnFlatteningAsync(CancellationToken cancellationToken = default);
}