using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System.Text;

public class Auditor : SpaceObserver
{
    public Auditor()
    {
        Show(Interest.InExpansions | Interest.InContractions);
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

public class Encoder
{
    public byte[] Encode(string value) => Encoding.UTF8.GetBytes(value);
}
