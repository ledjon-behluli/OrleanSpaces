using OrleanSpaces.Tuples.Numerics;
using System.Numerics;

namespace OrleanSpaces.Tests;

public class Class1
{
    [Fact]
    public void A()
    {
        if (Vector.IsHardwareAccelerated)
        {
            IntTuple tuple = new(new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1});
            IntTuple tuple1 = new(new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 });

            Assert.Equal(tuple, tuple1);
        }
    }
}
