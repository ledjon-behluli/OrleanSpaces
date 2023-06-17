using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Evaluations;

internal sealed class EvaluationChannel<T> : IConsumable
    where T : ISpaceTuple
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<Func<Task<T>>> channel
        = Channel.CreateUnbounded<Func<Task<T>>>(new() { SingleReader = true });

    public ChannelReader<Func<Task<T>>> Reader => channel.Reader;
    public ChannelWriter<Func<Task<T>>> Writer => channel.Writer;
}