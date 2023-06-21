using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

public class Auditor : SpaceObserver<SpaceTuple>
{
    public Auditor()
    {
        ListenTo(Expansions | Contractions);
    }

    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        Console.WriteLine($"AUDITOR: Space expanded via tuple '{tuple}'.");
        return Task.CompletedTask;
    }

    public override Task OnContractionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        Console.WriteLine($"AUDITOR: Space contracted via tuple '{tuple}'.");
        return Task.CompletedTask;
    }
}