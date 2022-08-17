using OrleanSpaces.Types;

namespace OrleanSpaces;

public interface ISyncSpaceProvider
{
    SpaceTuple Peek(SpaceTemplate template);
}