﻿using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class FloatTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        FloatTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal(2, tuple[1]);
        Assert.Equal(3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Keyword()
    {
        FloatTuple tuple = default;
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        FloatTuple tuple = new();
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        FloatTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new FloatTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        FloatTuple tuple1 = new(1, 2, 3);
        FloatTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        FloatTuple tuple1 = new();
        FloatTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        FloatTuple tuple = new(1, 2, 3);
        object obj = new FloatTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        FloatTuple tuple = new(1, 2, 3);
        object obj = new FloatTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        FloatTuple tuple1 = new(1, 2, 3);
        FloatTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        FloatTuple tuple1 = new(1, 2, 3);
        FloatTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new FloatTuple().ToString());
        Assert.Equal("(1)", new FloatTuple(1).ToString());
        Assert.Equal("(1, 2)", new FloatTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new FloatTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new FloatTuple(1, 2, 3, 4).ToString());
    }
}

public class FloatTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        FloatTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal(2, template[1]);
        Assert.Equal(3, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Keyword()
    {
        FloatTemplate template = default;
        Assert.Equal(0, template.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        FloatTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        FloatTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new FloatTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new FloatTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        FloatTemplate template1 = new(1, 2, 3, null);
        FloatTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        FloatTemplate template1 = new();
        FloatTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        FloatTemplate template = new(1, 2, 3, null);
        object obj = new FloatTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        FloatTemplate template = new(1, 2, 3);
        object obj = new FloatTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        FloatTemplate template1 = new(1, 2, 3);
        FloatTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        FloatTemplate template1 = new(1, 2, 3);
        FloatTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new FloatTemplate().ToString());
        Assert.Equal("(1)", new FloatTemplate(1).ToString());
        Assert.Equal("(1, 2)", new FloatTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new FloatTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new FloatTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new FloatTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_FloatTuple()
    {
        FloatTemplate template1 = new();
        ISpaceTemplate<float> explicit1 = new FloatTuple().ToTemplate();

        FloatTemplate template2 = new(1);
        ISpaceTemplate<float> explicit2 = new FloatTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        float[] fields = new float[3] { 1, 2, 3 };

        FloatTuple tuple1 = new(fields);
        FloatTuple tuple2 = (FloatTuple)Create<FloatTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<float> Create<T>(float[] fields)
            where T : ISpaceFactory<float, FloatTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        FloatTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly float field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(FloatTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new FloatTuple(1), "(1)", 3 },
            new object[] { new FloatTuple(1, 2), "(1, 2)", 6 },
            new object[] { new FloatTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class FloatMatchTests
{
    private readonly FloatTuple tuple;

    public FloatMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        FloatTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new FloatTemplate(1).Matches(new FloatTuple(1)));
        Assert.True(new FloatTemplate(1, 2).Matches(new FloatTuple(1, 2)));
        Assert.True(new FloatTemplate(1, 2, 3).Matches(new FloatTuple(1, 2, 3)));
        Assert.True(new FloatTemplate(1, 2, 3, null).Matches(new FloatTuple(1, 2, 3, 4)));
        Assert.True(new FloatTemplate(1, null, 3, null).Matches(new FloatTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new FloatTemplate(1, 2).Matches(new FloatTuple(1)));
        Assert.False(new FloatTemplate(1, 2, 3).Matches(new FloatTuple(1, 1, 3)));
        Assert.False(new FloatTemplate(1, 2, 3, null).Matches(new FloatTuple(1, 1, 3, 4)));
    }
}
