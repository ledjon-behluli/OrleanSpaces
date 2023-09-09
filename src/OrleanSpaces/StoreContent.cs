using OrleanSpaces.Tuples;
using System.Collections.Immutable;

namespace OrleanSpaces;

/// <summary>
/// The contents (<paramref name="Tuples"/>) of an <see cref="ITupleStore{T}"/> given by the <paramref name="StoreKey"/>.
/// </summary>
[GenerateSerializer, Immutable]
internal readonly record struct StoreContent<T>(string StoreKey, ImmutableArray<T> Tuples) where T : ISpaceTuple;