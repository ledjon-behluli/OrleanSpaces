using Orleans;
using OrleanSpaces.Types;

namespace OrleanSpaces;

public interface ISyncSpaceProvider : IGrainWithGuidKey
{
    SpaceTuple? TryPeek(SpaceTemplate template);
}