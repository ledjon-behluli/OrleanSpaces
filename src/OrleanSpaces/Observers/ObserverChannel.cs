using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal sealed class ObserverChannel : IConsumable
{
    private readonly Channel<ISpaceTuple> channel = 
        Channel.CreateUnbounded<ISpaceTuple>(new() { SingleReader = true });

    public bool IsBeingConsumed { get; set; }

    public ChannelReader<ISpaceTuple> Reader => channel.Reader;
    public ChannelWriter<ISpaceTuple> Writer => channel.Writer;
}

internal sealed class ObserverChannel<T, TTuple, TTemplate> : IConsumable
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