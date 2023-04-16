﻿using System.Numerics;

namespace OrleanSpaces.Tuples.Typed.Numerics;

internal interface INumericTuple<T, H> : ISpaceTuple<T, H>
    where T : struct, INumber<T>
    where H : ISpaceTuple<T, H>
{
    Span<T> Data { get; }
}