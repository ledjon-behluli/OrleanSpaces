using OrleanSpaces.Core;
using System.Threading.Channels;

namespace OrleanSpaces.Clients;

internal class PromisesChannel
{
    private readonly Channel<TuplePromise> channel = Channel.CreateUnbounded<TuplePromise>();

    public ChannelReader<TuplePromise> Reader => channel.Reader;
    public ChannelWriter<TuplePromise> Writer => channel.Writer;
}

internal class Promises