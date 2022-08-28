namespace OrleanSpaces;

public interface ISpaceChannelProxy
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Method is thread-safe.</remarks>
    Task<ISpaceChannel> OpenAsync();
}
