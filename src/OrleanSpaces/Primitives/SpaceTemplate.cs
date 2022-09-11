using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public readonly struct SpaceTemplate : ISpaceTuple, IEquatable<SpaceTemplate>
{
    private readonly object[] fields;
    public ref readonly object this[int index] => ref fields[index];
    
    public int Length => fields.Length;


    public SpaceTemplate() : this(new object[0]) { }

    private SpaceTemplate(params object[] fields)
    {
        if (fields.Length == 0)
        {
            throw new ArgumentException($"'{nameof(SpaceTemplate)}' without any fields is not valid.");
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

    public static SpaceTemplate Create(SpaceTuple spaceTuple)
    {
        var fields = new object[spaceTuple.Length];
        for (int i = 0; i < spaceTuple.Length; i++)
        {
            fields[i] = spaceTuple[i];
        }

        return new(fields);
    }

    public bool IsSatisfiedBy(SpaceTuple tuple)
    {
        // No need to pass by "ref SpaceTuple tuple" as the size of SpaceTuple is small, so its faster to copy the struct than having a reference to it.
        // In addition the field's themselves can be accessed via "ref readonly".

        if (tuple.Length != Length)
        {
            return false;
        }

        int length = tuple.Length;  // Can safley perform "Loop-Invariant Code Motion" as 'Length' can not be changed.
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

    public override int GetHashCode() => HashCode.Combine(fields, Length);

    public override string ToString() => $"<{string.Join(", ", fields)}>";
}
