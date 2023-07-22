using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="double"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct DoubleTuple :
    IEquatable<DoubleTuple>,
    INumericTuple<double>,
    ISpaceFactory<double, DoubleTuple>,
    ISpaceConvertible<double, DoubleTemplate>
{
    [Id(0), JsonProperty] private readonly double[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly double this[int index] => ref fields[index];

    Span<double> INumericTuple<double>.Fields => fields.AsSpan();

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public DoubleTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public DoubleTuple([AllowNull] params double[] fields)
        => this.fields = fields is null ? Array.Empty<double>() : fields;

    /// <summary>
    /// Returns a <see cref="DoubleTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="DoubleTemplate"/> is created.</i></remarks>
    public DoubleTemplate ToTemplate()
    {
        int length = Length;
        double?[] fields = new double?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new DoubleTemplate(fields);
    }

    public bool Equals(DoubleTuple other)
        => this.TryParallelEquals<double, DoubleTuple>(other, out bool result) ? 
               result : this.SequentialEquals<double, DoubleTuple>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TupleHelpers.ToString(fields);

    static DoubleTuple ISpaceFactory<double, DoubleTuple>.Create(double[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<double, DoubleTuple>(Constants.MaxFieldCharLength_Double);
    public ReadOnlySpan<double>.Enumerator GetEnumerator() =>
        (fields is null ? ReadOnlySpan<double>.Empty : new ReadOnlySpan<double>(fields)).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="double"/> field types only.
/// </summary>
public readonly record struct DoubleTemplate : 
    IEquatable<DoubleTemplate>,
    ISpaceTemplate<double>, 
    ISpaceMatchable<double, DoubleTuple>
{
    private readonly double?[] fields;

    public ref readonly double? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public DoubleTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public DoubleTemplate([AllowNull] params double?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new double?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(DoubleTuple tuple) => this.Matches<double, DoubleTuple, DoubleTemplate>(tuple);
    public bool Equals(DoubleTemplate other) => this.SequentialEquals<double, DoubleTemplate>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<double?>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<double?>.Empty : new ReadOnlySpan<double?>(fields)).GetEnumerator();
}
