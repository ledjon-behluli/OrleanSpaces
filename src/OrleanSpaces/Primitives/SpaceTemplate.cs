using Orleans.Concurrency;
using OrleanSpaces.Utils;
using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public struct SpaceTemplate : ISpaceElement, ITuple, IEquatable<SpaceTemplate>
{
    private readonly object[] _fields;

    public int Length => _fields.Length;
    public object this[int index] => _fields[index];

    public SpaceTemplate() : this(new object[0]) { }

    private SpaceTemplate(params object[] fields)
    {
        if (fields.Length == 0)
        {
            throw new ArgumentException($"Construction of '{nameof(SpaceTemplate)}' without any fields is not allowed.");
        }

        _fields = fields;
    }

    public static SpaceTemplate Create(object field)
    {
        if (field is null)
        {
            throw new ArgumentNullException(nameof(field));
        }

        return new(field);
    }

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

    public bool IsSatisfiedBy(SpaceTuple tuple)
    {
        if (tuple.Length != Length)
        {
            return false;
        }

        for (int i = 0; i < tuple.Length; i++)
        {
            if (this[i] is SpaceUnit)
            {
                continue;
            }
            else if (this[i] is Type templateType)
            {
                if (templateType.Equals(tuple[i].GetType()))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (this[i].Equals(tuple[i]))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsSatisfiedBySpan(SpaceTuple tuple)
    {
        if (tuple.Length != Length)
        {
            return false;
        }

        for (int i = 0; i < tuple.Fields.Length; i++)
        {
            if (this[i] is SpaceUnit)
            {
                continue;
            }
            else if (this[i] is Type templateType)
            {
                if (templateType.Equals(tuple.Fields[i].GetType()))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (this[i].Equals(tuple[i]))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsSatisfiedByTraverseBothSides(SpaceTuple tuple)
    {
        if (tuple.Length != Length)
        {
            return false;
        }

        for (int i = 0, j = tuple.Length - 1; i <= j; i++, j--)
        {
            if (_fields[i] is SpaceUnit)
            {
                continue;
            }
            else if (_fields[i] is Type templateType)
            {
                if (templateType.Equals(tuple[i].GetType()))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (_fields[i].Equals(tuple[i]))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool operator ==(SpaceTemplate first, SpaceTemplate second) => first.Equals(second);
    public static bool operator !=(SpaceTemplate first, SpaceTemplate second) => !(first == second);

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

    public override int GetHashCode() => HashCode.Combine(_fields, Length);

    public override string ToString() => $"<{string.Join(", ", _fields)}>";
}
