using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Evaluations;

internal sealed class EvaluationChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<Func<Task<SpaceTuple>>> channel
        = Channel.CreateUnbounded<Func<Task<SpaceTuple>>>(new() { SingleReader = true });

    public ChannelReader<Func<Task<SpaceTuple>>> Reader => channel.Reader;
    public ChannelWriter<Func<Task<SpaceTuple>>> Writer => channel.Writer;
}

internal sealed class EvaluationChannel<T, TTuple> : IConsumable
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<Func<Task<TTuple>>> channel
        = Channel.CreateUnbounded<Func<Task<TTuple>>>(new() { SingleReader = true });

    public ChannelReader<Func<Task<TTuple>>> Reader => channel.Reader;
    public ChannelWriter<Func<Task<TTuple>>> Writer => channel.Writer;
}