using System.Numerics;

namespace OrleanSpaces.Tuples;

public interface ISpaceTuple { }

public interface ISpaceTuple<T> : ISpaceTuple
    where T : unmanaged
{
    ref readonly T this[int index] { get; }
    int Length { get; }

    ReadOnlySpan<char> AsSpan();
    ReadOnlySpan<T>.Enumerator GetEnumerator();

    internal ISpaceTemplate<T> ToTemplate();
}

internal interface INumericTuple<T> : ISpaceTuple<T>
    where T : unmanaged, INumber<T>
{
    Span<T> Fields { get; }
}
