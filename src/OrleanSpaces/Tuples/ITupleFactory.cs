namespace OrleanSpaces.Tuples;

internal interface ITupleFactory<T>
    where T : struct
{ 
    ISpaceTuple<T> Create(T[] fields);
}