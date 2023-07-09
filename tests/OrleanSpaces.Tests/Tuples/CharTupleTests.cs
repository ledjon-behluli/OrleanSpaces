using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Tuples;

namespace OrleanBools.Tests.Tuples;

public class CharTupleTests
{
    [Fact]
    public void Should_Be_An_ISpaceTuple()
    {
        Assert.True(typeof(ISpaceTuple<char>).IsAssignableFrom(typeof(CharTuple)));
    }

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        CharTuple tuple = new('a', 'b', 'c');

        Assert.Equal(3, tuple.Length);
        Assert.Equal('a', tuple[0]);
        Assert.Equal('b', tuple['a']);
        Assert.Equal('c', tuple['b']);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        CharTuple tuple = new();
        Assert.Equal(0, tuple.Length);
    }


    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new CharTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        CharTuple tuple1 = new('a', 'b', 'c');
        CharTuple tuple2 = new('a', 'b', 'c');

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        CharTuple tuple1 = new();
        CharTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        CharTuple tuple = new('a', 'b', 'c');
        object obj = new CharTuple('a', 'b', 'c');

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        CharTuple tuple = new('a', 'b', 'c');
        object obj = new CharTuple('a', 'b');

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        CharTuple tuple1 = new('a', 'b', 'c');
        CharTuple tuple2 = new('a', 'b', 'd');

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        CharTuple tuple1 = new('a', 'b', 'c');
        CharTuple tuple2 = new('a', 'b');

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new CharTuple().ToString());
        Assert.Equal("(a)", new CharTuple('a').ToString());
        Assert.Equal("(a, b)", new CharTuple('a', 'b').ToString());
        Assert.Equal("(a, b, c)", new CharTuple('a', 'b', 'c').ToString());
        Assert.Equal("(a, b, c, d)", new CharTuple('a', 'b', 'c', 'd').ToString());
    }
}

public class CharTemplateTests
{
    [Fact]
    public void Should_Be_An_ICharTemplate()
    {
        Assert.True(typeof(ISpaceTemplate<char>).IsAssignableFrom(typeof(CharTemplate)));
    }

    [Fact]
    public void Should_Be_Created_On_Object_Array()
    {
        CharTemplate template = new('a', 'b', 'c');

        Assert.Equal(3, template.Length);
        Assert.Equal('a', template[0]);
        Assert.Equal('b', template['a']);
        Assert.Equal('c', template['b']);
    }

    [Fact]
    public void Should_Create_Empty_Template_On_Default_Constructor()
    {
        CharTemplate tuple = new();
        Assert.Equal(0, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        CharTemplate template = new(null);

        Assert.Equal('a', template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new CharTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new CharTemplate('a', 'b', null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        CharTemplate template1 = new('a', 'b', 'c', null);
        CharTemplate template2 = new('a', 'b', 'c', null);

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        CharTemplate template1 = new();
        CharTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        CharTemplate template = new('a', 'b', 'c', null);
        object obj = new CharTemplate('a', 'b', 'c', null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        CharTemplate template = new('a', 'b', 'c');
        object obj = new CharTemplate('a', 'b', null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        CharTemplate template1 = new('a', 'b', 'c');
        CharTemplate template2 = new('a', 'b', 'd');

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        CharTemplate template1 = new('a', 'b', 'c');
        CharTemplate template2 = new('a', 'b');

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new CharTemplate().ToString());
        Assert.Equal("(a)", new CharTemplate('a').ToString());
        Assert.Equal("(a, b)", new CharTemplate('a', 'b').ToString());
        Assert.Equal("(a, b, c)", new CharTemplate('a', 'b', 'c').ToString());
        Assert.Equal("(a, b, c, d)", new CharTemplate('a', 'b', 'c', 'd').ToString());
        Assert.Equal("(a, b, c, d, {NULL})", new CharTemplate('a', 'b', 'c', 'd', null).ToString());
    }

    [Fact]
    public void Should_Explicitly_Convert_From_CharTuple()
    {
        CharTemplate template1 = new();
        CharTemplate explicit1 = (CharTemplate)new CharTuple();

        CharTemplate template2 = new('a');
        CharTemplate explicit2 = (CharTemplate)new CharTuple('a');

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Convert_From_CharTuple()
    {
        CharTemplate template1 = new();
        ISpaceTemplate<char> explicit1 = (new CharTuple() as ISpaceTuple<char>).ToTemplate();

        CharTemplate template2 = new('a');
        ISpaceTemplate<char> explicit2 = (new CharTuple('a') as ISpaceTuple<char>).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Create_On_Static_Method()
    {
        char[] fields = new char[3] { 'a', 'b', 'c' };

        CharTuple tuple1 = new(fields);
        CharTuple tuple2 = (CharTuple)Create<CharTuple>(fields);

        Assert.Equal(tuple1, tuple2);

        static ISpaceTuple<char> Create<T>(char[] fields)
            where T : ISpaceTuple<char> => T.Create(fields);
    }

    [Fact]
    public void Should_Enumerate()
    {
        CharTuple tuple = new('a', 'b', 'c');
        int i = 0;
        
        foreach (ref readonly char field in tuple)
        {
            Assert.Equal(field, tuple[i]);
            i++;
        }
    }

    [Theory]
    [MemberData(nameof(SpanData))]
    public void Should_Convert_To_Span(CharTuple tuple, string spanString, int spanLength)
    {
        ReadOnlySpan<char> span = tuple.AsSpan();

        Assert.Equal(spanLength, span.Length);
        Assert.Equal(spanString, span.ToString());
    }

    private static object[][] SpanData() =>
       new[]
       {
            new object[] { new CharTuple('a'), "(a)", 3 },
            new object[] { new CharTuple('a', 'b'), "(a, b)", 6 },
            new object[] { new CharTuple('a', 'b', 'c'), "(a, b, c)", 9 }
       };
}

public class CharMatchTests
{
    private readonly CharTuple tuple;

    public CharMatchTests()
    {
        tuple = new('a', 'b', 'c');
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        CharTemplate template = new('a', 'b');
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new CharTemplate('a').Matches(new CharTuple('a')));
        Assert.True(new CharTemplate('a', 'b').Matches(new CharTuple('a', 'b')));
        Assert.True(new CharTemplate('a', 'b', 'c').Matches(new CharTuple('a', 'b', 'c')));
        Assert.True(new CharTemplate('a', 'b', 'c', null).Matches(new CharTuple('a', 'b', 'c', 'd')));
        Assert.True(new CharTemplate('a', null, 'c', null).Matches(new CharTuple('a', 'b', 'c', 'd')));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new CharTemplate('a', 'b').Matches(new CharTuple('a')));
        Assert.False(new CharTemplate('a', 'b', 'c').Matches(new CharTuple('a', 'a', 'c')));
        Assert.False(new CharTemplate('a', 'b', 'c', null).Matches(new CharTuple('a', 'a', 'c', 'd')));
    }
}
