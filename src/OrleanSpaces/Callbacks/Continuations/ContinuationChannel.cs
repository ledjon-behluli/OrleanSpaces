using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks.Continuations;

internal static class ContinuationChannel
{
    private readonly static Channel<SpaceTemplate> channel = Channel.CreateUnbounded<SpaceTemplate>();

    public static ChannelReader<SpaceTemplate> Reader => channel.Reader;
    public static ChannelWriter<SpaceTemplate> Writer => channel.Writer;
}
