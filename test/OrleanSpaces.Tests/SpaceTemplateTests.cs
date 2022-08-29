using OrleanSpaces.Primitives;
using System.Diagnostics;

namespace OrleanSpaces.Tests;

public class SpaceTemplateTests
{
    [Fact]
    public void SpaceTemplate_Should_Be_Created_On_Tuple()
    {
        SpaceTemplate template = SpaceTemplate.Create((1, "a", 1.5f));

        Assert.Equal(3, template.Length);
        Assert.Equal(1, template[0]);
        Assert.Equal("a", template[1]);
        Assert.Equal(1.5f, template[2]);
    }

    [Fact]
    public void SpaceTemplate_Should_Be_Created_On_Object()
    {
        SpaceTemplate template = SpaceTemplate.Create("a");

        Assert.Equal(1, template.Length);
        Assert.Equal("a", template[0]);
    }

    [Fact]
    public void SpaceTemplate_Should_Be_Created_On_UnitField()
    {
        SpaceTemplate template = SpaceTemplate.Create(UnitField.Null);

        Assert.Equal(1, template.Length);
        Assert.Equal(UnitField.Null, template[0]);
    }

    [Fact]
    public void SpaceTemplate_Should_Be_Created_On_Type()
    {
        SpaceTemplate template = SpaceTemplate.Create(typeof(int));

        Assert.Equal(1, template.Length);
        Assert.Equal(typeof(int), template[0]);
    }

    [Fact]
    public void SpaceTemplate_Shoud_Be_Statisfied_By_SpaceTuple()
    {
        Assert.True(SpaceTemplate.Create(1).IsSatisfied(SpaceTuple.Create(1)));
        Assert.True(SpaceTemplate.Create((1, "a")).IsSatisfied(SpaceTuple.Create((1, "a"))));
        Assert.True(SpaceTemplate.Create((1, "a", 1.5f)).IsSatisfied(SpaceTuple.Create((1, "a", 1.5f))));
        Assert.True(SpaceTemplate.Create((1, "a", 1.5f, UnitField.Null)).IsSatisfied(SpaceTuple.Create((1, "a", 1.5f, 1.1m))));
        Assert.True(SpaceTemplate.Create((1, UnitField.Null, 1.5f, UnitField.Null)).IsSatisfied(SpaceTuple.Create((1, "a", 1.5f, 1.1m))));
    }

    [Fact]
    public void SpaceTemplate_Shoud_Not_Be_Statisfied_By_SpaceTuple()
    {
        Assert.False(SpaceTemplate.Create((1, "a")).IsSatisfied(SpaceTuple.Create(1)));
        Assert.False(SpaceTemplate.Create((1, "a", 1.5f)).IsSatisfied(SpaceTuple.Create((1, "a", 2.5f))));
        Assert.False(SpaceTemplate.Create((1, "a", 1.5f, UnitField.Null)).IsSatisfied(SpaceTuple.Create((1, "b", 1.5f, 1.1m))));
        Assert.False(SpaceTemplate.Create((1, UnitField.Null, 1.5f, UnitField.Null)).IsSatisfied(SpaceTuple.Create((1, "a", 2.5f, 1.1m))));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Null()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTemplate.Create(null));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Null_Object()
    {
        Assert.Throws<ArgumentNullException>(() => SpaceTemplate.Create((object)null));
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Default_Constructor()
    {
        Assert.Throws<ArgumentException>(() => new SpaceTemplate());
    }

    [Fact]
    public void Exception_Should_Be_Thrown_On_Empty_ValueTuple()
    {
        Assert.Throws<ArgumentException>(() => SpaceTemplate.Create(new ValueTuple()));
    }

    [Fact]
    public void Exception_Should_Not_Be_Thrown_If_Template_Contains_UnitField()
    {
        var expection = Record.Exception(() => SpaceTemplate.Create((1, "a", UnitField.Null)));
        Assert.Null(expection);
    }

    [Fact]
    public void Exception_Should_Not_Be_Thrown_If_Template_Contains_Types()
    {
        var expection = Record.Exception(() => SpaceTemplate.Create((1, typeof(int), UnitField.Null)));
        Assert.Null(expection);
    }

    [Fact]
    public void Should_Be_Equal()
    {
        SpaceTemplate template1 = SpaceTemplate.Create((1, "a", 1.5f, UnitField.Null));
        SpaceTemplate template2 = SpaceTemplate.Create((1, "a", 1.5f, UnitField.Null));

        Assert.Equal(template1, template2);
        Assert.True(template1 == template2);
        Assert.False(template1 != template2);
    }

    [Fact]
    public void Should_Be_Equal_On_Object()
    {
        SpaceTemplate template = SpaceTemplate.Create((1, "a", 1.5f, UnitField.Null));
        object obj = SpaceTemplate.Create((1, "a", 1.5f, UnitField.Null));

        Assert.True(template.Equals(obj));
    }

    [Fact]
    public void Should_Not_Be_Equal_On_Object()
    {
        SpaceTemplate template = SpaceTemplate.Create((1, "a", 1.5f));
        object obj = SpaceTemplate.Create((1, "a", UnitField.Null));

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
    public void Should_Be_Faster_For_Different_Lengths()
    {
        long firstRun = Run(SpaceTemplate.Create((1, "a")), SpaceTemplate.Create((1, "b")));
        long secondRun = Run(SpaceTemplate.Create((1, "a")), SpaceTemplate.Create(1));

        Assert.True(firstRun > secondRun);

        static long Run(SpaceTemplate template1, SpaceTemplate template2)
        {
            Stopwatch watch = new();

            watch.Start();
            _ = template1 == template2;
            watch.Stop();

            return watch.ElapsedTicks;
        }
    }

    [Fact]
    public void Should_ToString()
    {
        Assert.Equal("<1>", SpaceTemplate.Create(1).ToString());
        Assert.Equal("<1, a>", SpaceTemplate.Create((1, "a")).ToString());
        Assert.Equal("<1, a, 1.5>", SpaceTemplate.Create((1, "a", 1.5f)).ToString());
        Assert.Equal("<1, a, 1.5, b>", SpaceTemplate.Create((1, "a", 1.5f, 'b')).ToString());
        Assert.Equal("<1, a, 1.5, b, {NULL}>", SpaceTemplate.Create((1, "a", 1.5f, 'b', UnitField.Null)).ToString());
        Assert.Equal("<1, a, 1.5, b, {NULL}, System.Int32>", SpaceTemplate.Create((1, "a", 1.5f, 'b', UnitField.Null, typeof(int))).ToString());
    }
}