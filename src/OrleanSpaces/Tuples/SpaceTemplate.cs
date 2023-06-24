namespace OrleanSpaces.Tuples;

/// <summary>
/// Represents a template (<i>or passive tuple</i>) in the tuple space paradigm.
/// </summary>
[GenerateSerializer, Immutable]
public readonly struct SpaceTemplate : ISpaceTemplate
{
    private readonly object?[] fields;

    public readonly object? this[int index] => fields[index];
    public int Length => fields.Length;

    /// <summary>
    /// Default constructor which always instantiates a <see cref="SpaceTemplate"/> with a single field of type <see cref="SpaceUnit"/>.
    /// </summary>
    public SpaceTemplate() : this(Array.Empty<object?>()) { }

    /// <summary>
    /// Main constructor which instantiates a <see cref="SpaceTemplate"/>, when all <paramref name="fields"/> are of valid type.
    /// </summary>
    /// <param name="fields">The ticks of this template.</param>
    /// <remarks><i>Template ticks can have types of: <see cref="SpaceUnit"/>, <see cref="Type"/>, <see cref="Type.IsPrimitive"/>, <see cref="Enum"/>, <see cref="string"/>, 
    /// <see cref="decimal"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Guid"/>.</i></remarks>
    /// <exception cref="ArgumentException"/>
    public SpaceTemplate(params object?[] fields)
    {
        if (fields is null)
        {
            this.fields = Array.Empty<object?>();
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

    public static bool operator ==(SpaceTemplate left, SpaceTemplate right) => left.Equals(right);
    public static bool operator !=(SpaceTemplate left, SpaceTemplate right) => !(left == right);

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><see langword="true"/>, if <paramref name="obj"/> is of type <see cref="SpaceTuple"/> and <see cref="Equals(SpaceTemplate)"/> returns <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is SpaceTemplate template && Equals(template);
    /// <summary>
    /// Determines whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="other"/> share the same number of fields, and all of them match on the type, value and index; otherwise, <see langword="false"/>.</returns>
    public bool Equals(SpaceTemplate other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < Length; i++)
        {
            if (this[i] is { } field && !field.Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>Whatever the result of length comparison between <see langword="this"/> and <paramref name="other"/> is.</returns>
    public int CompareTo(SpaceTemplate other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields.Select(field => field ?? "{NULL}"))})";

    public ReadOnlySpan<object?>.Enumerator GetEnumerator() => new ReadOnlySpan<object?>(fields).GetEnumerator();
}