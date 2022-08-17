using OrleanSpaces.Types;

namespace OrleanSpaces;

public interface ISyncSpaceProvider
{
    TupleResult TryPeek(SpaceTemplate template);
}