using OrleanSpaces.Primitives;
using System.Threading.Channels;

namespace OrleanSpaces.Evaluations;

internal class EvaluationChannel
{
    private readonly Channel<Func<Task<SpaceTuple>>> channel 
        = Channel.CreateUnbounded<Func<Task<SpaceTuple>>>();

    public ChannelReader<Func<Task<SpaceTuple>>> Reader => channel.Reader;
    public ChannelWriter<Func<Task<SpaceTuple>>> Writer => channel.Writer;
}