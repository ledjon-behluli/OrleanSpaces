using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

public class Auditor : SpaceObserver
{
    public Auditor()
    {
        Observe(SpaceEvent.TupleAdded);
        Observe(SpaceEvent.TupleRemoved);
    }

    public override Task OnAddedAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        Console.WriteLine($"AUDITOR: A new tuple '{tuple}' got added.");
        return Task.CompletedTask;
    }

    public override Task OnRemovedAsync(SpaceTemplate template, CancellationToken cancellationToken)
    {
        Console.WriteLine($"AUDITOR: A tuple corresponding to template '{template}' got removed.");
        return Task.CompletedTask;
    }
}
