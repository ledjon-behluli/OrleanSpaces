namespace OrleanSpaces.Tests;

public class SpaceTemplateTests
{
    [Fact]
    public void SpaceTemplate_Should_Be_Created_On_Tuple()
    {
        SpaceTemplate tuple = SpaceTemplate.Create((1, "a", 1.5f));

        Assert.Equal(3, tuple.Length);
        Assert.Equal(1, tuple[0]);
        Assert.Equal("a", tuple[1]);
        Assert.Equal(1.5f, tuple[2]);
    }

    [Fact]
    public void SpaceTemplate_Should_Be_Created_On_Single_Object()
    {
        SpaceTemplate tuple = SpaceTemplate.Create("a");

        Assert.Equal(1, tuple.Length);
        Assert.Equal("a", tuple[0]);
    }

    [Fact]
    public void SpaceTemplate_Should_Be_Created_On_Single_SpaceUnit()
    {
        SpaceTemplate tuple = SpaceTemplate.Create(SpaceUnit.Null);

        Assert.Equal(1, tuple.Length);
        Assert.Equal(SpaceUnit.Null, tuple[0]);
    }

    [Fact]
    public void SpaceTemplate_Should_Be_Created_On_Single_Type()
    {
        SpaceTemplate tuple = SpaceTemplate.Create(typeof(int));

        Assert.Equal(1, tuple.Length);
        Assert.Equal(typeof(int), tuple[0]);
    }

    [Fact]
    public void ArgumentNullException_Should_Be_Thrown_On_Null_Object()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTemplate.Create(null));
    }

    [Fact]
    public void OrleanSpacesException_Should_Be_Thrown_On_Empty_ValueTuple()
    {
        Assert.Throws<OrleanSpacesException>(() => SpaceTemplate.Create(new ValueTuple()));
    }

    [Fact]
    public void Exception_Should_Not_Be_Thrown_If_Tuple_Contains_SpaceUnit()
    {
        var expection = Record.Exception(() => SpaceTemplate.Create((1, "a", SpaceUnit.Null)));
        Assert.Null(expection);
    }

    [Fact]
    public void Exception_Should_Not_Be_Thrown_If_Tuple_Contains_Types()
    {
        var expection = Record.Exception(() => SpaceTemplate.Create((1, typeof(int), SpaceUnit.Null)));
        Assert.Null(expection);
    }
}