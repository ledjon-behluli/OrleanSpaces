using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanBools.Tests.Tuples;

public class UIntTupleTests
{
    [Fact]
    public void Should_Be_An_ISpaceTuple()
    {
        Assert.True(typeof(ISpaceTuple<uint>).IsAssignableFrom(typeof(UIntTuple)));
    }

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        UIntTuple tuple = new(1, 2, 3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal((uint)1, tuple[0]);
        Assert.Equal((uint)2, tuple[1]);
        Assert.Equal((uint)3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        UIntTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new UIntTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        UIntTuple tuple1 = new(1, 2, 3);
        UIntTuple tuple2 = new(1, 2, 3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        UIntTuple tuple1 = new();
        UIntTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        UIntTuple tuple = new(1, 2, 3);
        object obj = new UIntTuple(1, 2, 3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        UIntTuple tuple = new(1, 2, 3);
        object obj = new UIntTuple(1, 2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        UIntTuple tuple1 = new(1, 2, 3);
        UIntTuple tuple2 = new(1, 2, 4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        UIntTuple tuple1 = new(1, 2, 3);
        UIntTuple tuple2 = new(1, 2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new UIntTuple().ToString());
        Assert.Equal("(1)", new UIntTuple(1).ToString());
        Assert.Equal("(1, 2)", new UIntTuple(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new UIntTuple(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new UIntTuple(1, 2, 3, 4).ToString());
    }
}

public class UIntTemplateTests
{
    [Fact]
    public void Should_Be_An_IUIntTemplate()
    {
        Assert.True(typeof(ISpaceTemplate<uint>).IsAssignableFrom(typeof(UIntTemplate)));
    }

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        UIntTemplate template = new(1, 2, 3);

        Assert.Equal(3, template.Length);
        Assert.Equal((uint)1, template[0]);
        Assert.Equal((uint)2, template[1]);
        Assert.Equal((uint)3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        UIntTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        UIntTemplate template = new(null);

        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new UIntTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new UIntTemplate(1, 2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        UIntTemplate template1 = new(1, 2, 3, null);
        UIntTemplate template2 = new(1, 2, 3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        UIntTemplate template1 = new();
        UIntTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        UIntTemplate template = new(1, 2, 3, null);
        object obj = new UIntTemplate(1, 2, 3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        UIntTemplate template = new(1, 2, 3);
        object obj = new UIntTemplate(1, 2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        UIntTemplate template1 = new(1, 2, 3);
        UIntTemplate template2 = new(1, 2, 4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        UIntTemplate template1 = new(1, 2, 3);
        UIntTemplate template2 = new(1, 2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new UIntTemplate().ToString());
        Assert.Equal("(1)", new UIntTemplate(1).ToString());
        Assert.Equal("(1, 2)", new UIntTemplate(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new UIntTemplate(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new UIntTemplate(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, {NULL})", new UIntTemplate(1, 2, 3, 4, null).ToString());
    }

    [Fact]
    public void Should_Explicitly_Convert_From_UIntTuple()
    {
        UIntTemplate template1 = new();
        UIntTemplate explicit1 = (UIntTemplate)new UIntTuple();

        UIntTemplate template2 = new(1);
        UIntTemplate explicit2 = (UIntTemplate)new UIntTuple(1);

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Convert_From_UIntTuple()
    {
        UIntTemplate template1 = new();
        ISpaceTemplate<uint> explicit1 = (new UIntTuple() as ISpaceTuple<uint>).ToTemplate();

        UIntTemplate template2 = new(1);
        ISpaceTemplate<uint> explicit2 = (new UIntTuple(1) as ISpaceTuple<uint>).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        uint[] fields = new uint[3] { 1, 2, 3 };

        UIntTuple tuple1 = new(fields);
        UIntTuple tuple2 = (UIntTuple)Create<UIntTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<uint> Create<T>(uint[] fields)
            where T : ISpaceTuple<uint> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        UIntTuple tuple = new(1, 2, 3);
        int i = 0;
        
        foreach (ref readonly uint field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(UIntTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new UIntTuple(1), "(1)", 3 },
            new object[] { new UIntTuple(1, 2), "(1, 2)", 6 },
            new object[] { new UIntTuple(1, 2, 10), "(1, 2, 10)", 10 }
       };
}

public class UIntMatchTests
{
    private readonly UIntTuple tuple;

    public UIntMatchTests()
    {
        tuple = new(1, 2, 3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        UIntTemplate template = new(1, 2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new UIntTemplate(1).Matches(new UIntTuple(1)));
        Assert.True(new UIntTemplate(1, 2).Matches(new UIntTuple(1, 2)));
        Assert.True(new UIntTemplate(1, 2, 3).Matches(new UIntTuple(1, 2, 3)));
        Assert.True(new UIntTemplate(1, 2, 3, null).Matches(new UIntTuple(1, 2, 3, 4)));
        Assert.True(new UIntTemplate(1, null, 3, null).Matches(new UIntTuple(1, 2, 3, 4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new UIntTemplate(1, 2).Matches(new UIntTuple(1)));
        Assert.False(new UIntTemplate(1, 2, 3).Matches(new UIntTuple(1, 1, 3)));
        Assert.False(new UIntTemplate(1, 2, 3, null).Matches(new UIntTuple(1, 1, 3, 4)));
    }
}
