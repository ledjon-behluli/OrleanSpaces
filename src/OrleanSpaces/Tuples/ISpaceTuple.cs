using System.Numerics;

namespace OrleanSpaces.Tuples;

public interface ISpaceTuple 
{
    int Length { get; }
}

public interface ISpaceTuple<T> : ISpaceTuple
    where T : unmanaged
{  
    ref readonly T this[int index] { get; }

    ReadOnlySpan<char> AsSpan();
    ReadOnlySpan<T>.Enumerator GetEnumerator();

    internal static abstract ISpaceTuple<T> Create(T[] fields);
}

internal interface INumericTuple<T> : ISpaceTuple<T>
    where T : unmanaged, INumber<T>
{
    Span<T> Fields { get; }
}

internal interface ISpaceConvertible<T, TTemplate>
    where T : unmanaged
    where TTemplate : ISpaceTemplate<T>
{
    TTemplate ToTemplate();
}