using System.Threading.Channels;

namespace OrleanSpaces.Clients.Callbacks;

internal class CallbackChannel
{
    private readonly Channel<CallbackBag> channel = Channel.CreateUnbounded<CallbackBag>();

    public ChannelReader<CallbackBag> Reader => channel.Reader;
    public ChannelWriter<CallbackBag> Writer => channel.Writer;
}