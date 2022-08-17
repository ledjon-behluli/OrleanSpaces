using OrleanSpaces.Types;

namespace OrleanSpaces.Tests;

public class NullTupleTests
{
    [Fact]
    public void Should_Be_Equal_To_Each_Other()
    {
        var unit1 = NullTuple.Value;
        var unit2 = NullTuple.Value;

        Assert.Equal(unit1, unit2);
        Assert.True(unit1 == unit2);
        Assert.False(unit1 != unit2);
    }

    [Fact]
    public void Should_Be_Equatable()
    {
        var dictionary = new Dictionary<NullTuple, string>
        {
            {new NullTuple(), "value"},
        };

        Assert.Equal("value", dictionary[default]);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("NULL", NullTuple.Value.ToString());
    }

    [Fact]
    public void Should_CompareTo_As_Zero()
    {
        var unit1 = new NullTuple();
        var unit2 = new NullTuple();

        Assert.Equal(0, unit1.CompareTo(unit2));
    }

    [Theory]
    [MemberData(nameof(ValueData))]
    public void Should_be_equal(object value, bool isEqual)
    {
        var unit = NullTuple.Value;

        if (isEqual)
            Assert.True(unit.Equals(value));
        else
            Assert.False(unit.Equals(value));
    }

    [Theory]
    [MemberData(nameof(CompareToValueData))]
    public void Should_CompareTo_Value_As_Zero(object value)
    {
        var unit = new NullTuple();
        var comparable = (IComparable)unit;

        Assert.Equal(0, comparable.CompareTo(value));
    }

    public static object[][] CompareToValueData()
        => ValueData().Select(objects => new[] { objects[0] }).ToArray();

    public static object[][] ValueData() =>
        new[]
        {
            new object[] {new object(), false},
            new object[] {"", false},
            new object[] {"NULL", false},
            new object[] {null, false},
            new object[] {new Uri("https://www.google.com"), false},
            new object[] {new NullTuple(), true},
            new object[] {NullTuple.Value, true},
            new object[] {default(NullTuple), true},
        };
}