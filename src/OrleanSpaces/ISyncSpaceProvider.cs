using OrleanSpaces.Types;

namespace OrleanSpaces;

public interface ISyncSpaceProvider
{
    SpaceTuple? TryPeek(SpaceTemplate template);
}