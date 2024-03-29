﻿using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Tuples;

public class SpaceTupleTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        SpaceTuple tuple = new(1, "a", 1.5f, TestEnum.A);

        Assert.Equal(4, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal("a", tuple[1]);
        Assert.Equal(1.5f, tuple[2]);
        Assert.Equal(TestEnum.A, tuple[3]);
    }

    [Fact]
    public void Should_Be_Created_On_Direct_Array_Level_1()
    {
        object[] array = new object[4] { 1, "a", 1.5f, TestEnum.A };
        SpaceTuple tuple = new(array);

        Assert.Equal(4, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal("a", tuple[1]);
        Assert.Equal(1.5f, tuple[2]);
        Assert.Equal(TestEnum.A, tuple[3]);
    }

    [Fact]
    public void Should_Be_Created_On_Direct_Array_Level_2()
    {
        object[] array_l1 = new object[4] { 1, "a", 1.5f, TestEnum.A };
        object[] array_l2 = new object[1] { array_l1 }; 
        SpaceTuple tuple = new(array_l2);

        Assert.Equal(4, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal("a", tuple[1]);
        Assert.Equal(1.5f, tuple[2]);
        Assert.Equal(TestEnum.A, tuple[3]);
    }

    [Fact]
    public void Should_Be_Created_On_Value_Type()
    {
        SpaceTuple tuple = new(1);

        Assert.Equal(1, tuple.Length);
        Assert.Equal(1, tuple[0]);
    }

    [Fact]
    public void Should_Be_Created_On_String()
    {
        SpaceTuple tuple = new("a");

        Assert.Equal(1, tuple.Length);
        Assert.Equal("a", tuple[0]);
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new SpaceTuple(new object[] { null! }));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Types()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTuple(1, typeof(int), "a"));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Class_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTuple(1, "a", new TestClass()));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Struct_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTuple(1, "a", new TestStruct()));
    }

    [Fact]
    public void Should_Not_Throw_On_Empty_String()
    {
        var expection = Record.Exception(() =>
        {
            _ = new SpaceTuple("");
            _ = new SpaceTuple(string.Empty);
        });
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Keyword()
    {
        SpaceTuple tuple = default;
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Create_Empty_Tuple_On_Default_Constructor()
    {
        SpaceTuple tuple = new();
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        SpaceTuple tuple = new(null);
        Assert.Equal(0, tuple.Length);
        Assert.True(tuple.IsEmpty);
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new SpaceTuple());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        SpaceTuple tuple1 = new(1, "a", 1.5f);
        SpaceTuple tuple2 = new(1, "a", 1.5f);

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Default_Constructor()
    {
        SpaceTuple tuple1 = new();
        SpaceTuple tuple2 = new();

        Assert.Equal(tuple1, tuple2);
        Assert.True(tuple1 == tuple2);
        Assert.False(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        SpaceTuple tuple = new(1, "a", 1.5f);
        object obj = new SpaceTuple(1, "a", 1.5f);

        Assert.True(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SpaceTuple tuple = new(1, "a", 1.5f);
        object obj = new SpaceTuple(1, "a");

        Assert.False(tuple.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        SpaceTuple tuple1 = new(1, "a", 1.5f);
        SpaceTuple tuple2 = new(1, "b", 1.5f);

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        SpaceTuple tuple1 = new(1, "a", 1.5f);
        SpaceTuple tuple2 = new(1, "a");

        Assert.NotEqual(tuple1, tuple2);
        Assert.False(tuple1 == tuple2);
        Assert.True(tuple1 != tuple2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("()", new SpaceTuple().ToString());
        Assert.Equal("(1)", new SpaceTuple(1).ToString());
        Assert.Equal("(1, a)", new SpaceTuple(1, "a").ToString());
        Assert.Equal("(1, a, 1.5)", new SpaceTuple(1, "a", 1.5f).ToString());
        Assert.Equal("(1, a, 1.5, b)", new SpaceTuple(1, "a", 1.5f, 'b').ToString());
    }
}

public class SpaceTemplateTests
{
    [Fact]
    public void Should_Be_Created_On_Array()
    {
        SpaceTemplate template = new(1, "a", 1.5f, TestEnum.A);

        Assert.Equal(4, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal("a", template[1]);
        Assert.Equal(1.5f, template[2]);
        Assert.Equal(TestEnum.A, template[3]);
    }

    [Fact]
    public void Should_Be_Created_On_Direct_Array_Level_1()
    {
        object?[] array = new object?[4] { 1, "a", 1.5f, TestEnum.A };
        SpaceTemplate template = new(array);

        Assert.Equal(4, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal("a", template[1]);
        Assert.Equal(1.5f, template[2]);
        Assert.Equal(TestEnum.A, template[3]);
    }

    [Fact]
    public void Should_Be_Created_On_Direct_Array_Level_2()
    {
        object?[] array_l1 = new object?[4] { 1, "a", 1.5f, TestEnum.A };
        object?[] array_l2 = new object?[1] { array_l1 };
        SpaceTemplate template = new(array_l2);

        Assert.Equal(4, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal("a", template[1]);
        Assert.Equal(1.5f, template[2]);
        Assert.Equal(TestEnum.A, template[3]);
    }

    [Fact]
    public void Should_Be_Created_On_Empty_Array()
    {
        SpaceTemplate template = new(Array.Empty<object?>());
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Keyword()
    {
        SpaceTemplate template = default;
        Assert.Equal(0, template.Length);
    }

    [Fact]
    public void Should_Be_Created_On_Value_Type()
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
    public void Should_Convert_From_SpaceTuple()
    {
        SpaceTemplate template1 = new();
        SpaceTemplate explicit1 = new SpaceTuple().ToTemplate();

        SpaceTemplate template2 = new(1);
        SpaceTemplate explicit2 = new SpaceTuple(1).ToTemplate();

        Assert.Equal(template1, explicit1);
        Assert.Equal(template2, explicit2);
    }

    [Fact]
    public void Should_Be_Created_On_Default_Constructor()
    {
        SpaceTemplate template = new();
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Null()
    {
        SpaceTemplate template = new(null);
        Assert.Equal(1, template.Length);
        Assert.Null(template[0]);
    }

    [Fact]
    public void Should_Be_Created_On_Type()
    {
        SpaceTemplate template = new(typeof(int));

        Assert.Equal(1, template.Length);
        Assert.Equal(typeof(int), template[0]);
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Class_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTemplate(1, "a", new TestClass()));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Struct_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTemplate(1, "a", new TestStruct()));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_ValueTuple_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTemplate(1, "a", new ValueTuple<int>(1)));
    }

    [Fact]
    public void Should_Throw_If_Tuple_Contains_Tuple_Type_Field()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTemplate(1, "a", Tuple.Create(1)));
    }

    [Fact]
    public void Should_Not_Throw_On_Default_Constructor()
    {
        var expection = Record.Exception(() => new SpaceTemplate());
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Null()
    {
        var expection = Record.Exception(() => new SpaceTemplate(1, "a", null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_If_Template_Contains_Types()
    {
        var expection = Record.Exception(() => new SpaceTemplate(1, typeof(int), null));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Not_Throw_On_Empty_String()
    {
        var expection = Record.Exception(() =>
        {
            _ = new SpaceTemplate("");
            _ = new SpaceTemplate(string.Empty);
        });
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        SpaceTemplate template1 = new(1, "a", 1.5f, null);
        SpaceTemplate template2 = new(1, "a", 1.5f, null);

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
        SpaceTemplate template = new(1, "a", 1.5f, null);
        object obj = new SpaceTemplate(1, "a", 1.5f, null);

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SpaceTemplate template = new(1, "a", 1.5f);
        object obj = new SpaceTemplate(1, "a", null);

        Assert.False(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Same_Lengths()
    {
        SpaceTemplate template1 = new(1, "a", 1.5f);
        SpaceTemplate template2 = new(1, "b", 1.5f);

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_Not_Be_Equal_For_Different_Lengths()
    {
        SpaceTemplate template1 = new(1, "a", 1.5f);
        SpaceTemplate template2 = new(1, "a");

        Assert.NotEqual(template1, template2);
        Assert.False(template1 == template2);
        Assert.True(template1 != template2);
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("({NULL})", new SpaceTemplate().ToString());
        Assert.Equal("(1)", new SpaceTemplate(1).ToString());
        Assert.Equal("(1, a)", new SpaceTemplate(1, "a").ToString());
        Assert.Equal("(1, a, 1.5)", new SpaceTemplate(1, "a", 1.5f).ToString());
        Assert.Equal("(1, a, 1.5, b)", new SpaceTemplate(1, "a", 1.5f, 'b').ToString());
        Assert.Equal("(1, a, 1.5, b, {NULL})", new SpaceTemplate(1, "a", 1.5f, 'b', null).ToString());
        Assert.Equal("(1, a, 1.5, b, {NULL}, System.Int32)", new SpaceTemplate(1, "a", 1.5f, 'b', null, typeof(int)).ToString());
    }
}

public class SpaceMatchTests
{
    private readonly SpaceTuple tuple;

    public SpaceMatchTests()
    {
        tuple = new(1, "a", 1.5f);
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        SpaceTemplate template = new(1, "a");
        Assert.False(template.Matches(tuple));
    }

    [Fact]
    public void Shoud_Match_Various_Tuples()
    {
        Assert.True(new SpaceTemplate(1).Matches(new(1)));
        Assert.True(new SpaceTemplate(1, "a").Matches(new(1, "a")));
        Assert.True(new SpaceTemplate(1, "a", 1.5f).Matches(new(1, "a", 1.5f)));
        Assert.True(new SpaceTemplate(1, "a", 1.5f, null).Matches(new(1, "a", 1.5f, 1.1m)));
        Assert.True(new SpaceTemplate(1, null, 1.5f, null).Matches(new(1, "a", 1.5f, 1.1m)));
    }

    [Fact]
    public void Shoud_Not_Match_Various_Tuples()
    {
        Assert.False(new SpaceTemplate(1, "a").Matches(new(1)));
        Assert.False(new SpaceTemplate(1, "a", 1.5f).Matches(new(1, "a", 2.5f)));
        Assert.False(new SpaceTemplate(1, "a", 1.5f, null).Matches(new(1, "b", 1.5f, 1.1m)));
        Assert.False(new SpaceTemplate(1, null, 1.5f, null).Matches(new(1, "a", 2.5f, 1.1m)));
    }

    #region Values

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnValues()
    {
        SpaceTemplate template1 = new(2, "a", 1.5f);
        SpaceTemplate template2 = new(1, "b", 1.5f);
        SpaceTemplate template3 = new(1, "a", 1.6f);

        Assert.False(template1.Matches(tuple));
        Assert.False(template2.Matches(tuple));
        Assert.False(template3.Matches(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnValues()
    {
        SpaceTemplate template = tuple.ToTemplate();
        Assert.True(template.Matches(tuple));
    }

    [Fact]
    public void Should_Be_False_If_All_Items_Match_But_Are_Out_Of_Order_OnValues()
    {
        SpaceTemplate template1 = new("a", 1, 1.5f);
        SpaceTemplate template2 = new(1, 1.5f, "a");
        SpaceTemplate template3 = new(1.5f, "a", 1);

        Assert.False(template1.Matches(tuple));
        Assert.False(template2.Matches(tuple));
        Assert.False(template3.Matches(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnValues()
    {
        SpaceTemplate template1 = new(1, "a", null);
        SpaceTemplate template2 = new(1, null, 1.5f);
        SpaceTemplate template3 = new(1, null, null);
        SpaceTemplate template4 = new(null, "a", 1.5f);
        SpaceTemplate template5 = new(null, null, 1.5f);
        SpaceTemplate template6 = new(null, null, null);

        Assert.True(template1.Matches(tuple));
        Assert.True(template2.Matches(tuple));
        Assert.True(template3.Matches(tuple));
        Assert.True(template4.Matches(tuple));
        Assert.True(template5.Matches(tuple));
        Assert.True(template6.Matches(tuple));
    }

    #endregion

    #region Types

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnTypes()
    {
        SpaceTemplate template1 = new(typeof(int), typeof(string), typeof(double));
        SpaceTemplate template2 = new(typeof(int), typeof(double), typeof(float));
        SpaceTemplate template3 = new(typeof(double), typeof(string), typeof(float));

        Assert.False(template1.Matches(tuple));
        Assert.False(template2.Matches(tuple));
        Assert.False(template3.Matches(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnTypes()
    {
        SpaceTemplate template = new(typeof(int), typeof(string), typeof(float));
        Assert.True(template.Matches(tuple));
    }

    [Fact]
    public void Should_Be_False_If_All_Items_Match_But_Are_Out_Of_Order_OnTypes()
    {
        SpaceTemplate template1 = new(typeof(int), typeof(int), typeof(float));
        SpaceTemplate template2 = new(typeof(string), typeof(string), typeof(float));
        SpaceTemplate template3 = new(typeof(string), typeof(string), typeof(float));

        Assert.False(template1.Matches(tuple));
        Assert.False(template2.Matches(tuple));
        Assert.False(template3.Matches(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnTypes()
    {
        SpaceTemplate template1 = new(typeof(int), typeof(string), null);
        SpaceTemplate template2 = new(typeof(int), null, typeof(float));
        SpaceTemplate template3 = new(typeof(int), null, null);
        SpaceTemplate template4 = new(null, typeof(string), typeof(float));
        SpaceTemplate template5 = new(null, null, typeof(float));
        SpaceTemplate template6 = new(null, null, null);

        Assert.True(template1.Matches(tuple));
        Assert.True(template2.Matches(tuple));
        Assert.True(template3.Matches(tuple));
        Assert.True(template4.Matches(tuple));
        Assert.True(template5.Matches(tuple));
        Assert.True(template6.Matches(tuple));
    }

    #endregion

    #region Mixed

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnMixed()
    {
        SpaceTemplate template1 = new(typeof(string), "a", 1.5f);
        SpaceTemplate template2 = new(1, typeof(int), 1.5f);
        SpaceTemplate template3 = new(1, "a", typeof(double));

        Assert.False(template1.Matches(tuple));
        Assert.False(template2.Matches(tuple));
        Assert.False(template3.Matches(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnMixed()
    {
        SpaceTemplate template1 = new(1, typeof(string), 1.5f);
        SpaceTemplate template2 = new(1, typeof(string), typeof(float));
        SpaceTemplate template3 = new(typeof(int), "a", 1.5f);
        SpaceTemplate template4 = new(typeof(int), "a", typeof(float));
        SpaceTemplate template5 = new(typeof(int), typeof(string), 1.5f);
        SpaceTemplate template6 = new(typeof(int), typeof(string), typeof(float));

        Assert.True(template1.Matches(tuple));
        Assert.True(template2.Matches(tuple));
        Assert.True(template3.Matches(tuple));
        Assert.True(template4.Matches(tuple));
        Assert.True(template5.Matches(tuple));
        Assert.True(template6.Matches(tuple));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnMixed()
    {
        SpaceTemplate template1 = new(1, null, typeof(float));
        SpaceTemplate template2 = new(typeof(int), "a", null);
        SpaceTemplate template3 = new(typeof(int), null, null);
        SpaceTemplate template4 = new(null, typeof(string), 1.5f);
        SpaceTemplate template5 = new(null, "a", typeof(float));

        Assert.True(template1.Matches(tuple));
        Assert.True(template2.Matches(tuple));
        Assert.True(template3.Matches(tuple));
        Assert.True(template4.Matches(tuple));
        Assert.True(template5.Matches(tuple));
    }

    #endregion
}
