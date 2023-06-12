using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal sealed class ObserverChannel : IConsumable
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