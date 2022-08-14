using System;
using System.Collections.Generic;

namespace OrleanSpaces
{
    [Serializable]
    internal struct TupleSpaceState
    {
        public List<SpaceTuple> Tuples { get; set; }
    }
}
