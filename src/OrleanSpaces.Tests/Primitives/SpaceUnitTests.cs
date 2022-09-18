using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OrleanSpaces.Tests.Primitives;

public class SpaceUnitTests
{
    [Fact]
    public void Should_Be_Smaller_Or_Equal_To_IntPtr_Size()
    {
        Assert.True(Marshal.SizeOf(typeof(SpaceUnit)) <= IntPtr.Size);
    }

    [Fact]
    public void Should_Be_An_ITuple()
    {
        Assert.True(typeof(ITuple).IsAssignableFrom(typeof(SpaceUnit)));
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void Should_Be_Equatable(object value, bool isEqual)
    {
        var unit = SpaceUnit.Null;

        if (isEqual)
            Assert.True(unit.Equals(value));
        else
            Assert.False(unit.Equals(value));
    }

    [Theory]
    [MemberData(nameof(CompareData))]
    public void Should_Be_Compareable(SpaceUnit unit)
    {
        Assert.Equal(0, SpaceUnit.Null.CompareTo(unit));
    }

    [Fact]
    public void Should_Have_Length_Of_One()
    {
        Assert.Equal(1, ((ITuple)SpaceUnit.Null).Length);
    }

    [Fact]
    public void Should_Return_Itself_On_Zero_Index()
    {
        Assert.Equal(SpaceUnit.Null, ((ITuple)SpaceUnit.Null)[0]);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Should_Throw_On_Non_Zero_Index(int index)
    {
        Assert.Throws<IndexOutOfRangeException>(() => ((ITuple)SpaceUnit.Null)[index]);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("{NULL}", SpaceUnit.Null.ToString());
    }

    public static object[][] CompareData() =>
        new[]
        {
            new object[] { SpaceUnit.Null },
            new object[] { new SpaceUnit() },
            new object[] { default(SpaceUnit) },
        };

    public static object[][] Data() =>
        new[]
        {
            new object[] { new object(), false },
            new object[] { "", false },
            new object[] { "NULL", false },
            new object[] { null, false },
            new object[] { new Uri("https://www.google.com"), false },
            new object[] { new SpaceUnit(), true },
            new object[] { SpaceUnit.Null, true },
            new object[] { default(SpaceUnit), true },
        };
}