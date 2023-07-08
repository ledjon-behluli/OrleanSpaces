using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;
using System.Dynamic;

namespace OrleanSpaces.Tests;

public class HelperTests
{
    [Theory]
    [MemberData(nameof(SupportedTypes))]
    public void Should_Be_Of_Supported_Type(Type type)
        => Assert.True(type.IsSupportedType());

    [Theory]
    [MemberData(nameof(OtherTypes))]
    public void Should_Not_Be_Of_Supported_Type(Type type)
        => Assert.False(type.IsSupportedType());

    [Theory]
    [ClassData(typeof(EmptyTupleGenerator))]
    public void Should_Throw_On_Empty_Tuple(ISpaceTuple tuple)
        => Assert.Throws<ArgumentException>(() => ThrowHelpers.EmptyTuple(tuple));

    [Fact]
    public void Should_Throw_On_Invalid_Filed_Type()
        => Assert.Throws<ArgumentException>(() => ThrowHelpers.InvalidFieldType(0));

    [Fact]
    public void Should_Throw_On_Null_Filed_Type()
        => Assert.Throws<ArgumentNullException>(() => ThrowHelpers.NullField(0));
   
    private static object[][] SupportedTypes() =>
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
            new object[] { typeof(Int128) },
            new object[] { typeof(UInt128) },
            new object[] { typeof(DateTime) },
            new object[] { typeof(DateTimeOffset) },
            new object[] { typeof(TimeSpan) },
            new object[] { typeof(Guid) }
        };

    private static object[][] OtherTypes() =>
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
