namespace OrleanSpaces.Tests;

public class SpaceUnitTests
{
    [Fact]
    public void Should_be_equal_to_each_other()
    {
        var unit1 = SpaceUnit.Null;
        var unit2 = SpaceUnit.Null;

        Assert.Equal(unit1, unit2);
        Assert.True(unit1 == unit2);
        Assert.False(unit1 != unit2);
    }

    [Fact]
    public void Should_be_equitable()
    {
        var dictionary = new Dictionary<SpaceUnit, string>
        {
            {new SpaceUnit(), "value"},
        };

        Assert.Equal("value", dictionary[default]);
    }

    [Fact]
    public void Should_tostring()
    {
        var unit = SpaceUnit.Null;
        Assert.Equal("()", unit.ToString());
    }

    [Fact]
    public void Should_compareto_as_zero()
    {
        var unit1 = new SpaceUnit();
        var unit2 = new SpaceUnit();

        Assert.Equal(0, unit1.CompareTo(unit2));
    }

    public static object[][] ValueData()
    {
        return new[]
        {
            new object[] {new object(), false},
            new object[] {"", false},
            new object[] {"()", false},
            new object[] {null, false},
            new object[] {new Uri("https://www.google.com"), false},
            new object[] {new SpaceUnit(), true},
            new object[] {SpaceUnit.Null, true},
            new object[] {default(SpaceUnit), true},
        };
    }

    public static object[][] CompareToValueData()
        => ValueData().Select(objects => new[] { objects[0] }).ToArray();

    [Theory]
    [MemberData(nameof(ValueData))]
    public void Should_be_equal(object value, bool isEqual)
    {
        var unit1 = SpaceUnit.Null;

        if (isEqual)
            Assert.True(unit1.Equals(value));
        else
            Assert.False(unit1.Equals(value));
    }

    [Theory]
    [MemberData(nameof(CompareToValueData))]
    public void Should_compareto_value_as_zero(object value)
    {
        var unit1 = new SpaceUnit();

        Assert.Equal(0, ((IComparable)unit1).CompareTo(value));
    }
}