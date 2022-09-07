using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Primitives;

public class SpaceUnitTests
{
    [Fact]
    public void Should_Be_Equal_To_Each_Other()
    {
        var unit1 = SpaceUnit.Null;
        var unit2 = SpaceUnit.Null;

        Assert.Equal(unit1, unit2);
        Assert.True(unit1 == unit2);
        Assert.False(unit1 != unit2);
    }

    [Fact]
    public void Should_Be_Equatable()
    {
        Dictionary<SpaceUnit, string> dictionary = new()
        {
            { new SpaceUnit(), "value" },
        };

        Assert.Equal("value", dictionary[default]);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("{NULL}", SpaceUnit.Null.ToString());
    }

    [Fact]
    public void Should_CompareTo_As_Zero()
    {
        var unit1 = new SpaceUnit();
        var unit2 = new SpaceUnit();

        Assert.Equal(0, unit1.CompareTo(unit2));
    }

    [Theory]
    [MemberData(nameof(InlineData))]
    public void Should_Be_Equal(object value, bool isEqual)
    {
        var unit = SpaceUnit.Null;

        if (isEqual)
            Assert.True(unit.Equals(value));
        else
            Assert.False(unit.Equals(value));
    }

    [Theory]
    [MemberData(nameof(CompareToData))]
    public void Should_CompareTo_Value_As_Zero(object value)
    {
        var unit = new SpaceUnit();
        var comparable = (IComparable)unit;

        Assert.Equal(0, comparable.CompareTo(value));
    }

    public static object[][] CompareToData()
        => InlineData().Select(objects => new[] { objects[0] }).ToArray();

    public static object[][] InlineData() =>
        new[]
        {
            new object[] {new object(), false},
            new object[] {"", false},
            new object[] {"NULL", false},
            new object[] {null, false},
            new object[] {new Uri("https://www.google.com"), false},
            new object[] {new SpaceUnit(), true},
            new object[] {SpaceUnit.Null, true},
            new object[] {default(SpaceUnit), true},
        };
}