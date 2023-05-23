namespace OrleanSpaces.Tuples;

public interface ISpaceTuple
{
    object this[int index] { get; }
    int Length { get; }
}

public interface ITyped<T> 
    where T : struct
{

}

public interface ISpaceTuple<T, TSelf> : ITyped<TSelf>
    where T : struct
    where TSelf : struct, ITyped<TSelf>
{
    ref readonly T this[int index] { get; }
    int Length { get; }

    ReadOnlySpan<char> AsSpan();
}