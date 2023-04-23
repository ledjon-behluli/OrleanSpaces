using OrleanSpaces.Tuples.Typed;

Span<char> chars = stackalloc char[48];
bool result = new IntTuple(1, 1, 1).TryFormat(chars, out int w);

Console.ReadKey();