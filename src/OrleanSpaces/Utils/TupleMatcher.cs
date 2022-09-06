using OrleanSpaces.Primitives;

namespace OrleanSpaces.Utils;

// TODO: Benchmark performance of this!!!
internal static class TupleMatcher
{
    public static bool IsMatch(SpaceTuple tuple, SpaceTemplate template)
    {
        if (tuple.Length != template.Length)
        {
            return false;
        }

        bool result = true;

        for (int i = 0; i < tuple.Length; i++)
        {
            object field = template[i];

            if (field is UnitField)
            {
                result = true;
            }
            else if (field is Type templateType)
            {
                result &= templateType.Equals(tuple[i].GetType());
            }
            else
            {
                result &= field.Equals(tuple[i]);
            }

            if (!result)
            {
                return false;
            }
        }

        return result;
    }
}
