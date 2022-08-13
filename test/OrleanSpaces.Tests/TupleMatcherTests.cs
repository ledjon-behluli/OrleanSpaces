namespace OrleanSpaces.Tests;

public class TupleMatcherTests
{
    [Fact]
    public void Should_Be_False_If_Lengths_Are_Not_Equal()
    {
        SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));
        SpaceTemplate template = SpaceTemplate.Create((1, "a"));

        Assert.False(TupleMatcher.IsMatch(tuple, template));
    }

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_ValuesOnlyCase()
    {
        SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));

        SpaceTemplate template1 = SpaceTemplate.Create((2, "a", 1.5f));
        SpaceTemplate template2 = SpaceTemplate.Create((1, "b", 1.5f));
        SpaceTemplate template3 = SpaceTemplate.Create((1, "a", 1.6f));

        Assert.False(TupleMatcher.IsMatch(tuple, template1));
        Assert.False(TupleMatcher.IsMatch(tuple, template2));
        Assert.False(TupleMatcher.IsMatch(tuple, template3));
    }

    [Fact]
    public void Should_Be_False_If_At_Least_One_Item_Doesnt_Match_TypesOnlyCase()
    {
        SpaceTuple tuple = SpaceTuple.Create((1, "a", 1.5f));

        SpaceTemplate template1 = SpaceTemplate.Create((typeof(int), typeof(string), typeof(double)));
        SpaceTemplate template2 = SpaceTemplate.Create((typeof(int), typeof(double), typeof(float)));
        SpaceTemplate template3 = SpaceTemplate.Create((typeof(double), typeof(string), typeof(float)));

        Assert.False(TupleMatcher.IsMatch(tuple, template1));
        Assert.False(TupleMatcher.IsMatch(tuple, template2));
        Assert.False(TupleMatcher.IsMatch(tuple, template3));
    }
}
