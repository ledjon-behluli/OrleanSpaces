using OrleanSpaces.Tuples;

namespace AnalyzersOffenders;

public class OSA003
{
    int intVar = 1;
    TestClass classVar = new();
    TestStruct structVar = new();
    TestEnum enumVar = TestEnum.A;

    public OSA003()
    {
        SpaceTuple tuple = new(
            1, 'a', "a",
            true, false,
            (byte)1, (sbyte)1, (short)1, (ushort)1, (int)1, (uint)1, (long)1, (ulong)1, (float)1, (decimal)1, 
            typeof(string), (double)1, typeof(int),
            DateTime.MinValue, DateTimeOffset.MinValue, TimeSpan.MinValue, Guid.Empty,
            new SpaceUnit(), new TestClass(), TestEnum.A, new TestStruct(),
            intVar, classVar, structVar, enumVar);

        SpaceTemplate template = new(
            1, 'a', "a",
            true, false,
            (byte)1, (sbyte)1, (short)1, (ushort)1, (int)1, (uint)1, (long)1, (ulong)1, (float)1, (decimal)1, 
            typeof(string), (double)1, typeof(int),
            DateTime.MinValue, DateTimeOffset.MinValue, TimeSpan.MinValue, Guid.Empty,
            new SpaceUnit(), new TestClass(), TestEnum.A, new TestStruct(),
            intVar, classVar, structVar, enumVar);
    }

    class TestClass { }
    struct TestStruct { }
    enum TestEnum { A }
}
