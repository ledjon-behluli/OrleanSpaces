using OrleanSpaces.Tuples;
using System.Threading.Channels;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackChannel<TTuple>
    where TTuple : ISpaceTuple
{ 
    private readonly Channel<TTuple> channel =
        Channel.CreateUnbounded<TTuple>(new()
        {
            SingleReader = true,
            SingleWriter = true
        });

    public ChannelReader<TTuple> Reader => channel.Reader;
    public ChannelWriter<TTuple> Writer => channel.Writer;
}