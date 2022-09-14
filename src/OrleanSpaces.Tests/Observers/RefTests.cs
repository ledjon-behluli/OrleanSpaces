using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests.Observers;

public class RefTests
{
    [Fact]
    public void Should_Create()
    {
        Guid id = Guid.NewGuid();
        TestObserver observer = new();

        ObserverRef @ref = new(id, observer);

        Assert.Equal(id, @ref.Id);
        Assert.Equal(observer, @ref.Observer);
    }

    [Fact]
    public void Should_Equal()
    {
        Guid id = Guid.NewGuid();
        TestObserver observer = new();

        ObserverRef ref1 = new(id, observer);
        ObserverRef ref2 = new(id, observer);

        Assert.Equal(ref1, ref2);
        Assert.True(ref1.Equals(ref2));
        Assert.True(ref1.Equals((object)ref2));
        Assert.True(ref1 == ref2);
        Assert.False(ref1 != ref2);
    }

    [Fact]
    public void Should_Not_Equal_If_Ref_Is_Null()
    {
        ObserverRef @ref = new(Guid.NewGuid(), new TestObserver());

        Assert.False(@ref == null);
        Assert.True(@ref != null);
    }

    [Fact]
    public void Should_Not_Equal_If_Ids_Dont_Match()
    {
        TestObserver observer = new();

        ObserverRef ref1 = new(Guid.NewGuid(), observer);
        ObserverRef ref2 = new(Guid.NewGuid(), observer);

        Assert.NotEqual(ref1, ref2);
        Assert.False(ref1 == ref2);
        Assert.True(ref1 != ref2);
    }

    [Fact]
    public void Should_Not_Equal_If_Observers_Dont_Match()
    {
        Guid id = Guid.NewGuid();

        ObserverRef ref1 = new(id, new TestObserver());
        ObserverRef ref2 = new(id, new TestObserver());

        Assert.NotEqual(ref1, ref2);
        Assert.False(ref1 == ref2);
        Assert.True(ref1 != ref2);
    }

    [Fact]
    public void Should_Throw_If_Observer_Is_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new ObserverRef(Guid.NewGuid(), null));
    }
}
