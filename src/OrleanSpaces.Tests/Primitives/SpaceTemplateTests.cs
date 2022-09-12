using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Primitives;

public class SpaceTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Tuple()
    {
        SpaceTemplate template = new((1, "a", 1.5f));

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal("a", template[1]);
        Assert.Equal(1.5f, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_ValueType()
    {
        SpaceTemplate template = new(1);

        Assert.Equal(1, template.Length);
        Assert.Equal(1, template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_String()
    {
        SpaceTemplate template = new("a");

        Assert.Equal(1, template.Length);
        Assert.Equal("a", template[0]);
    }

    [Fact]
    public void Should_Implicitly_Convert_From_SpaceTuple()
    {
        SpaceTemplate template1 = new SpaceTemplate();
        SpaceTemplate implicitTemplate1 = new SpaceTuple();

        SpaceTemplate template2 = new(1);
        SpaceTemplate implicitTemplate2 = new(1);

        Assert.Equal(template1, implicitTemplate1);
        Assert.Equal(template2, implicitTemplate2);
    }

    [Fact]
    public void Should_Have_Length_Of_One_On_Default_Constructor()
    {
        SpaceTemplate tuple = new();
        Assert.Equal(1, tuple.Length);
    }

    [Fact]
    public void Should_Be_Created_On_SpaceUnit()
    {
        SpaceTemplate template = new(SpaceUnit.Null);

        Assert.Equal(1, template.Length);
        Assert.Equal(SpaceUnit.Null, template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Type()
    {
        SpaceTemplate template = new(typeof(int));

        Assert.Equal(1, template.Length);
        Assert.Equal(typeof(int), template[0]);
    }

    [Fact]
    public void Should_Throw_On_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new SpaceTemplate((string)null));
        Assert.Throws<ArgumentNullException>(() => new SpaceTemplate((Type)null));
        Assert.Throws<ArgumentNullException>(() => new SpaceTemplate((ValueType)null));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Class_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTemplate((1, "a", new TestClass())));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Struct_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTemplate((1, "a", new TestStruct())));
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new SpaceTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_On_Empty_ValueTuple()
    {
        var expection = Record.Exception(() => new SpaceTemplate(new ValueTuple()));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_SpaceUnit()
    {
        var expection = Record.Exception(() => new SpaceTemplate((1, "a", SpaceUnit.Null)));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Types()
    {
        var expection = Record.Exception(() => new SpaceTemplate((1, typeof(int), SpaceUnit.Null)));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_On_Empty_String()
    {
        var expection = Record.Exception(() =>
        {
            new SpaceTemplate("");
            new SpaceTemplate(string.Empty);
        });
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Assignable_From_Tuple()
    {
        Assert.True(typeof(ITuple).IsAssignableFrom(typeof(SpaceTemplate)));
    }

    [Fact]
    public void Should_Be_Equal()
    {
        SpaceTemplate template1 = new((1, "a", 1.5f, SpaceUnit.Null));
        SpaceTemplate template2 = new((1, "a", 1.5f, SpaceUnit.Null));

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        SpaceTemplate template1 = new();
        SpaceTemplate template2 = new();

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        SpaceTemplate template = new((1, "a", 1.5f, SpaceUnit.Null));
        object obj = new SpaceTemplate((1, "a", 1.5f, SpaceUnit.Null));

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SpaceTemplate template = new((1, "a", 1.5f));
        object obj = new SpaceTemplate((1, "a", SpaceUnit.Null));

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        SpaceTemplate template1 = new((1, "a", 1.5f));
        SpaceTemplate template2 = new((1, "b", 1.5f));

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        SpaceTemplate template1 = new((1, "a", 1.5f));
        SpaceTemplate template2 = new((1, "a"));

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal($"({SpaceUnit.Null})", new SpaceTemplate().ToString());
        Assert.Equal("(1)", new SpaceTemplate(1).ToString());
        Assert.Equal("(1, a)", new SpaceTemplate((1, "a")).ToString());
        Assert.Equal("(1, a, 1.5)", new SpaceTemplate((1, "a", 1.5f)).ToString());
        Assert.Equal("(1, a, 1.5, b)", new SpaceTemplate((1, "a", 1.5f, 'b')).ToString());
        Assert.Equal("(1, a, 1.5, b, {NULL})", new SpaceTemplate((1, "a", 1.5f, 'b', SpaceUnit.Null)).ToString());
        Assert.Equal("(1, a, 1.5, b, {NULL}, System.Int32)", new SpaceTemplate((1, "a", 1.5f, 'b', SpaceUnit.Null, typeof(int))).ToString());
    }
}

public class StatisfactionTests
{
    private readonly SpaceTuple tuple;

    public StatisfactionTests()
    {
        tuple = new((1, "a", 1.5f));
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        SpaceTemplate template = new((1, "a"));
        Assert.False(template.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Shoud_Be_Statisfied_By_Various_Tuples()
    {
        Assert.True(new SpaceTemplate(1).IsSatisfiedBy(new(1)));
        Assert.True(new SpaceTemplate((1, "a")).IsSatisfiedBy(new((1, "a"))));
        Assert.True(new SpaceTemplate((1, "a", 1.5f)).IsSatisfiedBy(new((1, "a", 1.5f))));
        Assert.True(new SpaceTemplate((1, "a", 1.5f, SpaceUnit.Null)).IsSatisfiedBy(new((1, "a", 1.5f, 1.1m))));
        Assert.True(new SpaceTemplate((1, SpaceUnit.Null, 1.5f, SpaceUnit.Null)).IsSatisfiedBy(new((1, "a", 1.5f, 1.1m))));
    }

    [Fact]
    public void Shoud_Not_Be_Statisfied_By_Various_Tuples()
    {
        Assert.False(new SpaceTemplate((1, "a")).IsSatisfiedBy(new(1)));
        Assert.False(new SpaceTemplate((1, "a", 1.5f)).IsSatisfiedBy(new((1, "a", 2.5f))));
        Assert.False(new SpaceTemplate((1, "a", 1.5f, SpaceUnit.Null)).IsSatisfiedBy(new((1, "b", 1.5f, 1.1m))));
        Assert.False(new SpaceTemplate((1, SpaceUnit.Null, 1.5f, SpaceUnit.Null)).IsSatisfiedBy(new((1, "a", 2.5f, 1.1m))));
    }

    #region Values

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnValues()
    {
        SpaceTemplate template1 = new((2, "a", 1.5f));
        SpaceTemplate template2 = new((1, "b", 1.5f));
        SpaceTemplate template3 = new((1, "a", 1.6f));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnValues()
    {
        SpaceTemplate template = tuple;
        Assert.True(template.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_False_If_All_Items_Match_But_Are_Out_Of_Order_OnValues()
    {
        SpaceTemplate template1 = new(("a", 1, 1.5f));
        SpaceTemplate template2 = new((1, 1.5f, "a"));
        SpaceTemplate template3 = new((1.5f, "a", 1));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnValues()
    {
        SpaceTemplate template1 = new((1, "a", SpaceUnit.Null));
        SpaceTemplate template2 = new((1, SpaceUnit.Null, 1.5f));
        SpaceTemplate template3 = new((1, SpaceUnit.Null, SpaceUnit.Null));
        SpaceTemplate template4 = new((SpaceUnit.Null, "a", 1.5f));
        SpaceTemplate template5 = new((SpaceUnit.Null, SpaceUnit.Null, 1.5f));
        SpaceTemplate template6 = new((SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null));

        Assert.True(template1.IsSatisfiedBy(tuple));
        Assert.True(template2.IsSatisfiedBy(tuple));
        Assert.True(template3.IsSatisfiedBy(tuple));
        Assert.True(template4.IsSatisfiedBy(tuple));
        Assert.True(template5.IsSatisfiedBy(tuple));
        Assert.True(template6.IsSatisfiedBy(tuple));
    }

    #endregion

    #region Types

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnTypes()
    {
        SpaceTemplate template1 = new((typeof(int), typeof(string), typeof(double)));
        SpaceTemplate template2 = new((typeof(int), typeof(double), typeof(float)));
        SpaceTemplate template3 = new((typeof(double), typeof(string), typeof(float)));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnTypes()
    {
        SpaceTemplate template = new((typeof(int), typeof(string), typeof(float)));
        Assert.True(template.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_False_If_All_Items_Match_But_Are_Out_Of_Order_OnTypes()
    {
        SpaceTemplate template1 = new((typeof(int), typeof(int), typeof(float)));
        SpaceTemplate template2 = new((typeof(string), typeof(string), typeof(float)));
        SpaceTemplate template3 = new((typeof(string), typeof(string), typeof(float)));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnTypes()
    {
        SpaceTemplate template1 = new((typeof(int), typeof(string), SpaceUnit.Null));
        SpaceTemplate template2 = new((typeof(int), SpaceUnit.Null, typeof(float)));
        SpaceTemplate template3 = new((typeof(int), SpaceUnit.Null, SpaceUnit.Null));
        SpaceTemplate template4 = new((SpaceUnit.Null, typeof(string), typeof(float)));
        SpaceTemplate template5 = new((SpaceUnit.Null, SpaceUnit.Null, typeof(float)));
        SpaceTemplate template6 = new((SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null));

        Assert.True(template1.IsSatisfiedBy(tuple));
        Assert.True(template2.IsSatisfiedBy(tuple));
        Assert.True(template3.IsSatisfiedBy(tuple));
        Assert.True(template4.IsSatisfiedBy(tuple));
        Assert.True(template5.IsSatisfiedBy(tuple));
        Assert.True(template6.IsSatisfiedBy(tuple));
    }

    #endregion

    #region Mixed

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnMixed()
    {
        SpaceTemplate template1 = new((typeof(string), "a", 1.5f));
        SpaceTemplate template2 = new((1, typeof(int), 1.5f));
        SpaceTemplate template3 = new((1, "a", typeof(double)));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnMixed()
    {
        SpaceTemplate template1 = new((1, typeof(string), 1.5f));
        SpaceTemplate template2 = new((1, typeof(string), typeof(float)));
        SpaceTemplate template3 = new((typeof(int), "a", 1.5f));
        SpaceTemplate template4 = new((typeof(int), "a", typeof(float)));
        SpaceTemplate template5 = new((typeof(int), typeof(string), 1.5f));
        SpaceTemplate template6 = new((typeof(int), typeof(string), typeof(float)));

        Assert.True(template1.IsSatisfiedBy(tuple));
        Assert.True(template2.IsSatisfiedBy(tuple));
        Assert.True(template3.IsSatisfiedBy(tuple));
        Assert.True(template4.IsSatisfiedBy(tuple));
        Assert.True(template5.IsSatisfiedBy(tuple));
        Assert.True(template6.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnMixed()
    {
        SpaceTemplate template1 = new((1, SpaceUnit.Null, typeof(float)));
        SpaceTemplate template2 = new((typeof(int), "a", SpaceUnit.Null));
        SpaceTemplate template3 = new((typeof(int), SpaceUnit.Null, SpaceUnit.Null));
        SpaceTemplate template4 = new((SpaceUnit.Null, typeof(string), 1.5f));
        SpaceTemplate template5 = new((SpaceUnit.Null, "a", typeof(float)));

        Assert.True(template1.IsSatisfiedBy(tuple));
        Assert.True(template2.IsSatisfiedBy(tuple));
        Assert.True(template3.IsSatisfiedBy(tuple));
        Assert.True(template4.IsSatisfiedBy(tuple));
        Assert.True(template5.IsSatisfiedBy(tuple));
    }

    #endregion
}
