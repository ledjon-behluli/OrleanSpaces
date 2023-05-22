using OrleanSpaces;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;

StringTuple a = new("abc", "123");
//var aaa = a.AsSpan();

Console.WriteLine(a[0]);
Console.WriteLine(((ISpaceTuple<char>)a)[0]);
Console.WriteLine(((ISpaceTuple<char>)a)[4]);


Console.ReadKey();
