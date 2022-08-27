namespace OrleanSpaces;

public interface ISpaceChannelProxy
{
    ValueTask<ISpaceChannel> OpenAsync(CancellationToken cancellationToken = default);
}
