using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.InteropServices;

//Span<char> chars = stackalloc char[48];
//bool result = new IntTuple(1, 1, 1).TryFormat(chars, out int w);

//foreach (ref readonly int item in new IntTuple(1, 1, 1))
//{
//    item = 1;
//    Console.WriteLine(item);
//}

CharTuple tuple1 = new('a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');
CharTuple tuple2 = new('a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

bool result = tuple1.Equals(tuple2);

Console.ReadKey();