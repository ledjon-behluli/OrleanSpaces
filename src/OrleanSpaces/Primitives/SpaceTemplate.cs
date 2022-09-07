using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public struct SpaceTemplate : ISpaceTuple, IEquatable<SpaceTemplate>
{
    private readonly object[] fields;

    public int Length => fields.Length;
    public ref readonly object this[int index] => ref fields[index];

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

    public bool IsSatisfiedByTraverseBothSides(SpaceTuple tuple)
    {
        if (tuple.Length != Length)
        {
            return false;
        }

        for (int i = 0, j = tuple.Length - 1; i <= j; i++, j--)
        {
            if (Check(i, ref this, ref tuple) && Check(j, ref this, ref tuple))

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

        static bool Check(int index, ref SpaceTemplate template, ref SpaceTuple tuple)
        {
            if (template[index] is SpaceUnit)
            {
                return true;
            }
            else if (template[index] is Type templateType)
            {
                if (templateType.Equals(tuple[index].GetType()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (template[index].Equals(tuple[index]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
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

        for (int i = 0, j = Length - 1; i <= j; i++, j--)
        {
            if (!this[i].Equals(other[i]) ||
                !this[j].Equals(other[j]))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => HashCode.Combine(fields, Length);

    public override string ToString() => $"<{string.Join(", ", fields)}>";
}
