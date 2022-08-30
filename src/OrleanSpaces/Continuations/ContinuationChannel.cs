using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Continuations;

internal static class ContinuationChannel
{
    private readonly static Channel<ISpaceElement> channel = Channel.CreateUnbounded<ISpaceElement>();

    public static ChannelReader<ISpaceElement> Reader => channel.Reader;
    public static ChannelWriter<ISpaceElement> Writer => channel.Writer;
}
