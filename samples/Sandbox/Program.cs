﻿using OrleanSpaces;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;

ISpaceAgent spaceAgent = null;

spaceAgent.WriteAsync<SpaceTuple>(new SpaceTuple());
spaceAgent.WriteAsync(new IntTuple());

BoolTemplate template = new();
ISpaceTemplate<bool> spaceTemplate = template;

Console.ReadKey();
