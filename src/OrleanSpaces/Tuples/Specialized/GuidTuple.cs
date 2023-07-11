using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct GuidTuple :
    IEquatable<GuidTuple>,
    ISpaceTuple<Guid>, 
    ISpaceFactory<Guid, GuidTuple>,
    ISpaceConvertible<Guid, GuidTemplate>
{
    [Id(0), JsonProperty] private readonly Guid[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly Guid this[int index] => ref fields[index];

    public GuidTuple() => fields = Array.Empty<Guid>();
    public GuidTuple([AllowNull] params Guid[] fields)
        => this.fields = fields is null ? Array.Empty<Guid>() : fields;

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
            return this.SequentialEquals(other);
        }

        for (int i = 0; i < Length; i++)
        {
            // We are transforming the managed pointer(s) of type 'Guid' (obtained after re-interpreting the readonly reference(s) 'fields[i]' and 'other.fields[i]' to new mutable reference(s))
            // to new managed pointer(s) of type 'Vector128<byte>' and comparing them.

            ref Vector128<byte> vLeft = ref SpaceHelpers.CastAs<Guid, Vector128<byte>>(in fields[i]);
            ref Vector128<byte> vRight = ref SpaceHelpers.CastAs<Guid, Vector128<byte>>(in other.fields[i]);

            if (vLeft != vRight)
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static GuidTuple ISpaceFactory<Guid, GuidTuple>.Create(Guid[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Guid);
    public ReadOnlySpan<Guid>.Enumerator GetEnumerator() => new ReadOnlySpan<Guid>(fields).GetEnumerator();
}

public readonly record struct GuidTemplate : 
    IEquatable<GuidTemplate>,
    ISpaceTemplate<Guid>, 
    ISpaceMatchable<Guid, GuidTuple>
{
    private readonly Guid?[] fields;

    public ref readonly Guid? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public GuidTemplate() => fields = Array.Empty<Guid?>();
    public GuidTemplate([AllowNull] params Guid?[] fields)
        => this.fields = fields is null ? Array.Empty<Guid?>() : fields;

    public bool Matches(GuidTuple tuple) => this.Matches<Guid, GuidTuple>(tuple);
    public bool Equals(GuidTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<Guid?>.Enumerator GetEnumerator() => new ReadOnlySpan<Guid?>(fields).GetEnumerator();
}
