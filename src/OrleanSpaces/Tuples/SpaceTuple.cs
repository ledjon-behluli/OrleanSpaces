using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples;

/// <summary>
/// Represents a tuple which has different kinds of field types.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct SpaceTuple : 
    ISpaceTuple,
    IEquatable<SpaceTuple>
{
    [Id(0), JsonProperty] private readonly object[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Returns the field specified by <paramref name="index"/>.
    /// </summary>
    public readonly object this[int index] => fields[index];

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public SpaceTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    /// <remarks><i>Tuple fields can be of type: <see cref="Type.IsPrimitive"/>, <see cref="Enum"/>, <see cref="string"/>, 
    /// <see cref="decimal"/>, <see cref="Int128"/>, <see cref="UInt128"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Guid"/>.</i></remarks>
    /// <exception cref="ArgumentException"/>
    public SpaceTuple([AllowNull] params object[] fields)
    {
        if (fields is null)
        {
            this.fields = Array.Empty<object>();
            return;
        }

        this.fields = new object[fields.Length];

        for (int i = 0; i < fields.Length; i++)
        {
            object? obj = fields[i];
            if (obj is null)
            {
                ThrowHelpers.NullField(i);
            }
            else
            {
                if (!obj.GetType().IsSupportedType())
                {
                    ThrowHelpers.InvalidFieldType(i);
                }

                this.fields[i] = obj;
            }
        }
    }

    /// <summary>
    /// Returns a <see cref="SpaceTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="SpaceTemplate"/> is created.</i></remarks>
    public SpaceTemplate ToTemplate()
    {
        int length = Length;
        object?[] fields = new object?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new SpaceTemplate(fields);
    }

    /// <summary>
    /// Determines whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="other"/> share the same number of fields, and all of them match on the type, value and index; otherwise, <see langword="false"/>.</returns>
    public bool Equals(SpaceTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < Length; i++)
        {
            if (!this[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    /// <summary>
    /// Returns an enumerator to enumerate over the fields of this tuple.
    /// </summary>
    public ReadOnlySpan<object>.Enumerator GetEnumerator() => new ReadOnlySpan<object>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has different kinds of field types.
/// </summary>
public readonly record struct SpaceTemplate : 
    ISpaceTemplate,
    IEquatable<SpaceTemplate>
{
    private readonly object?[] fields;
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Returns the field specified by <paramref name="index"/>.
    /// </summary>
    public readonly object? this[int index] => fields[index];

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public SpaceTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    /// <remarks><i>Template fields can be of type: <see cref="Type"/>, <see cref="Type.IsPrimitive"/>, <see cref="Enum"/>, <see langword="null"/>, <see cref="string"/>, 
    /// <see cref="decimal"/>, <see cref="Int128"/>, <see cref="UInt128"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Guid"/>.</i></remarks>
    /// <exception cref="ArgumentException"/>
    public SpaceTemplate([AllowNull] params object?[] fields)
    {
        if (fields is null || fields.Length == 0)
        {
            this.fields = new object?[1] { null };
            return;
        }

        this.fields = new object[fields.Length];

        for (int i = 0; i < fields.Length; i++)
        {
            object? obj = fields[i];
            if (obj is not null)
            {
                if (!obj.GetType().IsSupportedType() && obj is not Type)
                {
                    ThrowHelpers.InvalidFieldType(i);
                }
            }

            this.fields[i] = obj;
        }
    }

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the type, index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/>, or of type <see cref="Type"/> and matches the respective field type of
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

    public bool Equals(SpaceTemplate other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < Length; i++)
        {
            object? iLeft = this[i];
            object? iRight = other[i];

            if ((iLeft is null && iRight is not null) ||
                (iLeft is not null && iRight is null) ||
                (iLeft is { } l && !l.Equals(iRight)))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields.Select(field => field ?? "{NULL}"))})";

    /// <summary>
    /// Returns an enumerator to enumerate over the fields of this tuple.
    /// </summary>
    public ReadOnlySpan<object?>.Enumerator GetEnumerator() => new ReadOnlySpan<object?>(fields).GetEnumerator();
}