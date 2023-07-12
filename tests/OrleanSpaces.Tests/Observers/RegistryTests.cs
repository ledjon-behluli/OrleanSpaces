using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Observers;

public class SpaceRegistryTests
{
    private readonly ObserverRegistry<SpaceTuple> registry = new();
    private readonly TestSpaceObserver<SpaceTuple> observer = new();
    private readonly Guid observerId;

    public SpaceRegistryTests()
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
        registry.Add(new TestSpaceObserver<SpaceTuple>());
        Assert.Equal(2, registry.Observers.Count());
    }

    [Fact]
    public void Should_Remove_Observer()
    {
        registry.Remove(observerId);
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
        Guid id = registry.Add(new TestSpaceObserver<SpaceTuple>());
        Assert.NotEqual(id, observerId);
    }

    [Fact]
    public void Should_Throw_If_Null()
    {
        Assert.Throws<ArgumentNullException>(() => registry.Add(null!));
    }
}
