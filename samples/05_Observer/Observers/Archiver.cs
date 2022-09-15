using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

// Example of a class that HAS to inherit from another class, at the same time being a space observer.
// This class wouldn't be able to observe the space by inheriting from the SpaceObserver. 
public class Archiver : Encoder, ISpaceObserver
{
    public Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        var data = Encode(tuple.ToString());
        Console.WriteLine($"ARCHIVER: Archived tuple '{data}'");

        return Task.CompletedTask;
    }

    public Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken)
    {
        var data = Encode(template.ToString());
        Console.WriteLine($"ARCHIVER: Archived template '{data}'");

        return Task.CompletedTask;
    }

    public Task OnFlatteningAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}