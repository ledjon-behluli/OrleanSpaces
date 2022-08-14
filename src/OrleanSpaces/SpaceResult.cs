using System;

namespace OrleanSpaces
{
    [Serializable]
    public readonly struct SpaceResult
    {
        public bool Result => Tuple != null;
        public SpaceTuple? Tuple { get; }

        public static SpaceResult Empty = new SpaceResult();

        public SpaceResult(SpaceTuple? tuple)
        {
            Tuple = tuple;
        }
    }
}
