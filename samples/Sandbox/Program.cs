using Newtonsoft.Json.Linq;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

//Span<char> chars = stackalloc char[48];
//bool result = new IntTuple(1, 1, 1).TryFormat(chars, out int w);

//foreach (ref readonly int item in new IntTuple(1, 1, 1))
//{
//    item = 1;
//    Console.WriteLine(item);
//}

//Console.WriteLine(new DecimalTuple(1) == new DecimalTuple(1));

Console.WriteLine(new DecimalTuple(111111111111111, 2222222222222222, 33333333333333333) == new DecimalTuple(111111111111111, 2222222222222222, 33333333333333333));

//Console.WriteLine(new IntTuple(1, 2, 3) == new IntTuple(1, 2, 3));
//Console.WriteLine(new CharTuple('a', 'b', 'c') == new CharTuple('a', 'b', 'c'));

//Console.WriteLine(new StringTuple("abc") == new StringTuple("abc"));
//Console.WriteLine(new StringTuple("abc") == new StringTuple("abd"));
//Console.WriteLine(new StringTuple("abc", "cba") == new StringTuple("cbc", "cba"));
//Console.WriteLine(new StringTuple("abc", "cba") == new StringTuple("abc", "dba"));

//Console.WriteLine(new StringTuple("123456789", "123456789") == new StringTuple("123456789", "123456789"));

Console.ReadKey();