using OrleanSpaces.Grains;

namespace OrleanSpaces.Tests.Grains;

public class IdentityTests
{
    [Theory]
    [MemberData(nameof(KeyData))]
    public void Should_Equal_Key(string expected, string actual)
        => Assert.Equal(expected, actual);

    private static object[][] KeyData() =>
        new[]
        {
            new object[] { "BoolStore", IBoolGrain.Key },
            new object[] { "ByteStore", IByteGrain.Key },
            new object[] { "CharStore", ICharGrain.Key },
            new object[] { "DateTimeOffsetStore", IDateTimeOffsetGrain.Key },
            new object[] { "DateTimeStore", IDateTimeGrain.Key },
            new object[] { "DecimalStore", IDecimalGrain.Key },
            new object[] { "DoubleStore", IDoubleGrain.Key },
            new object[] { "FloatStore", IFloatGrain.Key },
            new object[] { "GuidStore", IGuidGrain.Key },
            new object[] { "HugeStore", IHugeGrain.Key },
            new object[] { "IntStore", IIntGrain.Key },
            new object[] { "LongStore", ILongGrain.Key },
            new object[] { "SByteStore", ISByteGrain.Key },
            new object[] { "ShortStore", IShortGrain.Key },
            new object[] { "SpaceStore", ISpaceGrain.Key },
            new object[] { "TimeSpanStore", ITimeSpanGrain.Key },
            new object[] { "UHugeStore", IUHugeGrain.Key },
            new object[] { "UIntStore", IUIntGrain.Key },
            new object[] { "ULongStore", IULongGrain.Key },
            new object[] { "UShortStore", IUShortGrain.Key }
        };
}