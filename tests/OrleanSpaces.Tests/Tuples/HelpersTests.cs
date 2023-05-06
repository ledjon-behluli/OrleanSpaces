using OrleanSpaces.Tuples;
using System.Dynamic;

namespace OrleanSpaces.Tests.Tuples;

public class SimpleTypeCheckingTests
{
    [Theory]
    [MemberData(nameof(SimpleTypes))]
    public void Should_Pass(Type type)
    {
        Assert.True(type.IsSimpleType());
    }

    [Theory]
    [MemberData(nameof(OtherTypes))]
    public void Should_Fail(Type type)
    {
        Assert.False(type.IsSimpleType());
    }

    public static object[][] SimpleTypes() =>
        new[]
        {
            // Primitives
            new object[] { typeof(bool) },
            new object[] { typeof(byte) },
            new object[] { typeof(sbyte) },
            new object[] { typeof(char) },
            new object[] { typeof(double) },
            new object[] { typeof(float) },
            new object[] { typeof(short) },
            new object[] { typeof(ushort) },
            new object[] { typeof(int) },
            new object[] { typeof(uint) },
            new object[] { typeof(long) },
            new object[] { typeof(ulong) },
            // Others
            new object[] { typeof(TestEnum) },
            new object[] { typeof(string) },
            new object[] { typeof(decimal) },
            new object[] { typeof(DateTime) },
            new object[] { typeof(DateTimeOffset) },
            new object[] { typeof(TimeSpan) },
            new object[] { typeof(Guid) }
        };

    public static object[][] OtherTypes() =>
        new[]
        {
            new object[] { typeof(TestStruct) },
            new object[] { typeof(TestClass) },
            new object[] { typeof(object) },
            new object[] { typeof(DynamicObject) },
        };

    private enum TestEnum { }
    private struct TestStruct { }
    private class TestClass { }
}
