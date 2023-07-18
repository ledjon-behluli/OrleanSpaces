using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="Guid"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct GuidTuple :
    IEquatable<GuidTuple>,
    ISpaceTuple<Guid>, 
    ISpaceFactory<Guid, GuidTuple>,
    ISpaceConvertible<Guid, GuidTemplate>
{
    [Id(0), JsonProperty] private readonly Guid[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly Guid this[int index] => ref fields[index];

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public GuidTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public GuidTuple([AllowNull] params Guid[] fields)
        => this.fields = fields is null ? Array.Empty<Guid>() : fields;

    /// <summary>
    /// Returns a <see cref="GuidTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="GuidTemplate"/> is created.</i></remarks>
    public GuidTemplate ToTemplate()
    {
        int length = Length;
        Guid?[] fields = new Guid?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new GuidTemplate(fields);
    }


    public bool Equals(GuidTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (!Vector128.IsHardwareAccelerated)
        {
            return this.SequentialEquals<Guid, GuidTuple>(other);
        }

        for (int i = 0; i < Length; i++)
        {
            // We are transforming the managed pointer(s) of type 'Guid' (obtained after re-interpreting the readonly reference(s) 'fields[i]' and 'other.fields[i]' to new mutable reference(s))
            // to new managed pointer(s) of type 'Vector128<byte>' and comparing them.

            ref Vector128<byte> vLeft = ref Helpers.Helpers.CastAs<Guid, Vector128<byte>>(in fields[i]);
            ref Vector128<byte> vRight = ref Helpers.Helpers.CastAs<Guid, Vector128<byte>>(in other.fields[i]);

            if (vLeft != vRight)
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static GuidTuple ISpaceFactory<Guid, GuidTuple>.Create(Guid[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<Guid, GuidTuple>(Constants.MaxFieldCharLength_Guid);
    public ReadOnlySpan<Guid>.Enumerator GetEnumerator() => new ReadOnlySpan<Guid>(fields).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="Guid"/> field types only.
/// </summary>
public readonly record struct GuidTemplate : 
    IEquatable<GuidTemplate>,
    ISpaceTemplate<Guid>, 
    ISpaceMatchable<Guid, GuidTuple>
{
    private readonly Guid?[] fields;

    public ref readonly Guid? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public GuidTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public GuidTemplate([AllowNull] params Guid?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new Guid?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(GuidTuple tuple) => this.Matches<Guid, GuidTuple, GuidTemplate>(tuple);
    public bool Equals(GuidTemplate other) => this.SequentialEquals<Guid, GuidTemplate>(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<Guid?>.Enumerator GetEnumerator() => new ReadOnlySpan<Guid?>(fields).GetEnumerator();
}
