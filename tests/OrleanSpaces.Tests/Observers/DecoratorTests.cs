using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Observers;

public class DecoratorTests
{
    private readonly MutedObserver observer = new();

    [Fact]
    public async Task Should_Forward_Expansions()
    {
        ObserverDecorator decorator = new(observer);

        SpaceTuple tuple = new(1);
        await decorator.OnExpansionAsync(tuple, default);

        Assert.Equal(tuple, observer.LastTuple);
    }

    [Fact]
    public async Task Should_Forward_Contractions()
    {
        ObserverDecorator decorator = new(observer);

        SpaceTemplate template = new(1);
        await decorator.OnContractionAsync(template, default);

        Assert.Equal(template, observer.LastTemplate);
    }

    [Fact]
    public async Task Should_Forward_Flattenings()
    {
        ObserverDecorator decorator = new(observer);

        await decorator.OnFlatteningAsync(default);

        Assert.True(observer.LastFlattening);
    }

    private class MutedObserver : TestObserver
    {
        public MutedObserver() => ListenTo(Nothing);
    }
}
