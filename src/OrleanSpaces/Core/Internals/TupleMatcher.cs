namespace OrleanSpaces.Core.Internals;

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
            if (template[i] is NullTuple)
            {
                result = true;
            }
            else if (template[i] is Type templateType)
            {
                result &= templateType.Equals(tuple[i].GetType());
            }
            else
            {
                result &= template[i].Equals(tuple[i]);
            }

            if (!result)
            {
                return false;
            }
        }

        return result;
    }
}