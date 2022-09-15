using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System.Text;

// Example of a class that "has" to inherit from another class, at the same time being a space observer.
// This class wouldn't be able to observe the space by inheriting from the SpaceObserver. Instead it uses
// the interface which means it has to implement all methods and will have to listen to all observation types.
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

public class Encoder
{
    public byte[] Encode(string value) => Encoding.UTF8.GetBytes(value);
}
