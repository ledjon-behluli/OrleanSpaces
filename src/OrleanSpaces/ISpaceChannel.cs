namespace OrleanSpaces;

public interface ISpaceChannel
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Method is thread-safe.</remarks>
    Task<ISpaceAgent> GetAsync();
}
