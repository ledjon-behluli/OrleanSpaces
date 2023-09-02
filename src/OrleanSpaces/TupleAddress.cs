using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal record struct TupleAddress<T>(T Tuple, Guid StoreId) where T : ISpaceTuple;
