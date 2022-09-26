using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;
using System;
using System.Text;

// Example of a class that "has" to inherit from another class, at the same time being a space observer.
// This class wouldn't be able to observe the space by inheriting from the SpaceObserver. Instead it uses
// the interface which means it has to implement all methods and will have to listen to all observation types.
public class Archiver : Encoder, ISpaceObserver
{
    public Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        Console.WriteLine($"ARCHIVER: Archived tuple '{Encode(tuple.ToString())}'");
        return Task.CompletedTask;
    }

    public Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken)
    {
        Console.WriteLine($"ARCHIVER: Archived template '{Encode(template.ToString())}'");
        return Task.CompletedTask;
    }

    public Task OnFlatteningAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("ARCHIVER: I don't care about Flattening's, yet since I inhert from 'Encoder' I need to use the interface instead.");
        return Task.CompletedTask;
    }
}

public class Encoder
{
    public string Encode(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
}
