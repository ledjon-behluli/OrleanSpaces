using OrleanSpaces.Observers;

public class Completer : SpaceObserver
{
    public Completer()
    {
        ListenTo(Flattenings);
    }

    public override async Task OnFlatteningAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("COMPLETER-er: Space has been flattened. Performing gracefully shutdown...");
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine("COMPLETER-er: Done");
    }
}