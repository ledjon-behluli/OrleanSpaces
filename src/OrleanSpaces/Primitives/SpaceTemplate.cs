using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public struct SpaceTemplate : ITuple, IEquatable<SpaceTemplate>
{
    private readonly ITuple tuple;

    public object this[int index] => tuple[index];
    public int Length => tuple.Length;

    public SpaceTemplate() : this(SpaceUnit.Null) { }
    private SpaceTemplate(ITuple tuple) => this.tuple = tuple;

    public static SpaceTemplate Create(ValueType value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value is ITuple tuple)
        {
            for (int i = 0; i < tuple.Length; i++)
            {
                ThrowOnNotSupported(tuple[i], i);
            }

            return new(tuple);
        }

        ThrowOnNotSupported(value);

        return new(new ValueTuple<ValueType>(value));
    }

    private static void ThrowOnNotSupported(object obj, int index = 0)
    {
        Type type = obj.GetType();

        if (!TypeChecker.IsSimpleType(type) && type != typeof(SpaceUnit) && obj is not Type)
        {
            throw new ArgumentException(
                $"The field at position = {index}, is not a valid '{nameof(SpaceTuple)}' member. " +
                $"Valid members are: '{nameof(String)}', '{nameof(ValueType)}'");
        }
    }

    public static SpaceTemplate Create(Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return new(new ValueTuple<Type>(type));
    }

    public static SpaceTemplate Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new(new ValueTuple<string>(value));
    }

    public bool IsSatisfiedBy(SpaceTuple tuple)
    {
        if (tuple.Length != Length)
        {
            return false;
        }

        int length = tuple.Length;
        int i = 0;

        do
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

            i++;
            length--;
        }
        while (length > 0);

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

    public override int GetHashCode() => HashCode.Combine(tuple, Length);

    public override string ToString() => Length == 1 && tuple[0] is SpaceUnit ?
        $"({SpaceUnit.Null})" : tuple.ToString();
}
