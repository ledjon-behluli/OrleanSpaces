using OrleanSpaces.Observers;

public class Completer : SpaceObserver
{
    public Completer()
    {
        Observe(SpaceEvent.SpaceEmptied);
    }

    public override async Task OnEmptyAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("COMPLETER-er: Got info that space is empty. Performing some gracefully shutdown ops...");
        await Task.Delay(1000);
    }
}