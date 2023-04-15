﻿using System.Numerics;

namespace OrleanSpaces.Tuples;

internal interface INumericTuple<T, H> : ISpaceTuple<T, H>
    where T : struct, INumber<T>
    where H : ISpaceTuple<T, H>
{
    Span<T> Data { get; }
}

internal interface IStronglyTypedTuple<T, H> : ISpaceTuple<T, H>
    where T : struct, INumber<T>
    where H : ISpaceTuple<T, H>
{
    Span<T> Data { get; }
}