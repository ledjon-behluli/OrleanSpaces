using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanBools.Tests.Tuples;

public class GuidTupleTests
{
    private static readonly Guid guid1 = Guid.Parse("347eb792-406e-46cf-93d3-14c67e2d9e08");
    private static readonly Guid guid2 = Guid.Parse("bc34dd3d-5971-498f-82fd-e08a8cf2d165");
    private static readonly Guid guid3 = Guid.Parse("a45c3bd9-ea58-4bf8-9f51-8a9265d58a78");
    private static readonly Guid guid4 = Guid.Parse("e5fd4745-1020-42fa-92eb-9a67df9093eb");

    [Fact]
    public void Should_Be_An_ISpaceTuple()
    {
        Assert.True(typeof(ISpaceTuple<Guid>).IsAssignableFrom(typeof(GuidTuple)));
    }

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        GuidTuple tuple = new(guid1, guid2, guid3);

        Assert.Equal(3, tuple.Length);
        Assert.Equal(guid1, tuple[0]);
        Assert.Equal(guid2, tuple[1]);
        Assert.Equal(guid3, tuple[2]);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        GuidTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new GuidTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        GuidTuple tuple1 = new(guid1, guid2, guid3);
        GuidTuple tuple2 = new(guid1, guid2, guid3);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        GuidTuple tuple1 = new();
        GuidTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        GuidTuple tuple = new(guid1, guid2, guid3);
        object obj = new GuidTuple(guid1, guid2, guid3);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        GuidTuple tuple = new(guid1, guid2, guid3);
        object obj = new GuidTuple(guid1, guid2);

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        GuidTuple tuple1 = new(guid1, guid2, guid3);
        GuidTuple tuple2 = new(guid1, guid2, guid4);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        GuidTuple tuple1 = new(guid1, guid2, guid3);
        GuidTuple tuple2 = new(guid1, guid2);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new GuidTuple().ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08)", new GuidTuple(guid1).ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165)", new GuidTuple(guid1, guid2).ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165, a45c3bd9-ea58-4bf8-9f51-8a9265d58a78)", new GuidTuple(guid1, guid2, guid3).ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165, a45c3bd9-ea58-4bf8-9f51-8a9265d58a78, e5fd4745-1020-42fa-92eb-9a67df9093eb)", new GuidTuple(guid1, guid2, guid3, guid4).ToString());
    }
}

public class GuidTemplateTests
{
    private static readonly Guid guid1 = Guid.Parse("347eb792-406e-46cf-93d3-14c67e2d9e08");
    private static readonly Guid guid2 = Guid.Parse("bc34dd3d-5971-498f-82fd-e08a8cf2d165");
    private static readonly Guid guid3 = Guid.Parse("a45c3bd9-ea58-4bf8-9f51-8a9265d58a78");
    private static readonly Guid guid4 = Guid.Parse("e5fd4745-1020-42fa-92eb-9a67df9093eb");

    [Fact]
    public void Should_Be_An_IGuidTemplate()
    {
        Assert.True(typeof(ISpaceTemplate<Guid>).IsAssignableFrom(typeof(GuidTemplate)));
    }

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        GuidTemplate template = new(guid1, guid2, guid3);

        Assert.Equal(3, template.Length);
        Assert.Equal(guid1, template[0]);
        Assert.Equal(guid2, template[1]);
        Assert.Equal(guid3, template[2]);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        GuidTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        GuidTemplate template = new(null);

        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new GuidTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new GuidTemplate(guid1, guid2, null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        GuidTemplate template1 = new(guid1, guid2, guid3, null);
        GuidTemplate template2 = new(guid1, guid2, guid3, null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        GuidTemplate template1 = new();
        GuidTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        GuidTemplate template = new(guid1, guid2, guid3, null);
        object obj = new GuidTemplate(guid1, guid2, guid3, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        GuidTemplate template = new(guid1, guid2, guid3);
        object obj = new GuidTemplate(guid1, guid2, null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        GuidTemplate template1 = new(guid1, guid2, guid3);
        GuidTemplate template2 = new(guid1, guid2, guid4);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        GuidTemplate template1 = new(guid1, guid2, guid3);
        GuidTemplate template2 = new(guid1, guid2);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new GuidTemplate().ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08)", new GuidTemplate(guid1).ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165)", new GuidTemplate(guid1, guid2).ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165, a45c3bd9-ea58-4bf8-9f51-8a9265d58a78)", new GuidTemplate(guid1, guid2, guid3).ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165, a45c3bd9-ea58-4bf8-9f51-8a9265d58a78, e5fd4745-1020-42fa-92eb-9a67df9093eb)", new GuidTemplate(guid1, guid2, guid3, guid4).ToString());
        Assert.Equal("(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165, a45c3bd9-ea58-4bf8-9f51-8a9265d58a78, e5fd4745-1020-42fa-92eb-9a67df9093eb, {NULL})", new GuidTemplate(guid1, guid2, guid3, guid4, null).ToString());
    }

    [Fact]
    public void Should_Explicitly_Convert_From_GuidTuple()
    {
        GuidTemplate template1 = new();
        GuidTemplate explicit1 = (GuidTemplate)new GuidTuple();

        GuidTemplate template2 = new(guid1);
        GuidTemplate explicit2 = (GuidTemplate)new GuidTuple(guid1);

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Convert_From_GuidTuple()
    {
        GuidTemplate template1 = new();
        ISpaceTemplate<Guid> explicit1 = (new GuidTuple() as ISpaceTuple<Guid>).ToTemplate();

        GuidTemplate template2 = new(guid1);
        ISpaceTemplate<Guid> explicit2 = (new GuidTuple(guid1) as ISpaceTuple<Guid>).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        Guid[] fields = new Guid[3] { guid1, guid2, guid3 };

        GuidTuple tuple1 = new(fields);
        GuidTuple tuple2 = (GuidTuple)Create<GuidTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<Guid> Create<T>(Guid[] fields)
            where T : ISpaceTuple<Guid> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        GuidTuple tuple = new(guid1, guid2, guid3);
        int i = 0;
        
        foreach (ref readonly Guid field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(GuidTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new GuidTuple(guid1), "(347eb792-406e-46cf-93d3-14c67e2d9e08)", 38 },
            new object[] { new GuidTuple(guid1, guid2), "(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165)", 76 },
            new object[] { new GuidTuple(guid1, guid2, guid3), "(347eb792-406e-46cf-93d3-14c67e2d9e08, bc34dd3d-5971-498f-82fd-e08a8cf2d165, a45c3bd9-ea58-4bf8-9f51-8a9265d58a78)", 114 }
       };
}

public class GuidMatchTests
{
    private static readonly Guid guid1 = Guid.Parse("347eb792-406e-46cf-93d3-14c67e2d9e08");
    private static readonly Guid guid2 = Guid.Parse("bc34dd3d-5971-498f-82fd-e08a8cf2d165");
    private static readonly Guid guid3 = Guid.Parse("a45c3bd9-ea58-4bf8-9f51-8a9265d58a78");
    private static readonly Guid guid4 = Guid.Parse("e5fd4745-1020-42fa-92eb-9a67df9093eb");

    private readonly GuidTuple tuple;

    public GuidMatchTests()
    {
        tuple = new(guid1, guid2, guid3);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        GuidTemplate template = new(guid1, guid2);
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new GuidTemplate(guid1).Matches(new GuidTuple(guid1)));
        Assert.True(new GuidTemplate(guid1, guid2).Matches(new GuidTuple(guid1, guid2)));
        Assert.True(new GuidTemplate(guid1, guid2, guid3).Matches(new GuidTuple(guid1, guid2, guid3)));
        Assert.True(new GuidTemplate(guid1, guid2, guid3, null).Matches(new GuidTuple(guid1, guid2, guid3, guid4)));
        Assert.True(new GuidTemplate(guid1, null, guid3, null).Matches(new GuidTuple(guid1, guid2, guid3, guid4)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new GuidTemplate(guid1, guid2).Matches(new GuidTuple(guid1)));
        Assert.False(new GuidTemplate(guid1, guid2, guid3).Matches(new GuidTuple(guid1, guid1, guid3)));
        Assert.False(new GuidTemplate(guid1, guid2, guid3, null).Matches(new GuidTuple(guid1, guid1, guid3, guid4)));
    }
}
