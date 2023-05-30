using OrleanSpaces;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;

StringTuple tuple = new("aa", "11", "xx");

StringTemplate template1 = new("aa", "11", "xx");
StringTemplate template2 = new("aa", null, "xx");


template1.Matches(tuple);
template2.Matches(tuple);

Console.ReadKey();
