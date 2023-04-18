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

        decimal decimalValue = 1234.5678m;

        Span<int> destination = stackalloc int[4];

        _ = decimal.GetBits(decimalValue, destination);

        Span<int> bits = MemoryMarshal.Cast<byte, int>(destination);
    }
}

