using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Observers;

public class ObserverTests 
{
    private static readonly SpaceTuple tuple = new(1);
    private static readonly SpaceTemplate template = new(1);

    private static Func<SpaceObserver, Task> Expansion =>
        async (observer) => await observer.NotifyAsync(tuple, default);

    private static Func<SpaceObserver, Task> Contraction =>
        async (observer) => await observer.NotifyAsync(template, default);

    private static Func<SpaceObserver, Task> Flattening =>
        async (observer) => await observer.NotifyAsync(new SpaceUnit(), default);

    private static Func<SpaceObserver, Task> Everything =>
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

        Assert.Equal(tuple, observer.LastTuple);
        Assert.Equal(template, observer.LastTemplate);
        Assert.True(observer.LastFlattening);
    }

    [Fact]
    public async Task Should_Invoke_OnExpansion()
    {
        ExpansionsObserver observer = new();

        await Everything(observer);

        Assert.Equal(tuple, observer.LastTuple);
        Assert.NotEqual(template, observer.LastTemplate);
        Assert.False(observer.LastFlattening);
    }

    [Fact]
    public async Task Should_Invoke_OnContraction()
    {
        ContractionsObserver observer = new();

        await Everything(observer);

        Assert.NotEqual(tuple, observer.LastTuple);
        Assert.Equal(template, observer.LastTemplate);
        Assert.False(observer.LastFlattening);
    }

    [Fact]
    public async Task Should_Invoke_OnExpansion_And_OnContraction()
    {
        CombinedObserver observer = new();

        await Everything(observer);

        Assert.Equal(tuple, observer.LastTuple);
        Assert.Equal(template, observer.LastTemplate);
        Assert.False(observer.LastFlattening);
    }

    [Fact]
    public async Task Should_Invoke_OnFlattening()
    {
        FlatteningsObserver observer = new();

        await Everything(observer);

        Assert.NotEqual(tuple, observer.LastTuple);
        Assert.NotEqual(template, observer.LastTemplate);
        Assert.True(observer.LastFlattening);
    }

    [Fact]
    public async Task Should_Invoke_Nothing()
    {
        NothingObserver observer = new();
        await Everything(observer);

        Assert.NotEqual(tuple, observer.LastTuple);
        Assert.NotEqual(template, observer.LastTemplate);
        Assert.False(observer.LastFlattening);
    }

    [Fact]
    public async Task Should_Be_Dynamic()
    {
        DynamicObserver observer = new();

        await Flattening(observer);
        Assert.False(observer.LastFlattening);

        await Contraction(observer);
        Assert.NotEqual(template, observer.LastTemplate);

        await Expansion(observer);
        Assert.Equal(tuple, observer.LastTuple);

        observer.Reset();

        await Contraction(observer);
        Assert.Equal(template, observer.LastTemplate);

        await Flattening(observer);
        Assert.True(observer.LastFlattening);

        await Expansion(observer);
        Assert.NotEqual(tuple, observer.LastTuple);
    }

    #region Observers

    private class EverythingObserver : BaseObserver
    {
        public EverythingObserver() => ListenTo(Everything);
    }

    private class ExpansionsObserver : BaseObserver
    {
        public ExpansionsObserver() => ListenTo(Expansions);
    }

    private class ContractionsObserver : BaseObserver
    {
        public ContractionsObserver() => ListenTo(Contractions);
    }

    private class CombinedObserver : BaseObserver
    {
        public CombinedObserver() => ListenTo(Expansions | Contractions);
    }

    private class FlatteningsObserver : BaseObserver
    {
        public FlatteningsObserver() => ListenTo(Flattenings);
    }

    private class NothingObserver : BaseObserver
    {
        public NothingObserver() => ListenTo(Nothing);
    }

    private class DynamicObserver : BaseObserver
    {
        public DynamicObserver() => ListenTo(Expansions);

        public void Reset()
        {
            LastTuple = new();
            LastTemplate = new();
            LastFlattening = false;
        }

        public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
        {
            base.OnExpansionAsync(tuple, cancellationToken);
            ListenTo(Contractions);

            return Task.CompletedTask;
        }

        public override Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken)
        {
            base.OnContractionAsync(template, cancellationToken);
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

    private class BaseObserver : SpaceObserver
    {
        public SpaceTuple LastTuple { get; protected set; } = new();
        public SpaceTemplate LastTemplate { get; protected set; } = new();
        public bool LastFlattening { get; protected set; }

        public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
        {
            LastTuple = tuple;
            return Task.CompletedTask;
        }

        public override Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken)
        {
            LastTemplate = template;
            return Task.CompletedTask;
        }

        public override Task OnFlatteningAsync(CancellationToken cancellationToken)
        {
            LastFlattening = true;
            return Task.CompletedTask;
        }
    }


    #endregion
}