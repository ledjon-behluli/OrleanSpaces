using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;

//Span<char> chars = stackalloc char[48];
//bool result = new IntTuple(1, 1, 1).TryFormat(chars, out int w);

//foreach (ref readonly int item in new IntTuple(1, 1, 1))
//{
//    item = 1;
//    Console.WriteLine(item);
//}

//Int128 ii = new(18446744073709551615, 18446744073709551615);
//Int128 i = 18446744073709551615;

Int128Tuple i1 = new(Int128.MaxValue);
Int128Tuple i2 = new(Int128.MaxValue);

Console.WriteLine(i1 == i2);

Console.ReadKey();