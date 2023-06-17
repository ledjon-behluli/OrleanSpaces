using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal sealed class ContinuationChannel<TTuple, TTemplate> : IConsumable
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<TTuple> tupleChannel =
        Channel.CreateUnbounded<TTuple>(new() { SingleReader = true });

    private readonly Channel<TTemplate> templateChannel =
        Channel.CreateUnbounded<TTemplate>(new() { SingleReader = true });

    public ChannelReader<TTuple> TupleReader => tupleChannel.Reader;
    public ChannelWriter<TTuple> TupleWriter => tupleChannel.Writer;

    public ChannelReader<TTemplate> TemplateReader => templateChannel.Reader;
    public ChannelWriter<TTemplate> TemplateWriter => templateChannel.Writer;
}