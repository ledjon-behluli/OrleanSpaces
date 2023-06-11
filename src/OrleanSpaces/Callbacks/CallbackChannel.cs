using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackChannel : IConsumable
{
    private readonly Channel<SpaceTuple> channel = 
        Channel.CreateUnbounded<SpaceTuple>(new() { SingleReader = true });

    public bool IsBeingConsumed { get; set; }

    public ChannelReader<SpaceTuple> Reader => channel.Reader;
    public ChannelWriter<SpaceTuple> Writer => channel.Writer;
}

internal sealed class CallbackChannel<T, TTuple, TTemplate> : IConsumable
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<TTuple> tupleChannel =
        Channel.CreateUnbounded<TTuple>(new() { SingleReader = true });

    public ChannelReader<TTuple> TupleReader => tupleChannel.Reader;
    public ChannelWriter<TTuple> TupleWriter => tupleChannel.Writer;


    private readonly Channel<TTemplate> templateChannel =
      Channel.CreateUnbounded<TTemplate>(new() { SingleReader = true });

    public ChannelReader<TTemplate> TemplateReader => templateChannel.Reader;
    public ChannelWriter<TTemplate> TemplateWriter => templateChannel.Writer;
}