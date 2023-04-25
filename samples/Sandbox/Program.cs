using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

//Span<char> chars = stackalloc char[48];
//bool result = new IntTuple(1, 1, 1).TryFormat(chars, out int w);

//foreach (ref readonly int item in new IntTuple(1, 1, 1))
//{
//    item = 1;
//    Console.WriteLine(item);
//}

//CharTuple tuple1 = new('a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');
//CharTuple tuple2 = new('a', 'a', 'a', 'a', 'a', 'a', 'a', 'a');

//bool result = tuple1.Equals(tuple2);

//IntTuple i1 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
//IntTuple i2 = new(1, 2, 3, 4, 5, 6, 7, 8, 9, 11);

//Console.WriteLine(i1.Equals(i2));

LongTuple l1 = new(1, 1, 1);
LongTuple l2 = new(1, 1, 1);

Console.WriteLine(l1.Equals(l2));

Console.ReadKey();