using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal sealed class ContinuationChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<SpaceTuple> tupleChannel = 
        Channel.CreateUnbounded<SpaceTuple>(new() { SingleReader = true });

    private readonly Channel<SpaceTemplate> templateChannel =
        Channel.CreateUnbounded<SpaceTemplate>(new() { SingleReader = true });

    public ChannelReader<SpaceTuple> TupleReader => tupleChannel.Reader;
    public ChannelWriter<SpaceTuple> TupleWriter => tupleChannel.Writer;

    public ChannelReader<SpaceTemplate> TemplateReader => templateChannel.Reader;
    public ChannelWriter<SpaceTemplate> TemplateWriter => templateChannel.Writer;
}