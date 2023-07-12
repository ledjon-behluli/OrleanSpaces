using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Observers;

public class DecoratorTests
{
    private readonly MutedObserver observer = new();

    [Fact]
    public async Task Should_Forward_Expansions()
    {
        ObserverDecorator<SpaceTuple> decorator = new(observer);

        SpaceTuple tuple = new(1);
        await decorator.OnExpansionAsync(tuple, default);

        Assert.Equal(tuple, observer.LastExpansionTuple);
    }

    [Fact]
    public async Task Should_Forward_Contractions()
    {
        ObserverDecorator<SpaceTuple> decorator = new(observer);

        SpaceTuple tuple = new(2);
        await decorator.OnContractionAsync(tuple, default);

        Assert.Equal(tuple, observer.LastContractionTuple);
    }

    [Fact]
    public async Task Should_Forward_Flattenings()
    {
        ObserverDecorator<SpaceTuple> decorator = new(observer);

        await decorator.OnFlatteningAsync(default);

        Assert.True(observer.HasFlattened);
    }

    private class MutedObserver : TestSpaceObserver<SpaceTuple>
    {
        public MutedObserver() => ListenTo(Nothing);
    }
}
