using OrleanSpaces.Core.Primitives;
using System.Diagnostics;

namespace OrleanSpaces.Tests.Core;

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
    public void Exception_Should_Be_Thrown_On_Null_Object()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTuple.Create((object)null));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Empty_ValueTuple()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create(new ValueTuple()));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_UnitField()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create(UnitField.Null));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Type()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create(typeof(int)));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_If_Tuple_Contains_UnitField()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, "a", UnitField.Null)));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_If_Tuple_Contains_Types()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, typeof(int), "a")));
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
    public void Should_Not_Be_Equal_If_First_Is_Null()
    {
        SpaceTuple tuple1 = null;
        SpaceTuple tuple2 = SpaceTuple.Create(1);

        Assert.True(tuple1 != tuple2);
        Assert.False(tuple1 == tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_If_Second_Is_Null()
    {
        SpaceTuple tuple1 = SpaceTuple.Create(1);
        SpaceTuple tuple2 = null;

        Assert.True(tuple1 != tuple2);
        Assert.False(tuple1 == tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_If_Both_Are_Null()
    {
        SpaceTuple tuple1 = null;
        SpaceTuple tuple2 = null;

        Assert.True(tuple1 != tuple2);
        Assert.False(tuple1 == tuple2);
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
    public void Should_Be_Faster_For_Different_Lengths()
    {
        long firstRun = Run(SpaceTuple.Create((1, "a")), SpaceTuple.Create((1, "b")));
        long secondRun = Run(SpaceTuple.Create((1, "a")), SpaceTuple.Create(1));

        Assert.True(firstRun > secondRun);

        static long Run(SpaceTuple tuple1, SpaceTuple tuple2)
        {
            Stopwatch watch = new();

            watch.Start();
            _ = tuple1 == tuple2;
            watch.Stop();

            return watch.ElapsedTicks;
        }
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("<1>", SpaceTuple.Create(1).ToString());
        Assert.Equal("<1, a>", SpaceTuple.Create((1, "a")).ToString());
        Assert.Equal("<1, a, 1.5>", SpaceTuple.Create((1, "a", 1.5f)).ToString());
        Assert.Equal("<1, a, 1.5, b>", SpaceTuple.Create((1, "a", 1.5f, 'b')).ToString());
    }
}