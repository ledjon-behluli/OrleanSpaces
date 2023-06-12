namespace OrleanSpaces.Tuples;

public interface ISpaceTemplate<T> : ISpaceElement
    where T : unmanaged
{
    ref readonly T? this[int index] { get; }
    int Length { get; }

    bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<T>;
    ReadOnlySpan<T?>.Enumerator GetEnumerator();

    internal ISpaceTuple<T> Create(T[] fields);
}