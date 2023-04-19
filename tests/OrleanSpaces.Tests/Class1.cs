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
        DecimalTupleV1 dt1 = new(1111111111111.11m, 11.111m); //new(111.11m, 123.321m, 11111111111.1111m, 2222222222222222222.222222m);
        DecimalTupleV1 dt2 = new(1111111111111.11m, 11.111m); //new(111.11m, 123.321m, 11111111111.1111m, 2222222222222222222.222222m);
        DecimalTupleV1 dt3 = new(1111111111111.12m, 11.112m); //new(2222222222222222222.222222m, 11111111111.1111m, 123.321m, 111.11m);

        Assert.Equal(dt1, dt2);
        Assert.NotEqual(dt1, dt3);
    }
}

