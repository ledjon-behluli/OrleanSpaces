using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System.Text;

public class Auditor : DynamicObserver
{
    public Auditor()
    {
        Interested(In.Expansions | In.Contractions);
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

// Example of a class that HAS to inherit from another class, at the same time being a space observer.
// This class would be able to observer the space by inheriting from the DynamicObserver. 
public class EncodedObserver : Encoder, ISpaceObserver
{
    public Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task OnFlatteningAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}