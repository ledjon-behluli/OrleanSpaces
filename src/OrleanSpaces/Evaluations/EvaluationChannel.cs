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

internal sealed class EvaluationChannel<T, TTuple, TTemplate> : IConsumable
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<Func<Task<TTuple>>> tupleChannel =
        Channel.CreateUnbounded<Func<Task<TTuple>>>(new() { SingleReader = true });

    private readonly Channel<Func<Task<TTemplate>>> templateChannel =
     Channel.CreateUnbounded<Func<Task<TTemplate>>>(new() { SingleReader = true });

    public ChannelReader<Func<Task<TTuple>>> TupleReader => tupleChannel.Reader;
    public ChannelWriter<Func<Task<TTuple>>> TupleWriter => tupleChannel.Writer;

    public ChannelReader<Func<Task<TTemplate>>> TemplateReader => templateChannel.Reader;
    public ChannelWriter<Func<Task<TTemplate>>> TemplateWriter => templateChannel.Writer;
}