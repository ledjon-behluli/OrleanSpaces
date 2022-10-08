using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Tuples;

public class SpaceTupleTests
{
    [Fact]
    public void Should_Be_An_ITuple()
    {
        Assert.True(typeof(ITuple).IsAssignableFrom(typeof(SpaceTuple)));
    }

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        SpaceTuple tuple = new(1, "a", 1.5f, TestEnum.A);

        Assert.Equal(4, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal("a", tuple[1]);
        Assert.Equal(1.5f, tuple[2]);
        Assert.Equal(TestEnum.A, tuple[3]);
    }

    [Fact]
    public void Should_Be_Created_On_Value_Type()
    {
        SpaceTuple tuple = new(1);

        Assert.Equal(1, tuple.Length);
        Assert.Equal(1, tuple[0]);
    }

    [Fact]
    public void Should_Be_Created_On_String()
    {
        SpaceTuple tuple = new("a");

        Assert.Equal(1, tuple.Length);
        Assert.Equal("a", tuple[0]);
    }

    [Fact]
    public void Should_Create_Null_Tuple_On_Null()
    {
        Assert.Equal(SpaceTuple.Null, new SpaceTuple(null));
    }

    [Fact]
    public void Should_Throw_On_SpaceUnit()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTuple(SpaceUnit.Null));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_SpaceUnit()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTuple(1, "a", SpaceUnit.Null));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Types()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTuple(1, typeof(int), "a"));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Class_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTuple(1, "a", new TestClass()));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Struct_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTuple(1, "a", new TestStruct()));
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new SpaceTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_On_Empty_String()
    {
        var expection = Record.Exception(() =>
        {
            new SpaceTuple("");
            new SpaceTuple(string.Empty);
        });
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Considered_As_Null_On_Default_Constructor()
    {
        SpaceTuple tuple = new();
        Assert.True(tuple.IsNull);
    }

    [Fact]
    public void Should_Have_Length_Of_One_On_Default_Constructor()
    {
        SpaceTuple tuple = new();
        Assert.Equal(1, tuple.Length);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        SpaceTuple tuple1 = new(1, "a", 1.5f);
        SpaceTuple tuple2 = new(1, "a", 1.5f);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        SpaceTuple tuple1 = new();
        SpaceTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        SpaceTuple tuple = new(1, "a", 1.5f);
        object obj = new SpaceTuple(1, "a", 1.5f);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SpaceTuple tuple = new(1, "a", 1.5f);
        object obj = new SpaceTuple(1, "a");

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        SpaceTuple tuple1 = new(1, "a", 1.5f);
        SpaceTuple tuple2 = new(1, "b", 1.5f);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        SpaceTuple tuple1 = new(1, "a", 1.5f);
        SpaceTuple tuple2 = new(1, "a");

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Theory]
    [MemberData(nameof(CompareData))]
    public void Should_Be_Compareable(SpaceTuple value, int compareValue)
    {
        SpaceTuple tuple = new("a", "b");
        Assert.Equal(compareValue, tuple.CompareTo(value));
    }

    [Fact]
    public void Should_Sort_By_Length_Asc()
    {
        List<SpaceTuple> actual = new()
        {
            new(1, 1),
            new(1),
            new(1, 1, 1, 1),
            new(1, 1, 1)
        };
        List<SpaceTuple> expected = new()
        {
            new(1),
            new(1, 1),
            new(1, 1, 1),
            new(1, 1, 1, 1)
        };

        actual.Sort();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal($"({SpaceUnit.Null})", new SpaceTuple().ToString());
        Assert.Equal("(1)", new SpaceTuple(1).ToString());
        Assert.Equal("(1, a)", new SpaceTuple(1, "a").ToString());
        Assert.Equal("(1, a, 1.5)", new SpaceTuple(1, "a", 1.5f).ToString());
        Assert.Equal("(1, a, 1.5, b)", new SpaceTuple(1, "a", 1.5f, 'b').ToString());
    }

    public static object[][] CompareData() =>
        new[]
        {
            new object[] { new SpaceTuple(1), 1 },
            new object[] { new SpaceTuple(2), 1 },
            new object[] { new SpaceTuple(1, "a"), 0 },
            new object[] { new SpaceTuple("a", 1), 0 },
            new object[] { new SpaceTuple(1, "a", 1.5f), -1 },
            new object[] { new SpaceTuple("a", 1, 1.8f), -1 },
        };
}