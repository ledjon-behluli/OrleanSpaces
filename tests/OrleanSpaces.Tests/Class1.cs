using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tests;

struct MyClass
{
    private int myField;

    public MyClass(int value)
    {
        myField = value;
    }

    public int GetMyFieldValue() => myField;
}

public class Class1
{
    [Fact]
    public void A()
    {
        DateTime dt1 = new(2022, 1, 1);

        DateTimeTuple_Optimized_V2 v2 = new(new DateTime[] { dt1, dt1, dt1, dt1 });

        Assert.Equal(v2, v2);

        Assert.Equal(4, Vector<long>.Count);
        Assert.Equal(8, Vector<int>.Count);
    }
}