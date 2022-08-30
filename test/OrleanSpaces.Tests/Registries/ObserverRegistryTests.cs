using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Registries;

public class ObserverRegistryTests
{
    private readonly ObserverRegistry registry = new();
    private readonly Observer observer = new();
    private readonly Guid observerId;

    public ObserverRegistryTests()
    {
        observerId = registry.Add(observer);
    }

    [Fact]
    public void Should_Contain_Observer_Count()
    {
        Assert.Single(registry.Observers);
    }

    [Fact]
    public void Should_Add_Observer()
    {
        registry.Add(new Observer());
        Assert.Equal(2, registry.Observers.Count());
    }

    [Fact]
    public void Should_Remove_Observer()
    {
        registry.Remove(observer);
        Assert.Empty(registry.Observers);
    }

    [Fact]
    public void Should_Return_Existing_Id()
    {
        Guid id = registry.Add(observer);
        Assert.Equal(id, observerId);
    }

    [Fact]
    public void Should_Return_New_Id()
    {
        Guid id = registry.Add(new Observer());
        Assert.NotEqual(id, observerId);
    }

    private class Observer : ISpaceObserver
    {
        public Task OnTupleAsync(SpaceTuple tuple) => Task.CompletedTask;
    }
}
