using Orleans.Concurrency;
using OrleanSpaces.Utils;
using System;
using System.Collections.Immutable;
using System.Data;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public struct SpaceTemplate : ISpaceElement, IEquatable<SpaceTemplate>
{
    private readonly object[] fields;

    public int Length => fields.Length;
    public object this[int index] => fields[index];

    public ReadOnlySpan<object> AsReadOnlySpan() => new(fields);

    public SpaceTemplate() : this(new object[0]) { }

    private SpaceTemplate(params object[] fields)
    {
        if (fields.Length == 0)
        {
            throw new ArgumentException($"Construction of '{nameof(SpaceTemplate)}' without any fields is not allowed.");
        }

        this.fields = fields;
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

        ReadOnlySpan<object> thisSpan = AsReadOnlySpan();
        ReadOnlySpan<object> thatSpan = tuple.AsReadOnlySpan();

        for (int i = 0; i < thatSpan.Length; i++)
        {
            if (thisSpan[i] is SpaceUnit)
            {
                continue;
            }
            else if (thisSpan[i] is Type templateType)
            {
                if (templateType.Equals(thatSpan[i].GetType()))
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
                if (thisSpan[i].Equals(thatSpan[i]))
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
            if (fields[i] is SpaceUnit)
            {
                continue;
            }
            else if (fields[i] is Type templateType)
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
                if (fields[i].Equals(tuple[i]))
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

    //public bool Equals(SpaceTemplate other)
    //{
    //    if (Length != other.Length)
    //    {
    //        return false;
    //    }

    //    ReadOnlySpan<object> thisSpan = AsReadOnlySpan();
    //    ReadOnlySpan<object> thatSpan = other.AsReadOnlySpan();

    //    for (int i = 0; i < Length; i++)
    //    {
    //        if (!thisSpan[i].Equals(thatSpan[i]))
    //        {
    //            return false;
    //        }
    //    }

    //    return true;
    //}

    public bool Equals(SpaceTemplate other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        ReadOnlySpan<object> thisSpan = AsReadOnlySpan();
        ReadOnlySpan<object> thatSpan = other.AsReadOnlySpan();

        for (int i = 0, j = Length - 1; i <= j; i++, j--)
        {
            if (!thisSpan[i].Equals(thatSpan[i]) ||
                !thisSpan[j].Equals(thisSpan[j]))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => HashCode.Combine(fields, Length);

    public override string ToString() => $"<{string.Join(", ", fields)}>";
}
