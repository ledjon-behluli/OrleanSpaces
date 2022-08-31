using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests.Observers;

public class RefTests
{
    [Fact]
    public void Should_Create()
    {
        Guid id = Guid.NewGuid();
        ISpaceObserver observer = new TestObserver();

        ObserverRef @ref = new(id, observer);

        Assert.Equal(id, @ref.Id);
        Assert.Equal(observer, @ref.Observer);
    }

    [Fact]
    public void Should_Throw_If_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new ObserverRef(Guid.NewGuid(), null));
    }
}
