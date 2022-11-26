using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Tuples;

public class SpaceUnitTests
{
    [Fact]
    public void Should_Be_An_ITuple()
    {
        Assert.True(typeof(ITuple).IsAssignableFrom(typeof(SpaceUnit)));
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void Should_Be_Equatable(object value, bool isEqual)
    {
        SpaceUnit unit = new();

        if (isEqual)
        {
            Assert.True(unit.Equals(value));
        }
        else
        {
            Assert.False(unit.Equals(value));
        }
    }

    [Fact]
    public void Should_Be_Equatable_By_Operator()
    {
        SpaceUnit unit1 = new();
        SpaceUnit unit2 = new();

        Assert.True(unit1 == unit2);
        Assert.False(unit1 != unit2);
    }

    [Theory]
    [MemberData(nameof(CompareData))]
    public void Should_Be_Compareable(SpaceUnit unit)
    {
        Assert.Equal(0, new SpaceUnit().CompareTo(unit));
    }

    [Fact]
    public void Should_Have_Length_Of_One()
    {
        Assert.Equal(1, ((ITuple)new SpaceUnit()).Length);
    }

    [Fact]
    public void Should_Return_Itself_On_Zero_Index()
    {
        Assert.Equal(new SpaceUnit(), ((ITuple)new SpaceUnit())[0]);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Should_Throw_On_Non_Zero_Index(int index)
    {
        Assert.Throws<IndexOutOfRangeException>(() => ((ITuple)new SpaceUnit())[index]);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("{NULL}", new SpaceUnit().ToString());
    }

    public static object[][] CompareData() =>
        new[]
        {
            new object[] { new SpaceUnit() },
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
            new object[] { new SpaceUnit(), true },
            new object[] { default(SpaceUnit), true },
        };
}