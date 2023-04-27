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

Console.WriteLine(new CharTuple('a', 'b', 'c', 'c', 'b', 'a') == new CharTuple('a', 'b', 'c', 'c', 'b', 'a'));
Console.WriteLine(new StringTuple("abc", "cba") == new StringTuple("abc", "cba"));

//Console.WriteLine(new StringTuple("a12", "21a") == new StringTuple("a12", "21a"));

Console.ReadKey();