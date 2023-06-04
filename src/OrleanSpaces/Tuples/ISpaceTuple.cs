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
}

public interface ISpaceTemplate<T>
    where T : unmanaged
{
    ref readonly T? this[int index] { get; }
    int Length { get; }

    bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<T>;

    internal ISpaceTuple<T> Create(T[] fields);
}

internal interface IManagedTuple<T>
    where T : struct
{
    ISpaceTuple<H> GetSpaceTuple<H>() where H : unmanaged;
}

internal interface INumericTuple<T> : ISpaceTuple<T>
    where T : unmanaged, INumber<T>
{
    Span<T> Fields { get; }
}
