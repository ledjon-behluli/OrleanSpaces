using OrleanSpaces;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;

StringTuple tuple = new("12", "34", "56");

StringTemplate template1 = new("12", "34", "56");
template1.Matches(tuple);

Console.ReadKey();
