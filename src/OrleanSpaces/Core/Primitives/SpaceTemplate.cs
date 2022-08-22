using System.Runtime.CompilerServices;

namespace OrleanSpaces.Core.Primitives;

[Serializable]
public sealed class SpaceTemplate : ITuple, IEquatable<SpaceTemplate>
{
    private readonly object[] _fields;

    public int Length => _fields.Length;
    public object this[int index] => _fields[index];

    private SpaceTemplate(params object[] fields)
    {
        if (fields.Length == 0)
        {
            throw new ArgumentException($"Construction of '{nameof(SpaceTemplate)}' without any fields is not allowed."); ;
        }

        _fields = fields;
    }

    public static SpaceTemplate Create(object field) => new(field);
    public static SpaceTemplate Create(ITuple tuple)
    {
        if (tuple is null)
        {
            throw new ArgumentNullException(nameof(tuple));
        }

        var fields = new object[tuple.Length];

        for (int i = 0; i < tuple.Length; i++)
        {
            fields[i] = tuple[i];
        }

        return new(fields);
    }

    public static bool operator ==(SpaceTemplate? first, SpaceTemplate? second)
    {
        if (first is null || second is null)
            return false;

        return first.Equals(second);
    }
    public static bool operator !=(SpaceTemplate? first, SpaceTemplate? second) => !(first == second);


    public static implicit operator SpaceTemplate(SpaceTuple tuple) => Create(tuple);

    public override bool Equals(object obj) =>
        obj is SpaceTemplate overrides && Equals(overrides);

    public bool Equals(SpaceTemplate other)
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

    public override int GetHashCode() => HashCode.Combine(_fields, Length);

    public override string ToString() => $"<{string.Join(", ", _fields)}>";
}
