using Orleans;

namespace OrleanSpaces.Core;

public interface ISpaceGrain : IGrainWithGuidKey, ISpaceWriter, ISpaceReader, ISpaceBlockingReader
{
    
}