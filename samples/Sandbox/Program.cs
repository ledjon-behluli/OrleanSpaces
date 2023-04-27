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

Console.WriteLine(new CharTuple('1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i') == new CharTuple(
    '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i'));
Console.WriteLine(new CharTuple('1', '2', '3') == new CharTuple('1', '2', '3'));
Console.WriteLine(new StringTuple("abc", "cba") == new StringTuple("abc", "cba"));

//Console.WriteLine(new StringTuple("a12", "21a") == new StringTuple("a12", "21a"));

Console.ReadKey();