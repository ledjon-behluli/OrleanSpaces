using OrleanSpaces;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;

ISpaceAgent<BoolTuple, BoolTemplate> spaceAgent = null;

var a = spaceAgent.WriteAsync(new(true, false));
spaceAgent.PeekAsync(new(true, new SpaceUnit(), false));

Console.ReadKey();
