namespace OrleanSpaces.Tuples;

public interface ISpaceTemplate 
{
    int Length { get; }
}

public interface ISpaceTemplate<T> : ISpaceTemplate
    where T : unmanaged
{
    ref readonly T? this[int index] { get; }

    bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<T>;
    ReadOnlySpan<T?>.Enumerator GetEnumerator();

    internal ISpaceTuple<T> Create(T[] fields);
}