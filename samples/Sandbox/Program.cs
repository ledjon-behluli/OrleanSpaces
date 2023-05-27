using OrleanSpaces;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;

ISpaceAgent<EnumTuple<MyE>, EnumTemplate<MyE>> spaceAgent = null;

var a = spaceAgent.WriteAsync(new(MyE.A, MyE.A));
spaceAgent.PeekAsync(new(MyE.A, new SpaceUnit(), MyE.A));

Console.ReadKey();

enum MyE : int
{
    A = long.MaxValue
}

enum MyE1 : ulong
{
    A = ulong.MaxValue
}