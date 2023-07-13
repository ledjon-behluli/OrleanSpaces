﻿using OrleanSpaces.Channels;
using OrleanSpaces.Processors;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests.Processors;

public class EvaluationSpaceProcessorTests : IClassFixture<EvaluationSpaceProcessorTests.Fixture>
{
    private readonly SpaceOptions options;
    private readonly EvaluationChannel<SpaceTuple> evaluationChannel;
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel;

    public EvaluationSpaceProcessorTests(Fixture fixture)
    {
        options = fixture.Options;
        evaluationChannel = fixture.EvaluationChannel;
        continuationChannel = fixture.ContinuationChannel;
    }

    [Fact]
    public async Task Should_Forward_If_Evaluation_Results_In_Tuple()
    {
        SpaceTuple tuple = new("eval");
        await evaluationChannel.Writer.WriteAsync(() => Task.FromResult(tuple));

        SpaceTuple result = default;
        while (result.Length == 0)
        {
            result = await continuationChannel.TupleReader.ReadAsync(default);
        }

        Assert.Equal(tuple, result);
    }

    [Fact]
    public async Task Should_Not_Forward_If_Evaluation_Throws()
    {
        options.HandleEvaluationExceptions = true;

        await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        continuationChannel.TupleReader.TryRead(out SpaceTuple result);

        result.AssertEmpty();

        options.HandleEvaluationExceptions = false;
    }

    [Fact]
    public void Should_Throw_If_Evaluation_Throws()
    {
        options.HandleEvaluationExceptions = false;

        _ = Assert.ThrowsAsync<Exception>(async () => await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test")));

        options.HandleEvaluationExceptions = true;
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly EvaluationProcessor<SpaceTuple, SpaceTemplate> processor;

        internal SpaceOptions Options { get; }
        internal EvaluationChannel<SpaceTuple> EvaluationChannel { get; }
        internal ContinuationChannel<SpaceTuple, SpaceTemplate> ContinuationChannel { get; }

        public Fixture()
        {
            Options = new();
            EvaluationChannel = new();
            ContinuationChannel = new();
            processor = new(Options, EvaluationChannel, ContinuationChannel);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}

public class EvaluationIntProcessorTests : IClassFixture<EvaluationIntProcessorTests.Fixture>
{
    private readonly SpaceOptions options;
    private readonly EvaluationChannel<IntTuple> evaluationChannel;
    private readonly ContinuationChannel<IntTuple, IntTemplate> continuationChannel;

    public EvaluationIntProcessorTests(Fixture fixture)
    {
        options = fixture.Options;
        evaluationChannel = fixture.EvaluationChannel;
        continuationChannel = fixture.ContinuationChannel;
    }

    [Fact]
    public async Task Should_Forward_If_Evaluation_Results_In_Tuple()
    {
        IntTuple tuple = new(1);
        await evaluationChannel.Writer.WriteAsync(() => Task.FromResult(tuple));

        IntTuple result = default;
        while (result.Length == 0)
        {
            result = await continuationChannel.TupleReader.ReadAsync(default);
        }

        Assert.Equal(tuple, result);
    }

    [Fact]
    public async Task Should_Not_Forward_If_Evaluation_Throws()
    {
        options.HandleEvaluationExceptions = true;

        await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        continuationChannel.TupleReader.TryRead(out IntTuple result);

        result.AssertEmpty();

        options.HandleEvaluationExceptions = false;
    }

    [Fact]
    public void Should_Throw_If_Evaluation_Throws()
    {
        options.HandleEvaluationExceptions = false;

        _ = Assert.ThrowsAsync<Exception>(async () => await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test")));

        options.HandleEvaluationExceptions = true;
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly EvaluationProcessor<IntTuple, IntTemplate> processor;

        internal SpaceOptions Options { get; }
        internal EvaluationChannel<IntTuple> EvaluationChannel { get; }
        internal ContinuationChannel<IntTuple, IntTemplate> ContinuationChannel { get; }

        public Fixture()
        {
            Options = new();
            EvaluationChannel = new();
            ContinuationChannel = new();
            processor = new(Options, EvaluationChannel, ContinuationChannel);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}