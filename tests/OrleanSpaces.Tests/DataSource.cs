using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;
using System.Collections;

namespace OrleanSpaces.Tests;

public class SpaceTupleGenerator : IEnumerable<object[]>
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

public class EmptyTemplateGenerator : IEnumerable<ISpaceTemplate[]>
{
    private readonly List<ISpaceTemplate[]> data = new()
    {
        new ISpaceTemplate[] { new SpaceTemplate() },
        new ISpaceTemplate[] { new BoolTemplate() },
        new ISpaceTemplate[] { new CharTemplate() },
        new ISpaceTemplate[] { new DateTimeOffsetTemplate() },
        new ISpaceTemplate[] { new DateTimeTemplate() },
        new ISpaceTemplate[] { new DecimalTemplate() },
        new ISpaceTemplate[] { new DoubleTemplate() },
        new ISpaceTemplate[] { new FloatTemplate() },
        new ISpaceTemplate[] { new GuidTemplate() },
        new ISpaceTemplate[] { new HugeTemplate() },
        new ISpaceTemplate[] { new IntTemplate() },
        new ISpaceTemplate[] { new LongTemplate() },
        new ISpaceTemplate[] { new SByteTemplate() },
        new ISpaceTemplate[] { new ShortTemplate() },
        new ISpaceTemplate[] { new TimeSpanTemplate() },
        new ISpaceTemplate[] { new UHugeTemplate() },
        new ISpaceTemplate[] { new UIntTemplate() },
        new ISpaceTemplate[] { new ULongTemplate() },
        new ISpaceTemplate[] { new UShortTemplate() }
    };

    public IEnumerator<ISpaceTemplate[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class TupleTypeGenerator : IEnumerable<Type[]>
{
    private readonly List<Type[]> data = new()
    {
        new Type[] { typeof(object), typeof(SpaceTuple), typeof(SpaceTemplate) },
        new Type[] { typeof(bool), typeof(BoolTuple), typeof(BoolTemplate) },
        new Type[] { typeof(char), typeof(CharTuple), typeof(CharTemplate) },
        new Type[] { typeof(DateTimeOffset), typeof(DateTimeOffsetTuple), typeof(DateTimeOffsetTemplate) },
        new Type[] { typeof(DateTime), typeof(DateTimeTuple), typeof(DateTimeTemplate) },
        new Type[] { typeof(decimal), typeof(DecimalTuple), typeof(DecimalTemplate) },
        new Type[] { typeof(double), typeof(DoubleTuple), typeof(DoubleTemplate) },
        new Type[] { typeof(float), typeof(FloatTuple), typeof(FloatTemplate) },
        new Type[] { typeof(Guid), typeof(GuidTuple), typeof(GuidTemplate) },
        new Type[] { typeof(Int128), typeof(HugeTuple), typeof(HugeTemplate) },
        new Type[] { typeof(int), typeof(IntTuple), typeof(IntTemplate) },
        new Type[] { typeof(long), typeof(LongTuple), typeof(LongTemplate) },
        new Type[] { typeof(sbyte), typeof(SByteTuple), typeof(SByteTemplate) },
        new Type[] { typeof(short), typeof(ShortTuple), typeof(ShortTemplate) },
        new Type[] { typeof(TimeSpan), typeof(TimeSpanTuple), typeof(TimeSpanTemplate) },
        new Type[] { typeof(UInt128), typeof(UHugeTuple), typeof(UHugeTemplate) },
        new Type[] { typeof(uint), typeof(UIntTuple), typeof(UIntTemplate) },
        new Type[] { typeof(ulong), typeof(ULongTuple), typeof(ULongTemplate) },
        new Type[] { typeof(ushort), typeof(UShortTuple), typeof(UShortTemplate) }
    };

    public IEnumerator<Type[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}