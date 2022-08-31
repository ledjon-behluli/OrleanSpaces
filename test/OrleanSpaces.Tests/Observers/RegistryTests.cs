using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests.Observers;

public class RegistryTests
{
    private readonly ObserverRegistry registry = new();
    private readonly TestObserver observer = new();
    private readonly Guid observerId;

    public RegistryTests()
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
        registry.Add(new TestObserver());
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
        Guid id = registry.Add(new TestObserver());
        Assert.NotEqual(id, observerId);
    }

    [Fact]
    public void Should_Throw_If_Null()
    {
        Assert.Throws<ArgumentNullException>(() => registry.Add(null));
        Assert.Throws<ArgumentNullException>(() => registry.Remove(null));
    }
}
