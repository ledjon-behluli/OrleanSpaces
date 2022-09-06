using OrleanSpaces.Primitives;

namespace OrleanSpaces.Utils;

internal static class TupleMatcher
{
    public static bool IsMatch(SpaceTuple tuple, SpaceTemplate template)
    {
        if (tuple.Length != template.Length)
        {
            return false;
        }

        for (int i = 0; i < tuple.Length; i++)
        {
            if (template[i] is UnitField)
            {
                continue;
            }
            else if (template[i] is Type templateType)
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
                if (template[i].Equals(tuple[i]))
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
}