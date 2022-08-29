namespace OrleanSpaces;

public interface ISpaceChannelProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Method is thread-safe.</remarks>
    Task<ISpaceChannel> GetAsync();
}
