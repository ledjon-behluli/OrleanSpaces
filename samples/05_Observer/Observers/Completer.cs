using OrleanSpaces;
using OrleanSpaces.Tuples;

public class Completer : SpaceObserver<SpaceTuple>
{
    public Completer()
    {
        ListenTo(Flattenings);
    }

    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task OnContractionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
   
    public override async Task OnFlatteningAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("COMPLETER-er: Space has been flattened. Performing gracefully shutdown...");
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine("COMPLETER-er: Done");
    }
}