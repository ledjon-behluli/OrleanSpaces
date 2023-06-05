using System.Numerics;

namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{
   
}

public interface ISpaceTuple<T> : ISpaceTuple
    where T : unmanaged
{
    ref readonly T this[int index] { get; }
    int Length { get; }

    ReadOnlySpan<char> AsSpan();
    ReadOnlySpan<T>.Enumerator GetEnumerator();
}

public interface ISpaceTemplate<T>
    where T : unmanaged
{
    ref readonly T? this[int index] { get; }
    int Length { get; }

    bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<T>;
    ReadOnlySpan<T?>.Enumerator GetEnumerator();

    internal ISpaceTuple<T> Create(T[] fields);
}

internal interface INumericTuple<T> : ISpaceTuple<T>
    where T : unmanaged, INumber<T>
{
    Span<T> Fields { get; }
}
