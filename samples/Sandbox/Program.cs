using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Runtime.InteropServices;

//Span<char> chars = stackalloc char[48];
//bool result = new IntTuple(1, 1, 1).TryFormat(chars, out int w);

//foreach (ref readonly int item in new IntTuple(1, 1, 1))
//{
//    item = 1;
//    Console.WriteLine(item);
//}

char[] a = new char[] { 'a' };
Span<char> aSpan = new(a);

var bSpan = MemoryMarshal.Cast<char, ushort>(aSpan);
var cSpan = MemoryMarshal.Cast<ushort, char>(bSpan);

var dSpan = MemoryMarshal.Cast<char, byte>(aSpan);
var eSpan = MemoryMarshal.Cast<byte, char>(dSpan);

var b = bSpan.Length;
var c = cSpan.Length;

var d = dSpan.Length;
var e = eSpan.Length;

Console.ReadKey();