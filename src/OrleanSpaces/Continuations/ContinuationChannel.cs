using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal sealed class ContinuationChannel<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly Channel<TTuple> tupleChannel =
        Channel.CreateUnbounded<TTuple>(new()
        {
            SingleReader = true,
            SingleWriter = true
        });

    private readonly Channel<TTemplate> templateChannel =
        Channel.CreateUnbounded<TTemplate>(new()
        {
            SingleReader = true,
            SingleWriter = true
        });

    public ChannelReader<TTuple> TupleReader => tupleChannel.Reader;
    public ChannelWriter<TTuple> TupleWriter => tupleChannel.Writer;

    public ChannelReader<TTemplate> TemplateReader => templateChannel.Reader;
    public ChannelWriter<TTemplate> TemplateWriter => templateChannel.Writer;
}