using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal sealed class ObserverChannel<T>
    where T : ISpaceTuple
{
    private readonly Channel<TupleAction<T>> tupleChannel =
        Channel.CreateUnbounded<TupleAction<T>>(new() { SingleReader = true });

    public ChannelReader<TupleAction<T>> Reader => tupleChannel.Reader;
    public ChannelWriter<TupleAction<T>> Writer => tupleChannel.Writer;
}