using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal static class CallbackChannel
{
    private readonly static Channel<SpaceTuple> channel = Channel.CreateUnbounded<SpaceTuple>();

    public static ChannelReader<SpaceTuple> Reader => channel.Reader;
    public static ChannelWriter<SpaceTuple> Writer => channel.Writer;
}