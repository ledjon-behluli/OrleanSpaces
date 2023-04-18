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
        TimeSpan[] ts1 = new TimeSpan[] { TimeSpan.MinValue, TimeSpan.MaxValue };
        TimeSpan[] ts2 = new TimeSpan[] { TimeSpan.Zero, TimeSpan.MinValue, TimeSpan.MaxValue };

        TimeSpanTuple tst1 = new(ts1);
        TimeSpanTuple tst2 = new(ts2);

        Assert.Equal(tst1, tst1);
        Assert.NotEqual(tst1, tst2);
    }
}

