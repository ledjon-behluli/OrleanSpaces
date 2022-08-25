using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Observers;

internal static class ObserverChannel
{
    private readonly static Channel<SpaceTuple> channel = Channel.CreateUnbounded<SpaceTuple>();

    public static ChannelReader<SpaceTuple> Reader => channel.Reader;
    public static ChannelWriter<SpaceTuple> Writer => channel.Writer;
}
