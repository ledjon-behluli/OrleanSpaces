using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

public class Auditor : SpaceObserver
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

    public override Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken)
    {
        Console.WriteLine($"AUDITOR: Space contracted via template '{template}'.");
        return Task.CompletedTask;
    }
}