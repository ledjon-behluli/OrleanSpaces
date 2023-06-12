using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples;

/// <summary>
/// Represents a template (<i>or passive tuple</i>) in the tuple space paradigm.
/// </summary>
[Immutable]
public readonly struct SpaceTemplate : ISpaceElement
{
    private readonly object?[] fields;

    public readonly object? this[int index] => fields[index];
    public int Length => fields.Length;

    /// <summary>
    /// Default constructor which always instantiates a <see cref="SpaceTemplate"/> with a single field of type <see cref="SpaceUnit"/>.
    /// </summary>
    public SpaceTemplate() : this(null) { }

    /// <summary>
    /// Main constructor which instantiates a <see cref="SpaceTemplate"/>, when all <paramref name="fields"/> are of valid type.
    /// </summary>
    /// <param name="fields">The ticks of this template.</param>
    /// <remarks><i>Template ticks can have types of: <see cref="SpaceUnit"/>, <see cref="Type"/>, <see cref="Type.IsPrimitive"/>, <see cref="Enum"/>, <see cref="string"/>, 
    /// <see cref="decimal"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Guid"/>.</i></remarks>
    /// <exception cref="ArgumentException"/>
    public SpaceTemplate([AllowNull] params object[] fields)
    {
        if (fields == null || fields.Length == 0)
        {
            this.fields = new object?[1] { null };
        }
        else
        {
            this.fields = new object[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                object? obj = fields[i];
                if (obj != null)
                {
                    if (!obj.GetType().IsSupportedType() && obj is not Type)
                    {
                        throw new ArgumentException($"The field at position = {i} is not a valid type.");
                    }
                }

                this.fields[i] = obj;
            }
        }
    }

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by this instance.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of ticks, and all of them match on the type, index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see cref="SpaceUnit"/>, or of type <see cref="Type"/> and matches the respective field type of
    /// <paramref name="tuple"/> at the same index</i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(SpaceTuple tuple)
    {
        if (tuple.Length != Length)
        {
            return false;
        }

        for (int i = 0; i < tuple.Length; i++)
        {
            if (this[i] is { } field)
            {
                if (field is Type templateType)
                {
                    if (!templateType.Equals(tuple[i].GetType()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!field.Equals(tuple[i]))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public static implicit operator SpaceTemplate(SpaceTuple tuple)
    {
        object[] fields = new object[tuple.Length];

        for (int i = 0; i < tuple.Length; i++)
        {
            fields[i] = tuple[i];
        }

        return new(fields);
    }

    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<object?>.Enumerator GetEnumerator() => new ReadOnlySpan<object?>(fields).GetEnumerator();
}