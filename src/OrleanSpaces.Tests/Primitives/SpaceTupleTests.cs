using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Primitives;

public class SpaceTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Tuple()
    {
        SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f, TestEnum.A));

        Assert.Equal(4, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal("a", tuple[1]);
        Assert.Equal(1.5f, tuple[2]);
        Assert.Equal(TestEnum.A, tuple[3]);
    }

    [Fact]
    public void Should_Be_Created_On_ValueType()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);

        Assert.Equal(1, tuple.Length);
        Assert.Equal(1, tuple[0]);
    }

    [Fact]
    public void Should_Be_Created_On_String()
    {
        SpaceTuple tuple = SpaceTuple.Create("a");

        Assert.Equal(1, tuple.Length);
        Assert.Equal("a", tuple[0]);
    }

    [Fact]
    public void Should_Throw_On_Null()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTuple.Create((ValueType)null));
        Assert.Throws<ArgumentNullException>(() => SpaceTuple.Create((string)null));
    }

    [Fact]
    public void Should_Throw_On_Empty_String()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTuple.Create(""));
        Assert.Throws<ArgumentNullException>(() => SpaceTuple.Create(string.Empty));
    }

    [Fact]
    public void Should_Throw_On_SpaceUnit()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create(SpaceUnit.Null));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_SpaceUnit()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, "a", SpaceUnit.Null)));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Types()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, typeof(int), "a")));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Class_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, "a", new TestClass())));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Struct_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, "a", new TestStruct())));
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new SpaceTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Considered_As_Empty_On_Default_Constructor()
    {
        SpaceTuple tuple = new();
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Have_Length_Of_One_On_Default_Constructor()
    {
        SpaceTuple tuple = new();
        Assert.Equal(1, tuple.Length);
    }

    [Fact]
    public void Should_Be_Assignable_From_Tuple()
    {
        Assert.True(typeof(ITuple).IsAssignableFrom(typeof(SpaceTuple)));
    }

    [Fact]
    public void Should_Be_Equal()
    {
        SpaceTuple tuple1 = SpaceTuple.Create((1, "a", 1.5f));
        SpaceTuple tuple2 = SpaceTuple.Create((1, "a", 1.5f));

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));
        object obj = SpaceTuple.Create((1, "a", 1.5f));

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));
        object obj = SpaceTuple.Create((1, "a"));

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        SpaceTuple tuple1 = SpaceTuple.Create((1, "a", 1.5f));
        SpaceTuple tuple2 = SpaceTuple.Create((1, "b", 1.5f));

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        SpaceTuple tuple1 = SpaceTuple.Create((1, "a", 1.5f));
        SpaceTuple tuple2 = SpaceTuple.Create((1, "a"));

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new SpaceTuple().ToString());
        Assert.Equal("(1)", SpaceTuple.Create(1).ToString());
        Assert.Equal("(1, a)", SpaceTuple.Create((1, "a")).ToString());
        Assert.Equal("(1, a, 1.5)", SpaceTuple.Create((1, "a", 1.5f)).ToString());
        Assert.Equal("(1, a, 1.5, b)", SpaceTuple.Create((1, "a", 1.5f, 'b')).ToString());
    }
}