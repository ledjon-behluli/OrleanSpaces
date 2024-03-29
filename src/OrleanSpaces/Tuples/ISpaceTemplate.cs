﻿namespace OrleanSpaces.Tuples;

/// <summary>
/// Marker interface for any space template.
/// </summary>
[InternalUseOnly]
public interface ISpaceTemplate 
{
    /// <summary>
    /// The length of this template.
    /// </summary>
    int Length { get; }
}

/// <summary>
/// Marker interface for any specialized space template.
/// </summary>
/// <typeparam name="T">Any of the supported non-reference types.</typeparam>
public interface ISpaceTemplate<T> : ISpaceTemplate
    where T : unmanaged
{
    /// <summary>
    /// Returns a readonly reference of the field specified by <paramref name="index"/>.
    /// </summary>
    ref readonly T? this[int index] { get; }

    /// <summary>
    /// Returns an enumerator to enumerate over the fields of this template.
    /// </summary>
    ReadOnlySpan<T?>.Enumerator GetEnumerator();
}

internal interface ISpaceMatchable<T, TTuple>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
{
    bool Matches(TTuple tuple);
}