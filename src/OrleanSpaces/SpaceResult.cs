using System;

namespace OrleanSpaces
{
    [Serializable]
    public struct SpaceResult
    {
        public bool Result { get; }
        public SpaceTuple? Tuple { get; }

        internal static SpaceResult Success(SpaceTuple tuple) => new SpaceResult(true, tuple);
        internal static SpaceResult Fail() => new SpaceResult(false, null);

        private SpaceResult(bool success, SpaceTuple? tuple)
        {
            Result = success;
            Tuple = tuple;
        }
    }
}
