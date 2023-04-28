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

//TODO: Test me!
DecimalTuple d1 = new(12345678, 87654321);
DecimalTuple d2 = new(12345678, 87654321);

Console.WriteLine(d1 == d2);

//Console.WriteLine(new IntTuple(1, 2, 3) == new IntTuple(1, 2, 3));
//Console.WriteLine(new CharTuple('a', 'b', 'c') == new CharTuple('a', 'b', 'c'));
//
//Console.WriteLine(new StringTuple("abc") == new StringTuple("abc"));
//Console.WriteLine(new StringTuple("abc") == new StringTuple("abd"));
//Console.WriteLine(new StringTuple("abc", "cba") == new StringTuple("cbc", "cba"));
//Console.WriteLine(new StringTuple("abc", "cba") == new StringTuple("abc", "dba"));

Console.ReadKey();