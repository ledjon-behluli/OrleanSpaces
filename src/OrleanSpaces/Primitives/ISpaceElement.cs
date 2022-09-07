namespace OrleanSpaces.Primitives;

internal interface ISpaceTuple
{
    ref readonly object this[int index] { get; }
    int Length { get; }
}