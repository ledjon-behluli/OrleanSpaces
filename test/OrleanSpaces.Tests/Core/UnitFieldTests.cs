using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Tests.Core;

public class UnitFieldTests
{
    [Fact]
    public void Should_Be_Equal_To_Each_Other()
    {
        var unit1 = UnitField.Null;
        var unit2 = UnitField.Null;

        Assert.Equal(unit1, unit2);
        Assert.True(unit1 == unit2);
        Assert.False(unit1 != unit2);
    }

    [Fact]
    public void Should_Be_Equatable()
    {
        Dictionary<UnitField, string> dictionary = new()
        {
            { new UnitField(), "value" },
        };

        Assert.Equal("value", dictionary[default]);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("{NULL}", UnitField.Null.ToString());
    }

    [Fact]
    public void Should_CompareTo_As_Zero()
    {
        var unit1 = new UnitField();
        var unit2 = new UnitField();

        Assert.Equal(0, unit1.CompareTo(unit2));
    }

    [Theory]
    [MemberData(nameof(ValueData))]
    public void Should_Be_Equal(object value, bool isEqual)
    {
        var unit = UnitField.Null;

        if (isEqual)
            Assert.True(unit.Equals(value));
        else
            Assert.False(unit.Equals(value));
    }

    [Theory]
    [MemberData(nameof(CompareToValueData))]
    public void Should_CompareTo_Value_As_Zero(object value)
    {
        var unit = new UnitField();
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
            new object[] {new UnitField(), true},
            new object[] {UnitField.Null, true},
            new object[] {default(UnitField), true},
        };
}