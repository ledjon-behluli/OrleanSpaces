using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;
using System;

namespace OrleanSpaces.Tests.Continuations;

[Collection("continuations")]
public class TupleRouting_ProcessorTests : IClassFixture<Fixture>
{
    private readonly ContinuationChannel channel;
    private readonly TestRouter router;

    public TupleRouting_ProcessorTests(Fixture fixture)
    {
        channel = fixture.Channel;
        router = fixture.Router;
    }

    [Fact]
    public async Task Should_Forward_Tuple_To_Router()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await channel.Writer.WriteAsync(tuple);

        while(router.Element == null)
        {

        }

        Assert.Equal(tuple, router.Element);
    }
}

[Collection("continuations")]
public class TemplateRouting_ProcessorTests : IClassFixture<Fixture>
{
    private readonly ContinuationChannel channel;
    private readonly TestRouter router;

    public TemplateRouting_ProcessorTests(Fixture fixture)
    {
        channel = fixture.Channel;
        router = fixture.Router;
    }

    [Fact]
    public async Task Should_Forward_Template_To_Router()
    {
        SpaceTemplate template = SpaceTemplate.Create(1);
        await channel.Writer.WriteAsync(template);

        while (router.Element == null)
        {

        }

        Assert.Equal(template, router.Element);
    }
}