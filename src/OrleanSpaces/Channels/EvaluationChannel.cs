using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Channels;

internal sealed class EvaluationChannel<T>
    where T : ISpaceTuple
{
    private readonly Channel<Func<Task<T>>> channel
        = Channel.CreateUnbounded<Func<Task<T>>>(new()
        {
            SingleReader = true,
            SingleWriter = true
        });

    public ChannelReader<Func<Task<T>>> Reader => channel.Reader;
    public ChannelWriter<Func<Task<T>>> Writer => channel.Writer;
}