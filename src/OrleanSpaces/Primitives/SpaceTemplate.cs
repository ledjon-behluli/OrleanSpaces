using Orleans.Concurrency;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Immutable]
public readonly partial struct SpaceTemplate : ITuple, IEquatable<SpaceTemplate>, IComparable<SpaceTemplate>
{
    private readonly object[] fields;

    public object this[int index] => fields[index];
    public int Length => fields.Length;

    public SpaceTemplate() : this(new object[0])
    {

    }

    public SpaceTemplate(params object[] fields)
    {
        if (fields == null)
        {
            throw new ArgumentNullException(nameof(fields));
        }

        if (fields.Length == 0)
        {
            this.fields = new object[1] { SpaceUnit.Null };
        }
        else
        {
            this.fields = new object[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                object obj = fields[i];
                Type type = obj.GetType();

                if (!TypeChecker.IsSimpleType(type) && type != typeof(SpaceUnit) && obj is not Type)
                {
                    throw new ArgumentException($"The field at position = {i}, is not a valid type. Allowed types are: strings, primitives, enums and '{nameof(SpaceUnit)}'.");
                }

                this.fields[i] = obj;
            }
        }
    }

    public bool Matches(SpaceTuple tuple)
    {
        if (tuple.Length != Length)
        {
            return false;
        }

        for (int i = 0; i < tuple.Length; i++)
        {
            if (this[i] is not SpaceUnit)
            {
                if (this[i] is Type templateType)
                {
                    if (!templateType.Equals(tuple[i].GetType()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!this[i].Equals(tuple[i]))
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

    public override bool Equals(object obj) => obj is SpaceTemplate template && Equals(template);

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

    public int CompareTo(SpaceTemplate other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}