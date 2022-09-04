using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Continuations;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.AccessControl;

namespace OrleanSpaces.Tests.Continuations;

public class ProcessorTests : IClassFixture<ProcessorTests.Fixture>
{
    private readonly ContinuationChannel channel;
    private readonly TestRouter router;

    public ProcessorTests(Fixture fixture)
    {
        channel = fixture.Channel;
        router = fixture.Router;
    }

    //[Fact]
    //public async Task Should_Forward_Tuple_To_Router()
    //{
    //    SpaceTuple tuple = SpaceTuple.Create("continue");
    //    await channel.Writer.WriteAsync(tuple);

    //    ISpaceElement element = null;

    //    do
    //    {
    //        element = router.Elements.SingleOrDefault(x => x.Equals(tuple));
    //    }
    //    while (element == null);

    //    Assert.Equal(tuple, element);
    //}

    //[Fact]
    //public async Task Should_Forward_Template_To_Router()
    //{
    //    SpaceTemplate template = SpaceTemplate.Create("continue");
    //    await channel.Writer.WriteAsync(template);

    //    ISpaceElement element = null;

    //    do
    //    {
    //        element = router.Elements.SingleOrDefault(x => x.Equals(template));
    //    }
    //    while (element == null);

    //    Assert.Equal(template, element);
    //}

    public class Fixture : IAsyncLifetime
    {
        private readonly ContinuationProcessor processor;

        internal TestRouter Router { get; }
        internal ContinuationChannel Channel { get; }

        public Fixture()
        {
            Channel = new();
            Router = new TestRouter();

            processor = new(Channel, Router, new NullLogger<ContinuationProcessor>());
        }

        public async Task InitializeAsync() => await processor.StartAsync(default);
        public async Task DisposeAsync() => await processor.StopAsync(default);
    }

    internal class TestRouter : ISpaceElementRouter
    {
        private readonly List<ISpaceElement> elements = new();
        public IEnumerable<ISpaceElement> Elements => elements;

        public Task RouteAsync(ISpaceElement element)
        {
            elements.Add(element);
            return Task.CompletedTask;
        }
    }
}