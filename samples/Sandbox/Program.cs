
using OrleanSpaces.Tuples.Typed;

Span<char> chars = stackalloc char[48];
bool result = new IntTuple().TryFormat(chars, out int w);

Span<char> chars1 = stackalloc char[48];
bool result1 = new IntTuple(1).TryFormat(chars, out int w1);

Span<char> chars2 = stackalloc char[48];
bool result2 = new IntTuple(1, 1).TryFormat(chars, out int w2);

Console.ReadKey();