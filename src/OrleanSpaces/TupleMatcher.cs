using System;

namespace OrleanSpaces
{
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
                if (template[i] is Type templateType)
                {
                    result &= templateType == tuple[i].GetType();
                }
                else
                {
                    result &= tuple[i].Equals(template[i]);
                }

                if (!result)
                {
                    return false;
                }
            }

            return result;
        }
    }
}