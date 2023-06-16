using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Evaluations;

internal sealed class EvaluationChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<Func<Task<ISpaceTuple>>> channel
        = Channel.CreateUnbounded<Func<Task<ISpaceTuple>>>(new() { SingleReader = true });

    public ChannelReader<Func<Task<ISpaceTuple>>> Reader => channel.Reader;
    public ChannelWriter<Func<Task<ISpaceTuple>>> Writer => channel.Writer;
}