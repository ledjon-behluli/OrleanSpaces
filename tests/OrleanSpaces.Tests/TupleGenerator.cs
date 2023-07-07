using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;
using System.Collections;

namespace OrleanSpaces.Tests;

public class TupleGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> data = new()
    {
        new object[] { new SpaceTuple(1) },
        new object[] { new SpaceTuple(1, "a") },
        new object[] { new SpaceTuple(1, "a", 1.5f) },
        new object[] { new SpaceTuple(1, "a", 1.5f, true) }
    };

    public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class EmptyTupleGenerator : IEnumerable<ISpaceTuple[]>
{
    private readonly List<ISpaceTuple[]> data = new()
    {
        new ISpaceTuple[] { new SpaceTuple() },
        new ISpaceTuple[] { new BoolTuple() },
        new ISpaceTuple[] { new CharTuple() },
        new ISpaceTuple[] { new DateTimeOffsetTuple() },
        new ISpaceTuple[] { new DateTimeTuple() },
        new ISpaceTuple[] { new DecimalTuple() },
        new ISpaceTuple[] { new DoubleTuple() },
        new ISpaceTuple[] { new FloatTuple() },
        new ISpaceTuple[] { new GuidTuple() },
        new ISpaceTuple[] { new HugeTuple() },
        new ISpaceTuple[] { new IntTuple() },
        new ISpaceTuple[] { new LongTuple() },
        new ISpaceTuple[] { new SByteTuple() },
        new ISpaceTuple[] { new ShortTuple() },
        new ISpaceTuple[] { new TimeSpanTuple() },
        new ISpaceTuple[] { new UHugeTuple() },
        new ISpaceTuple[] { new UIntTuple() },
        new ISpaceTuple[] { new ULongTuple() },
        new ISpaceTuple[] { new UShortTuple() }
    };

    public IEnumerator<ISpaceTuple[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}