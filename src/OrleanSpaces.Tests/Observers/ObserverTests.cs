using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Observers;

public partial class ObserverTests 
{
    private static readonly SpaceTuple tuple = new(1);
    private static readonly SpaceTemplate template = new(1);

    [Fact]
    public async Task Should_Invoke_All_For_AllObserver()
    {
        AllObserver allObserver = new();

        await Run(allObserver);

        Assert.Equal(tuple, allObserver.LastTuple);
        Assert.Equal(template, allObserver.LastTemplate);
        Assert.True(allObserver.SpaceEmptiedReceived);
    }

    [Fact]
    public async Task Should_Invoke_Only_OnAdded_For_TupleAddedObserver()
    {
        TupleAddedObserver observer = new();

        await Run(observer);

        Assert.Equal(tuple, observer.LastTuple);
        Assert.NotEqual(template, observer.LastTemplate);
        Assert.False(observer.SpaceEmptiedReceived);
    }

    [Fact]
    public async Task Should_Invoke_Only_OnRemoved_For_TupleRemovedObserver()
    {
        TupleAddedObserver observer = new();

        await Run(observer);

        Assert.NotEqual(tuple, observer.LastTuple);
        Assert.Equal(template, observer.LastTemplate);
        Assert.False(observer.SpaceEmptiedReceived);
    }

    [Fact]
    public async Task Should_Invoke_Only_OnEmpty_For_EmptySpaceObserver()
    {
        TupleAddedObserver observer = new();

        await Run(observer);

        Assert.NotEqual(tuple, observer.LastTuple);
        Assert.NotEqual(template, observer.LastTemplate);
        Assert.True(observer.SpaceEmptiedReceived);
    }

    private static async Task Run(SpaceObserver observer)
    {
        await observer.HandleAsync(tuple, default);
        await observer.HandleAsync(template, default);
        await observer.HandleAsync(SpaceUnit.Null, default);
    }

    #region Observers

    private class AllObserver : BaseObserver
    {
        public AllObserver() => ObserveAll();
    }

    private class TupleAddedObserver : BaseObserver
    {
        public TupleAddedObserver() => Observe(SpaceEvent.TupleAdded);
    }

    private class TupleRemovedObserver : BaseObserver
    {
        public TupleRemovedObserver() => Observe(SpaceEvent.TupleRemoved);
    }

    private class EmptySpaceObserver : BaseObserver
    {
        public EmptySpaceObserver()
        {
            Observe(SpaceEvent.SpaceEmptied);
        }
    }

    private class TupleAddedAndRemovedObserver : BaseObserver
    {
        public TupleAddedAndRemovedObserver()
        {
            Interested(In.TupleAdditions);
            Interested(In.TupleRemovals);
            Interested(In.SpaceEmptying);
        }
    }

    private class BaseObserver : SpaceObserver
    {
        public SpaceTuple LastTuple { get; private set; } = new();
        public SpaceTemplate LastTemplate { get; private set; } = new();
        public bool SpaceEmptiedReceived { get; private set; }

        public override Task OnAddedAsync(SpaceTuple tuple, CancellationToken cancellationToken)
        {
            LastTuple = tuple;
            return Task.CompletedTask;
        }

        public override Task OnRemovedAsync(SpaceTemplate template, CancellationToken cancellationToken)
        {
            LastTemplate = template;
            return Task.CompletedTask;
        }

        public override Task OnEmptyAsync(CancellationToken cancellationToken)
        {
            SpaceEmptiedReceived = true;
            return Task.CompletedTask;
        }
    }


    #endregion
}