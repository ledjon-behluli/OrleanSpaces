using OrleanSpaces.Exceptions;
using OrleanSpaces.Types;

namespace OrleanSpaces.Tests;

public class SpaceTupleTests
{
    [Fact]
    public void SpaceTuple_Should_Be_Created_On_Tuple()
    {
        SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));

        Assert.Equal(3, tuple.Length);

        Assert.Equal(1, tuple[0]);
        Assert.Equal("a", tuple[1]);
        Assert.Equal(1.5f, tuple[2]);
    }

    [Fact]
    public void SpaceTuple_Should_Be_Created_On_Object()
    {
        SpaceTuple tuple = SpaceTuple.Create("a");

        Assert.Equal(1, tuple.Length);
        Assert.Equal("a", tuple[0]);
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Null()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTuple.Create(null));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Empty_ValueTuple()
    {
        Assert.Throws<TupleFieldLengthException>(() => SpaceTuple.Create(new ValueTuple()));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_SpaceUnit()
    {
        Assert.Throws<TupleFieldException>(() => SpaceTuple.Create(NullTuple.Value));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Type()
    {
        Assert.Throws<TupleFieldException>(() => SpaceTuple.Create(typeof(int)));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_If_Tuple_Contains_SpaceUnit()
    {
        Assert.Throws<TupleFieldException>(() => SpaceTuple.Create((1, "a", NullTuple.Value)));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_If_Tuple_Contains_Types()
    {
        Assert.Throws<TupleFieldException>(() => SpaceTuple.Create((1, typeof(int), "a")));
    }
}