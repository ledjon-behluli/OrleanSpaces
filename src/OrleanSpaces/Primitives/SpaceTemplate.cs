using Orleans.Concurrency;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Immutable]
public readonly partial struct SpaceTemplate : ITuple, IEquatable<SpaceTemplate>, IComparable<SpaceTemplate>
{
    private readonly object[] fields;

    public object this[int index] => fields[index];
    public int Length => fields.Length;

    public SpaceTemplate()
    {
        fields = new object[1] { SpaceUnit.Null };
    }

    public SpaceTemplate(string value)
    {
        if (value is null) 
        {
            throw new ArgumentNullException(nameof(value));
        }

        fields = new object[1] { value };
    }

    public SpaceTemplate(Type type)
    {
        if (type is null) 
        {
            throw new ArgumentNullException(nameof(type));
        }

        fields = new object[1] { type };
    }

    public SpaceTemplate(ValueType valueType)
    {
        if (valueType is null) 
        {
            throw new ArgumentNullException(nameof(valueType));
        }

        if (valueType is ITuple _tuple)
        {
            fields = new object[_tuple.Length];
            for (int i = 0; i < _tuple.Length; i++)
            {
                ThrowIfNotSupported(_tuple[i], i);
                fields[i] = _tuple[i];
            }
        }
        else
        {
            ThrowIfNotSupported(valueType);
            fields = new object[1] { valueType };
        }

        static void ThrowIfNotSupported(object obj, int index = 0)
        {
            Type type = obj.GetType();

            if (!TypeChecker.IsSimpleType(type) && type != typeof(SpaceUnit) && obj is not Type)
            {
                throw new ArgumentException($"The field at position = {index}, is not a valid type. Allowed types include: strings, primitives, enums and '{nameof(SpaceUnit)}'.");
            }
        }
    }

    public bool IsSatisfiedBy(SpaceTuple tuple)
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

    public static implicit operator SpaceTemplate(SpaceTuple tuple) => new(tuple);

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