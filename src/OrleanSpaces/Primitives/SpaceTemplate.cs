using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public readonly partial struct SpaceTemplate : ITuple, IEquatable<SpaceTemplate>
{
    private readonly ITuple tuple;

    public object this[int index] => tuple[index];
    public int Length => tuple.Length;

    public SpaceTemplate()
    {
        tuple = SpaceUnit.Null;
    }

    public SpaceTemplate(string value)
    {
        if (value is null) 
        {
            throw new ArgumentNullException(nameof(value));
        }

        tuple = new ValueTuple<string>(value);
    }

    public SpaceTemplate(Type type)
    {
        if (type is null) 
        {
            throw new ArgumentNullException(nameof(type));
        }

        tuple = new ValueTuple<Type>(type);
    }

    public SpaceTemplate(ValueType valueType)
    {
        if (valueType is null) 
        {
            throw new ArgumentNullException(nameof(valueType));
        }

        if (valueType is ITuple _tuple)
        {
            for (int i = 0; i < _tuple.Length; i++)
            {
                ThrowIfNotSupported(_tuple[i], i);
            }

            tuple = _tuple;
        }
        else
        {
            ThrowIfNotSupported(valueType);
            tuple = new ValueTuple<ValueType>(valueType);
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

    public override bool Equals(object obj) =>
        obj is SpaceTemplate template && Equals(template);

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

    public override int GetHashCode() => tuple.GetHashCode();

    public override string ToString() => Length == 1 ? $"({tuple[0]})" : tuple.ToString();
}