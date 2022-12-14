using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Evaluations;

internal sealed class EvaluationChannel : IConsumable
{
    private readonly Channel<Func<Task<SpaceTuple>>> channel 
        = Channel.CreateUnbounded<Func<Task<SpaceTuple>>>(new() { SingleReader = true });

    public bool IsBeingConsumed { get; set; }

    public ChannelReader<Func<Task<SpaceTuple>>> Reader => channel.Reader;
    public ChannelWriter<Func<Task<SpaceTuple>>> Writer => channel.Writer;
}