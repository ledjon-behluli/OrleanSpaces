using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tests;

public class Class1
{
    [Fact]
    public void A()
    {
        bool[] arr1 = { false, false };
        bool[] arr2 = { true, true };

        BoolTuple bt1 = new(arr1);
        BoolTuple bt2 = new(arr2);
        BoolTuple bt3 = new(arr2);

        Assert.NotEqual(bt1, bt2);
        Assert.NotEqual(bt1, bt3);
        Assert.Equal(bt2, bt3);

        BoolTuple_Opt bt1_o = new(arr1);
        BoolTuple_Opt bt2_o = new(arr2);
        BoolTuple_Opt bt3_o = new(arr2);

        Assert.NotEqual(bt1_o, bt2_o);
        Assert.NotEqual(bt1_o, bt3_o);
        Assert.Equal(bt2_o, bt3_o);
    }
}

