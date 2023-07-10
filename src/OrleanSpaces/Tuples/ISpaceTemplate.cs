namespace OrleanSpaces.Tuples;

public interface ISpaceTemplate 
{
    int Length { get; }
}

public interface ISpaceTemplate<T> : ISpaceTemplate
    where T : unmanaged
{
    ref readonly T? this[int index] { get; }
    ReadOnlySpan<T?>.Enumerator GetEnumerator();
}

internal interface ISpaceMatchable<T, TTuple>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
{
    bool Matches(TTuple tuple);
}