using OrleanSpaces.Observers;

public class Completer : DynamicObserver
{
    public Completer()
    {
        Interested(In.Flattening);
    }

    public override async Task OnFlatteningAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("COMPLETER-er: Space has been flattened. Performing gracefully shutdown...");
        await Task.Delay(1000, cancellationToken);
        Console.WriteLine("COMPLETER-er: Done");
    }
}