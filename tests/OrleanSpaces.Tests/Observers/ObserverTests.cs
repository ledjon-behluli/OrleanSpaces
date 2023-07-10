using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Observers;

public class ObserverTests 
{
    private static readonly SpaceTuple expansionTuple = new(1);
    private static readonly SpaceTuple contractionTuple = new(2);
    private static readonly SpaceTuple flatteningTuple = new(3);

    private static Func<SpaceObserver<SpaceTuple>, Task> Expansion =>
        async (observer) => await observer.NotifyAsync(new(Guid.NewGuid(), expansionTuple, TupleActionType.Insert), default);

    private static Func<SpaceObserver<SpaceTuple>, Task> Contraction =>
        async (observer) => await observer.NotifyAsync(new(Guid.NewGuid(), contractionTuple, TupleActionType.Remove), default);

    private static Func<SpaceObserver<SpaceTuple>, Task> Flattening =>
        async (observer) => await observer.NotifyAsync(new(Guid.NewGuid(), flatteningTuple, TupleActionType.Clean), default);

    private static Func<SpaceObserver<SpaceTuple>, Task> Everything =>
        async (observer) =>
        {
            await Expansion(observer);
            await Contraction(observer);
            await Flattening(observer);
        };


    [Fact]
    public async Task Should_Invoke_Everything()
    {
        EverythingObserver observer = new();

        await Everything(observer);

        Assert.Equal(expansionTuple, observer.LastExpansionTuple);
        Assert.Equal(contractionTuple, observer.LastContractionTuple);
        Assert.True(observer.HasFlattened);
    }

    [Fact]
    public async Task Should_Invoke_OnExpansion()
    {
        ExpansionsObserver observer = new();

        await Everything(observer);

        Assert.Equal(expansionTuple, observer.LastExpansionTuple);
        Assert.NotEqual(contractionTuple, observer.LastContractionTuple);
        Assert.False(observer.HasFlattened);
    }

    [Fact]
    public async Task Should_Invoke_OnContraction()
    {
        ContractionsObserver observer = new();

        await Everything(observer);

        Assert.NotEqual(expansionTuple, observer.LastExpansionTuple);
        Assert.Equal(contractionTuple, observer.LastContractionTuple);
        Assert.False(observer.HasFlattened);
    }

    [Fact]
    public async Task Should_Invoke_OnExpansion_And_OnContraction()
    {
        CombinedObserver observer = new();

        await Everything(observer);

        Assert.Equal(expansionTuple, observer.LastExpansionTuple);
        Assert.Equal(contractionTuple, observer.LastContractionTuple);
        Assert.False(observer.HasFlattened);
    }

    [Fact]
    public async Task Should_Invoke_OnFlattening()
    {
        FlatteningsObserver observer = new();

        await Everything(observer);

        Assert.NotEqual(expansionTuple, observer.LastExpansionTuple);
        Assert.NotEqual(contractionTuple, observer.LastContractionTuple);
        Assert.True(observer.HasFlattened);
    }

    [Fact]
    public async Task Should_Invoke_Nothing()
    {
        NothingObserver observer = new();
        await Everything(observer);

        Assert.NotEqual(expansionTuple, observer.LastExpansionTuple);
        Assert.NotEqual(contractionTuple, observer.LastContractionTuple);
        Assert.False(observer.HasFlattened);
    }

    [Fact]
    public async Task Should_Be_Dynamic()
    {
        DynamicObserver observer = new();

        await Flattening(observer);
        Assert.False(observer.HasFlattened);

        await Contraction(observer);
        Assert.NotEqual(contractionTuple, observer.LastContractionTuple);

        await Expansion(observer);
        Assert.Equal(expansionTuple, observer.LastExpansionTuple);

        observer.Reset();

        await Contraction(observer);
        Assert.Equal(contractionTuple, observer.LastContractionTuple);

        await Flattening(observer);
        Assert.True(observer.HasFlattened);

        await Expansion(observer);
        Assert.NotEqual(expansionTuple, observer.LastExpansionTuple);
    }

    #region Observers

    private class EverythingObserver : TestSpaceObserver<SpaceTuple>
    {
        public EverythingObserver() => ListenTo(Everything);
    }

    private class ExpansionsObserver : TestSpaceObserver<SpaceTuple>
    {
        public ExpansionsObserver() => ListenTo(Expansions);
    }

    private class ContractionsObserver : TestSpaceObserver<SpaceTuple>
    {
        public ContractionsObserver() => ListenTo(Contractions);
    }

    private class CombinedObserver : TestSpaceObserver<SpaceTuple>
    {
        public CombinedObserver() => ListenTo(Expansions | Contractions);
    }

    private class FlatteningsObserver : TestSpaceObserver<SpaceTuple>
    {
        public FlatteningsObserver() => ListenTo(Flattenings);
    }

    private class NothingObserver : TestSpaceObserver<SpaceTuple>
    {
        public NothingObserver() => ListenTo(Nothing);
    }

    private class DynamicObserver : TestSpaceObserver<SpaceTuple>
    {
        public DynamicObserver() => ListenTo(Expansions);

        public void Reset()
        {
            LastExpansionTuple = new();
            LastContractionTuple = new();
            HasFlattened = false;
        }

        public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
        {
            base.OnExpansionAsync(tuple, cancellationToken);
            ListenTo(Contractions);

            return Task.CompletedTask;
        }

        public override Task OnContractionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
        {
            base.OnContractionAsync(tuple, cancellationToken);
            ListenTo(Flattenings);

            return Task.CompletedTask;
        }

        public override Task OnFlatteningAsync(CancellationToken cancellationToken)
        {
            base.OnFlatteningAsync(cancellationToken);
            ListenTo(Nothing);

            return Task.CompletedTask;
        }
    }

    #endregion
}