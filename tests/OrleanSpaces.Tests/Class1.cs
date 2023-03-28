using OrleanSpaces.Tuples;
using System.Numerics;

namespace OrleanSpaces.Tests;

public class Class1
{
    [Fact]
    public void A()
    {
        if (Vector.IsHardwareAccelerated)
        {
            IntegerTuple tuple = new(new[] { 1, 2, 3, 4});
            IntegerTuple tuple1 = new(new[] { 1, 2, 3, 5 });

            Assert.Equal(tuple, tuple1);
        }
    }
}
