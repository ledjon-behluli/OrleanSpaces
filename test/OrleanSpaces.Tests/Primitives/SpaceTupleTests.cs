﻿using OrleanSpaces.Primitives;
using System.Diagnostics;

namespace OrleanSpaces.Tests.Primitives;

public class SpaceTupleTests
{
    private struct TestStruct { }
    private class TestClass { }

    [Fact]
    public void Should_Be_Created_On_Struct()
    {
        TestStruct test = new();
        SpaceTuple tuple = SpaceTuple.Create(test);

        Assert.Equal(1, tuple.Length);
        Assert.Equal(test, tuple[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Tuple()
    {
        SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));

        Assert.Equal(3, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal("a", tuple[1]);
        Assert.Equal(1.5f, tuple[2]);
    }

    [Fact]
    public void Should_Be_Created_On_String()
    {
        SpaceTuple tuple = SpaceTuple.Create("a");

        Assert.Equal(1, tuple.Length);
        Assert.Equal("a", tuple[0]);
    }

    [Fact]
    public void Tuple_With_Zero_Length_Should_Be_Created_On_Default_Constructor()
    {
        SpaceTuple tuple = new();

        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
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
    public void Should_Throw_On_UnitField()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create(UnitField.Null));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_UnitField()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, "a", UnitField.Null)));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Types()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, typeof(int), "a")));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Reference_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => SpaceTuple.Create((1, "a", new TestClass())));
    }

    [Fact]
    public void Should_Throw_When_Accessing_Indexer_Of_Empty_Tuple()
    {
        SpaceTuple tuple = new();

        Assert.True(tuple.IsEmpty);
        Assert.Throws<IndexOutOfRangeException>(() => tuple[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new SpaceTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void IsEmpty_Should_Be_True_On_Default_Constructor()
    {
        SpaceTuple tuple = new();
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Length_Should_Be_Zero_On_Default_Constructor()
    {
        SpaceTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Be_A_SpaceElement()
    {
        Assert.True(typeof(ISpaceElement).IsAssignableFrom(typeof(SpaceTuple)));
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