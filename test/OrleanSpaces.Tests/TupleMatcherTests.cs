using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Tests;

public class TupleMatcherTests
{
    private readonly SpaceTuple tuple;

    public TupleMatcherTests()
    {
        tuple = SpaceTuple.Create((1, "a", 1.5f));
    }

    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        SpaceTemplate template = SpaceTemplate.Create((1, "a"));
        Assert.False(TupleMatcher.IsMatch(tuple, template));
    }

    #region Values

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnValues()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((2, "a", 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, "b", 1.5f));
        SpaceTemplate template3 = SpaceTemplate.Create((1, "a", 1.6f));

        Assert.False(TupleMatcher.IsMatch(tuple, template1));
        Assert.False(TupleMatcher.IsMatch(tuple, template2));
        Assert.False(TupleMatcher.IsMatch(tuple, template3));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnValues()
    {
        SpaceTemplate template = SpaceTemplate.Create(tuple);
        Assert.True(TupleMatcher.IsMatch(tuple, template));
    }

    [Fact]
    public void Should_Be_False_If_All_Items_Match_But_Are_Out_Of_Order_OnValues()
    {
        SpaceTemplate template1 = SpaceTemplate.Create(("a", 1, 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, 1.5f, "a"));
        SpaceTemplate template3 = SpaceTemplate.Create((1.5f, "a", 1));

        Assert.False(TupleMatcher.IsMatch(tuple, template1));
        Assert.False(TupleMatcher.IsMatch(tuple, template2));
        Assert.False(TupleMatcher.IsMatch(tuple, template3));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnValues()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((1, "a", NullTuple.Value));
        SpaceTemplate template2 = SpaceTemplate.Create((1, NullTuple.Value, 1.5f));
        SpaceTemplate template3 = SpaceTemplate.Create((1, NullTuple.Value, NullTuple.Value));
        SpaceTemplate template4 = SpaceTemplate.Create((NullTuple.Value, "a", 1.5f));
        SpaceTemplate template5 = SpaceTemplate.Create((NullTuple.Value, NullTuple.Value, 1.5f));
        SpaceTemplate template6 = SpaceTemplate.Create((NullTuple.Value, NullTuple.Value, NullTuple.Value));

        Assert.True(TupleMatcher.IsMatch(tuple, template1));
        Assert.True(TupleMatcher.IsMatch(tuple, template2));
        Assert.True(TupleMatcher.IsMatch(tuple, template3));
        Assert.True(TupleMatcher.IsMatch(tuple, template4));
        Assert.True(TupleMatcher.IsMatch(tuple, template5));
        Assert.True(TupleMatcher.IsMatch(tuple, template6));
    }

    #endregion

    #region Types

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnTypes()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((typeof(int), typeof(string), typeof(double)));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(int), typeof(double), typeof(float)));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(double), typeof(string), typeof(float)));

        Assert.False(TupleMatcher.IsMatch(tuple, template1));
        Assert.False(TupleMatcher.IsMatch(tuple, template2));
        Assert.False(TupleMatcher.IsMatch(tuple, template3));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_OnTypes()
    {
        SpaceTemplate template = SpaceTemplate.Create((typeof(int), typeof(string), typeof(float)));
        Assert.True(TupleMatcher.IsMatch(tuple, template));
    }

    [Fact]
    public void Should_Be_False_If_All_Items_Match_But_Are_Out_Of_Order_OnTypes()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((typeof(int), typeof(int), typeof(float)));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(string), typeof(string), typeof(float)));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(string), typeof(string), typeof(float)));

        Assert.False(TupleMatcher.IsMatch(tuple, template1));
        Assert.False(TupleMatcher.IsMatch(tuple, template2));
        Assert.False(TupleMatcher.IsMatch(tuple, template3));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnTypes()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((typeof(int), typeof(string), NullTuple.Value));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(int), NullTuple.Value, typeof(float)));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(int), NullTuple.Value, NullTuple.Value));
        SpaceTemplate template4 = SpaceTemplate.Create((NullTuple.Value, typeof(string), typeof(float)));
        SpaceTemplate template5 = SpaceTemplate.Create((NullTuple.Value, NullTuple.Value, typeof(float)));
        SpaceTemplate template6 = SpaceTemplate.Create((NullTuple.Value, NullTuple.Value, NullTuple.Value));

        Assert.True(TupleMatcher.IsMatch(tuple, template1));
        Assert.True(TupleMatcher.IsMatch(tuple, template2));
        Assert.True(TupleMatcher.IsMatch(tuple, template3));
        Assert.True(TupleMatcher.IsMatch(tuple, template4));
        Assert.True(TupleMatcher.IsMatch(tuple, template5));
        Assert.True(TupleMatcher.IsMatch(tuple, template6));
    }

    #endregion

    #region Mixed

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_OnMixed()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((typeof(string), "a", 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, typeof(int), 1.5f));
        SpaceTemplate template3 = SpaceTemplate.Create((1, "a", typeof(double)));

        Assert.False(TupleMatcher.IsMatch(tuple, template1));
        Assert.False(TupleMatcher.IsMatch(tuple, template2));
        Assert.False(TupleMatcher.IsMatch(tuple, template3));
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

        Assert.True(TupleMatcher.IsMatch(tuple, template1));
        Assert.True(TupleMatcher.IsMatch(tuple, template2));
        Assert.True(TupleMatcher.IsMatch(tuple, template3));
        Assert.True(TupleMatcher.IsMatch(tuple, template4));
        Assert.True(TupleMatcher.IsMatch(tuple, template5));
        Assert.True(TupleMatcher.IsMatch(tuple, template6));
    }

    [Fact]
    public void Should_Be_True_If_All_Items_Match_But_Some_Are_Null_OnMixed()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((1, NullTuple.Value, typeof(float)));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(int), "a", NullTuple.Value));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(int), NullTuple.Value, NullTuple.Value));
        SpaceTemplate template4 = SpaceTemplate.Create((NullTuple.Value, typeof(string), 1.5f));
        SpaceTemplate template5 = SpaceTemplate.Create((NullTuple.Value, "a", typeof(float)));

        Assert.True(TupleMatcher.IsMatch(tuple, template1));
        Assert.True(TupleMatcher.IsMatch(tuple, template2));
        Assert.True(TupleMatcher.IsMatch(tuple, template3));
        Assert.True(TupleMatcher.IsMatch(tuple, template4));
        Assert.True(TupleMatcher.IsMatch(tuple, template5));
    }

    #endregion
}
