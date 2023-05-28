using System.Numerics;

namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{
   
}

public interface ISpaceTuple<T> : ISpaceTuple
    where T : struct
{
    ref readonly T this[int index] { get; }
    int Length { get; }
}

internal interface INumericTuple<T> : ISpaceTuple<T>
    where T : struct, INumber<T>
{
    Span<T> Fields { get; }
}

public interface ISpaceTemplate<T>
    where T : struct
{
    ref readonly T? this[int index] { get; }
    int Length { get; }

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<T>;
}