using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Evaluations;

internal sealed class EvaluationChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<Func<Task<ISpaceElement>>> channel
        = Channel.CreateUnbounded<Func<Task<ISpaceElement>>>(new() { SingleReader = true });

    public ChannelReader<Func<Task<ISpaceElement>>> Reader => channel.Reader;
    public ChannelWriter<Func<Task<ISpaceElement>>> Writer => channel.Writer;
}