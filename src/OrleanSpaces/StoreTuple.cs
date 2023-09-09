using OrleanSpaces.Tuples;

namespace OrleanSpaces;

/// <summary>
/// An <see cref="ISpaceTuple"/> that is part of an <see cref="ITupleStore{T}"/> given by the <paramref name="StoreId"/>.
/// </summary>
[GenerateSerializer, Immutable]
internal readonly record struct StoreTuple<T>(Guid StoreId, T Tuple) where T : ISpaceTuple;