using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal static class CallbackChannel
{
    private readonly static Channel<CallbackBag> channel = Channel.CreateUnbounded<CallbackBag>();

    public static ChannelReader<CallbackBag> Reader => channel.Reader;
    public static ChannelWriter<CallbackBag> Writer => channel.Writer;
}