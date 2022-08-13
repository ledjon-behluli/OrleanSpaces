namespace OrleanSpaces;

public class NullUnitTests
{
    [Fact]
    public void Should_be_equal_to_each_other()
    {
        var unit1 = NullUnit.Value;
        var unit2 = NullUnit.Value;

        Assert.Equal(unit1, unit2);
        Assert.True(unit1 == unit2);
        Assert.False(unit1 != unit2);
    }

    [Fact]
    public void Should_be_equitable()
    {
        var dictionary = new Dictionary<NullUnit, string>
        {
            {new NullUnit(), "value"},
        };

        Assert.Equal("value", dictionary[default]);
    }

    [Fact]
    public void Should_tostring()
    {
        var unit = NullUnit.Value;
        Assert.Equal("()", unit.ToString());
    }

    [Fact]
    public void Should_compareto_as_zero()
    {
        var unit1 = new NullUnit();
        var unit2 = new NullUnit();

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
            new object[] {new NullUnit(), true},
            new object[] {NullUnit.Value, true},
            new object[] {default(NullUnit), true},
        };
    }

    public static object[][] CompareToValueData()
        => ValueData().Select(objects => new[] { objects[0] }).ToArray();

    [Theory]
    [MemberData(nameof(ValueData))]
    public void Should_be_equal(object value, bool isEqual)
    {
        var unit1 = NullUnit.Value;

        if (isEqual)
            Assert.True(unit1.Equals(value));
        else
            Assert.False(unit1.Equals(value));
    }

    [Theory]
    [MemberData(nameof(CompareToValueData))]
    public void Should_compareto_value_as_zero(object value)
    {
        var unit1 = new NullUnit();

        Assert.Equal(0, ((IComparable)unit1).CompareTo(value));
    }
}