using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Primitives;

public class SpaceTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Tuple()
    {
        SpaceTemplate template = SpaceTemplate.Create((1, "a", 1.5f));

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal("a", template[1]);
        Assert.Equal(1.5f, template[2]);
    }

    [Fact]
    public void Should_Be_Created_On_ValueType()
    {
        SpaceTemplate template = SpaceTemplate.Create(1);

        Assert.Equal(1, template.Length);
        Assert.Equal(1, template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_String()
    {
        SpaceTemplate template = SpaceTemplate.Create("a");

        Assert.Equal(1, template.Length);
        Assert.Equal("a", template[0]);
    }

    [Fact]
    public void Should_Implicitly_Convert_From_SpaceTuple()
    {
        SpaceTemplate normalTemplate = SpaceTemplate.Create(1);
        SpaceTemplate implicitTemplate = SpaceTuple.Create(1);

        Assert.Equal(normalTemplate, implicitTemplate);
    }

    [Fact]
    public void Should_Be_Created_On_SpaceUnit()
    {
        SpaceTemplate template = SpaceTemplate.Create(SpaceUnit.Null);

        Assert.Equal(1, template.Length);
        Assert.Equal(SpaceUnit.Null, template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Type()
    {
        SpaceTemplate template = SpaceTemplate.Create(typeof(int));

        Assert.Equal(1, template.Length);
        Assert.Equal(typeof(int), template[0]);
    }

    [Fact]
    public void Should_Throw_On_Null()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTemplate.Create((Type)null));
        Assert.Throws<ArgumentNullException>(() => SpaceTemplate.Create((ValueType)null));
    }

    [Fact]
    public void Should_Throw_On_Empty_String()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTemplate.Create(""));
        Assert.Throws<ArgumentNullException>(() => SpaceTemplate.Create(string.Empty));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Class_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => SpaceTemplate.Create((1, "a", new TestClass())));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Struct_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => SpaceTemplate.Create((1, "a", new TestStruct())));
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
        var expection = Record.Exception(() => SpaceTemplate.Create(new ValueTuple()));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_SpaceUnit()
    {
        var expection = Record.Exception(() => SpaceTemplate.Create((1, "a", SpaceUnit.Null)));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Types()
    {
        var expection = Record.Exception(() => SpaceTemplate.Create((1, typeof(int), SpaceUnit.Null)));
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
        SpaceTemplate template1 = SpaceTemplate.Create((1, "a", 1.5f, SpaceUnit.Null));
        SpaceTemplate template2 = SpaceTemplate.Create((1, "a", 1.5f, SpaceUnit.Null));

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        SpaceTemplate template = SpaceTemplate.Create((1, "a", 1.5f, SpaceUnit.Null));
        object obj = SpaceTemplate.Create((1, "a", 1.5f, SpaceUnit.Null));

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SpaceTemplate template = SpaceTemplate.Create((1, "a", 1.5f));
        object obj = SpaceTemplate.Create((1, "a", SpaceUnit.Null));

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((1, "a", 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, "b", 1.5f));

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((1, "a", 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, "a"));

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal($"({SpaceUnit.Null})", new SpaceTemplate().ToString());
        Assert.Equal("(1)", SpaceTemplate.Create(1).ToString());
        Assert.Equal("(1, a)", SpaceTemplate.Create((1, "a")).ToString());
        Assert.Equal("(1, a, 1.5)", SpaceTemplate.Create((1, "a", 1.5f)).ToString());
        Assert.Equal("(1, a, 1.5, b)", SpaceTemplate.Create((1, "a", 1.5f, 'b')).ToString());
        Assert.Equal("(1, a, 1.5, b, {NULL})", SpaceTemplate.Create((1, "a", 1.5f, 'b', SpaceUnit.Null)).ToString());
        Assert.Equal("(1, a, 1.5, b, {NULL}, System.Int32)", SpaceTemplate.Create((1, "a", 1.5f, 'b', SpaceUnit.Null, typeof(int))).ToString());
    }
}

public class SpaceTemplate_TupleStatisfactionTests
{
    private readonly SpaceTuple tuple;

    public SpaceTemplate_TupleStatisfactionTests()
    {
        tuple = SpaceTuple.Create((1, "a", 1.5f));
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        SpaceTemplate template = SpaceTemplate.Create((1, "a"));
        Assert.False(template.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Shoud_Be_Statisfied_By_Various_Tuples()
    {
        Assert.True(SpaceTemplate.Create(1).IsSatisfiedBy(SpaceTuple.Create(1)));
        Assert.True(SpaceTemplate.Create((1, "a")).IsSatisfiedBy(SpaceTuple.Create((1, "a"))));
        Assert.True(SpaceTemplate.Create((1, "a", 1.5f)).IsSatisfiedBy(SpaceTuple.Create((1, "a", 1.5f))));
        Assert.True(SpaceTemplate.Create((1, "a", 1.5f, SpaceUnit.Null)).IsSatisfiedBy(SpaceTuple.Create((1, "a", 1.5f, 1.1m))));
        Assert.True(SpaceTemplate.Create((1, SpaceUnit.Null, 1.5f, SpaceUnit.Null)).IsSatisfiedBy(SpaceTuple.Create((1, "a", 1.5f, 1.1m))));
    }

    [Fact]
    public void Shoud_Not_Be_Statisfied_By_Various_Tuples()
    {
        Assert.False(SpaceTemplate.Create((1, "a")).IsSatisfiedBy(SpaceTuple.Create(1)));
        Assert.False(SpaceTemplate.Create((1, "a", 1.5f)).IsSatisfiedBy(SpaceTuple.Create((1, "a", 2.5f))));
        Assert.False(SpaceTemplate.Create((1, "a", 1.5f, SpaceUnit.Null)).IsSatisfiedBy(SpaceTuple.Create((1, "b", 1.5f, 1.1m))));
        Assert.False(SpaceTemplate.Create((1, SpaceUnit.Null, 1.5f, SpaceUnit.Null)).IsSatisfiedBy(SpaceTuple.Create((1, "a", 2.5f, 1.1m))));
    }

    #region Values

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnValues()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((2, "a", 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, "b", 1.5f));
        SpaceTemplate template3 = SpaceTemplate.Create((1, "a", 1.6f));

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
        SpaceTemplate template1 = SpaceTemplate.Create(("a", 1, 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, 1.5f, "a"));
        SpaceTemplate template3 = SpaceTemplate.Create((1.5f, "a", 1));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnValues()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((1, "a", SpaceUnit.Null));
        SpaceTemplate template2 = SpaceTemplate.Create((1, SpaceUnit.Null, 1.5f));
        SpaceTemplate template3 = SpaceTemplate.Create((1, SpaceUnit.Null, SpaceUnit.Null));
        SpaceTemplate template4 = SpaceTemplate.Create((SpaceUnit.Null, "a", 1.5f));
        SpaceTemplate template5 = SpaceTemplate.Create((SpaceUnit.Null, SpaceUnit.Null, 1.5f));
        SpaceTemplate template6 = SpaceTemplate.Create((SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null));

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
        SpaceTemplate template1 = SpaceTemplate.Create((typeof(int), typeof(string), typeof(double)));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(int), typeof(double), typeof(float)));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(double), typeof(string), typeof(float)));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnTypes()
    {
        SpaceTemplate template = SpaceTemplate.Create((typeof(int), typeof(string), typeof(float)));
        Assert.True(template.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_False_If_All_Items_Match_But_Are_Out_Of_Order_OnTypes()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((typeof(int), typeof(int), typeof(float)));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(string), typeof(string), typeof(float)));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(string), typeof(string), typeof(float)));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnTypes()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((typeof(int), typeof(string), SpaceUnit.Null));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(int), SpaceUnit.Null, typeof(float)));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(int), SpaceUnit.Null, SpaceUnit.Null));
        SpaceTemplate template4 = SpaceTemplate.Create((SpaceUnit.Null, typeof(string), typeof(float)));
        SpaceTemplate template5 = SpaceTemplate.Create((SpaceUnit.Null, SpaceUnit.Null, typeof(float)));
        SpaceTemplate template6 = SpaceTemplate.Create((SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null));

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
        SpaceTemplate template1 = SpaceTemplate.Create((typeof(string), "a", 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, typeof(int), 1.5f));
        SpaceTemplate template3 = SpaceTemplate.Create((1, "a", typeof(double)));

        Assert.False(template1.IsSatisfiedBy(tuple));
        Assert.False(template2.IsSatisfiedBy(tuple));
        Assert.False(template3.IsSatisfiedBy(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnMixed()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((1, typeof(string), 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, typeof(string), typeof(float)));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(int), "a", 1.5f));
        SpaceTemplate template4 = SpaceTemplate.Create((typeof(int), "a", typeof(float)));
        SpaceTemplate template5 = SpaceTemplate.Create((typeof(int), typeof(string), 1.5f));
        SpaceTemplate template6 = SpaceTemplate.Create((typeof(int), typeof(string), typeof(float)));

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
        SpaceTemplate template1 = SpaceTemplate.Create((1, SpaceUnit.Null, typeof(float)));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(int), "a", SpaceUnit.Null));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(int), SpaceUnit.Null, SpaceUnit.Null));
        SpaceTemplate template4 = SpaceTemplate.Create((SpaceUnit.Null, typeof(string), 1.5f));
        SpaceTemplate template5 = SpaceTemplate.Create((SpaceUnit.Null, "a", typeof(float)));

        Assert.True(template1.IsSatisfiedBy(tuple));
        Assert.True(template2.IsSatisfiedBy(tuple));
        Assert.True(template3.IsSatisfiedBy(tuple));
        Assert.True(template4.IsSatisfiedBy(tuple));
        Assert.True(template5.IsSatisfiedBy(tuple));
    }

    #endregion
}
