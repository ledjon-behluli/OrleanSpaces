using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Evaluations;

internal static class EvaluationChannel
{
    private readonly static Channel<Func<Task<SpaceTuple>>> channel 
        = Channel.CreateUnbounded<Func<Task<SpaceTuple>>>();

    public static ChannelReader<Func<Task<SpaceTuple>>> Reader => channel.Reader;
    public static ChannelWriter<Func<Task<SpaceTuple>>> Writer => channel.Writer;
}