using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackChannel : IConsumable
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

internal sealed class CallbackChannel<T, TTuple, TTemplate> : IConsumable
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
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