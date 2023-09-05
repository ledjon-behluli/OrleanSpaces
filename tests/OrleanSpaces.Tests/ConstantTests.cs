namespace OrleanSpaces.Tests;

public class ConstantTests
{
    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("OrleanSpaces", Constants.StorageName);
        Assert.Equal("OrleanSpaces", Constants.StreamName);
        Assert.Equal("PubSubProvider", Constants.PubSubProvider);
        Assert.Equal("PubSubStore", Constants.PubSubStore);

        Assert.Equal(1024, Constants.MaxStackSize);

        Assert.Equal(16, Constants.ByteCount_Int128);
        Assert.Equal(16, Constants.ByteCount_UInt128);

        Assert.Equal(5, Constants.MaxFieldCharLength_Bool);
        Assert.Equal(1, Constants.MaxFieldCharLength_Char);
        Assert.Equal(3, Constants.MaxFieldCharLength_Byte);
        Assert.Equal(4, Constants.MaxFieldCharLength_SByte);
        Assert.Equal(6, Constants.MaxFieldCharLength_Short);
        Assert.Equal(5, Constants.MaxFieldCharLength_UShort);
        Assert.Equal(11, Constants.MaxFieldCharLength_Int);
        Assert.Equal(10, Constants.MaxFieldCharLength_UInt);
        Assert.Equal(20, Constants.MaxFieldCharLength_Long);
        Assert.Equal(20, Constants.MaxFieldCharLength_ULong);
        Assert.Equal(40, Constants.MaxFieldCharLength_Huge);
        Assert.Equal(39, Constants.MaxFieldCharLength_UHuge);
        Assert.Equal(22, Constants.MaxFieldCharLength_DateTime);
        Assert.Equal(29, Constants.MaxFieldCharLength_DateTimeOffset);
        Assert.Equal(26, Constants.MaxFieldCharLength_TimeSpan);
        Assert.Equal(36, Constants.MaxFieldCharLength_Guid);
        Assert.Equal(24, Constants.MaxFieldCharLength_Double);
        Assert.Equal(30, Constants.MaxFieldCharLength_Decimal);
        Assert.Equal(24, Constants.MaxFieldCharLength_Float);

        Assert.Equal("space-realm", Constants.RealmKey_Space);
        Assert.Equal("bool-realm", Constants.RealmKey_Bool);
        Assert.Equal("char-realm", Constants.RealmKey_Char);
        Assert.Equal("byte-realm", Constants.RealmKey_Byte);
        Assert.Equal("sbyte-realm", Constants.RealmKey_SByte);
        Assert.Equal("short-realm", Constants.RealmKey_Short);
        Assert.Equal("ushort-realm", Constants.RealmKey_UShort);
        Assert.Equal("int-realm", Constants.RealmKey_Int);
        Assert.Equal("uint-realm", Constants.RealmKey_UInt);
        Assert.Equal("long-realm", Constants.RealmKey_Long);
        Assert.Equal("ulong-realm", Constants.RealmKey_ULong);
        Assert.Equal("huge-realm", Constants.RealmKey_Huge);
        Assert.Equal("uhuge-realm", Constants.RealmKey_UHuge);
        Assert.Equal("datetime-realm", Constants.RealmKey_DateTime);
        Assert.Equal("datetimeoffset-realm", Constants.RealmKey_DateTimeOffset);
        Assert.Equal("timespan-realm", Constants.RealmKey_TimeSpan);
        Assert.Equal("guid-realm", Constants.RealmKey_Guid);
        Assert.Equal("double-realm", Constants.RealmKey_Double);
        Assert.Equal("decimal-realm", Constants.RealmKey_Decimal);
        Assert.Equal("float-realm", Constants.RealmKey_Float);
    }
}
