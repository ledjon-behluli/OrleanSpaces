using OrleanSpaces;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;

ISpaceAgent<IntTuple, IntTemplate> spaceAgent = null;

var a = spaceAgent.WriteAsync(new(1, 2 , 3));
spaceAgent.PeekAsync(new(1, 2, new(1), new(2), new()));

IntTemplate intTemplate = new();
var b = intTemplate[0];

Console.ReadKey();
