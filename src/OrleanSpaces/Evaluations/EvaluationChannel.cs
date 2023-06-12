using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Evaluations;

internal sealed class EvaluationChannel : IConsumable
{
    public bool IsBeingConsumed { get; set; }

    private readonly Channel<Func<Task<SpaceTuple>>> tupleChannel 
        = Channel.CreateUnbounded<Func<Task<SpaceTuple>>>(new() { SingleReader = true });

    private readonly Channel<Func<Task<SpaceTemplate>>> templateChannel
       = Channel.CreateUnbounded<Func<Task<SpaceTemplate>>>(new() { SingleReader = true });

    public ChannelReader<Func<Task<SpaceTuple>>> TupleReader => tupleChannel.Reader;
    public ChannelWriter<Func<Task<SpaceTuple>>> TupleWriter => tupleChannel.Writer;

    public ChannelReader<Func<Task<SpaceTemplate>>> TemplateReader => templateChannel.Reader;
    public ChannelWriter<Func<Task<SpaceTemplate>>> TemplateWriter => templateChannel.Writer;
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