using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class BoolTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        BoolTuple tuple = new(true, false, true);

        Assert.Equal(3, tuple.Length);
        Assert.True(tuple[0]);
        Assert.False(tuple[1]);
        Assert.True(tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        BoolTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new BoolTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        BoolTuple tuple1 = new(true, false, true);
        BoolTuple tuple2 = new(true, false, true);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        BoolTuple tuple1 = new();
        BoolTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        BoolTuple tuple = new(true, false, true);
        object obj = new BoolTuple(true, false, true);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        BoolTuple tuple = new(true, false, true);
        object obj = new BoolTuple(true, false);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        BoolTuple tuple1 = new(true, false, true);
        BoolTuple tuple2 = new(true, true, true);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        BoolTuple tuple1 = new(true, false, true);
        BoolTuple tuple2 = new(true, false);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new BoolTuple().ToString());
        Assert.Equal("(True)", new BoolTuple(true).ToString());
        Assert.Equal("(True, False)", new BoolTuple(true, false).ToString());
        Assert.Equal("(True, False, True)", new BoolTuple(true, false, true).ToString());
        Assert.Equal("(True, False, True, False)", new BoolTuple(true, false, true, false).ToString());
    }
}

public class BoolTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        BoolTemplate template = new(true, false, true);

        Assert.Equal(3, template.Length);
        Assert.True(template[0]);
        Assert.False(template[1]);
        Assert.True(template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        BoolTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        BoolTemplate template = new(null);

        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new BoolTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new BoolTemplate(true, false, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        BoolTemplate template1 = new(true, false, true, null);
        BoolTemplate template2 = new(true, false, true, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        BoolTemplate template1 = new();
        BoolTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        BoolTemplate template = new(true, false, true, null);
        object obj = new BoolTemplate(true, false, true, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        BoolTemplate template = new(true, false, true);
        object obj = new BoolTemplate(true, false, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        BoolTemplate template1 = new(true, false, true);
        BoolTemplate template2 = new(true, true, true);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        BoolTemplate template1 = new(true, false, true);
        BoolTemplate template2 = new(true, false);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new BoolTemplate().ToString());
        Assert.Equal("(True)", new BoolTemplate(true).ToString());
        Assert.Equal("(True, False)", new BoolTemplate(true, false).ToString());
        Assert.Equal("(True, False, True)", new BoolTemplate(true, false, true).ToString());
        Assert.Equal("(True, False, True, False)", new BoolTemplate(true, false, true, false).ToString());
        Assert.Equal("(True, False, True, False, {NULL})", new BoolTemplate(true, false, true, false, null).ToString());
    }

    [Fact]
    public void Should_Convert_From_BoolTuple()
    {
        BoolTemplate template1 = new();
        BoolTemplate explicit1 = new BoolTuple().ToTemplate();

        BoolTemplate template2 = new(true);
        BoolTemplate explicit2 = new BoolTuple(true).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        bool[] fields = new bool[3] { true, false, true };

        BoolTuple tuple1 = new(fields);
        BoolTuple tuple2 = (BoolTuple)Create<BoolTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<bool> Create<T>(bool[] fields)
            where T : ISpaceFactory<bool, BoolTuple> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        BoolTuple tuple = new(true, false, true);
        int i = 0;
        
        foreach (ref readonly bool field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(BoolTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new BoolTuple(false), "(False)", 7 },
            new object[] { new BoolTuple(true), "(True)", 6 },
            new object[] { new BoolTuple(true, false), "(True, False)", 13 },
            new object[] { new BoolTuple(true, false, true), "(True, False, True)", 19 }
       };
}

public class BoolMatchTests
{
    private readonly BoolTuple tuple;

    public BoolMatchTests()
    {
        tuple = new(true, false, true);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        BoolTemplate template = new(true, false);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new BoolTemplate(true).Matches(new BoolTuple(true)));
        Assert.True(new BoolTemplate(false).Matches(new BoolTuple(false)));
        Assert.True(new BoolTemplate(true, false).Matches(new BoolTuple(true, false)));
        Assert.True(new BoolTemplate(true, false, true).Matches(new BoolTuple(true, false, true)));
        Assert.True(new BoolTemplate(true, false, true, null).Matches(new BoolTuple(true, false, true, false)));
        Assert.True(new BoolTemplate(true, null, false, null).Matches(new BoolTuple(true, false, false, true)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new BoolTemplate(true, false).Matches(new BoolTuple(true)));
        Assert.False(new BoolTemplate(true, false, true).Matches(new BoolTuple(true, false, false)));
        Assert.False(new BoolTemplate(true, false, true, null).Matches(new BoolTuple(true, false, false, true)));
    }
}
