using OrleanSpaces.Tuples;

namespace OrleanSpaces;

[GenerateSerializer, Immutable]
internal readonly record struct TupleAddress<T>(T Tuple, Guid StoreId) where T : ISpaceTuple;
