using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal sealed class ContinuationChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<ISpaceTuple> tupleChannel = 
        Channel.CreateUnbounded<ISpaceTuple>(new() { SingleReader = true });

    private readonly Channel<ISpaceTemplate> templateChannel =
        Channel.CreateUnbounded<ISpaceTemplate>(new() { SingleReader = true });

    public ChannelReader<ISpaceTuple> TupleReader => tupleChannel.Reader;
    public ChannelWriter<ISpaceTuple> TupleWriter => tupleChannel.Writer;

    public ChannelReader<ISpaceTemplate> TemplateReader => templateChannel.Reader;
    public ChannelWriter<ISpaceTemplate> TemplateWriter => templateChannel.Writer;
}