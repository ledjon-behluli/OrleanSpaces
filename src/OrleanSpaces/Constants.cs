namespace OrleanSpaces;

public static class Constants
{
    /// <summary>
    /// The name of the storage provider.
    /// </summary>
    public const string StorageName = "OrleanSpaces";
    /// <summary>
    /// The name of the stream provider.
    /// </summary>
    public const string StreamName = "OrleanSpace";
    /// <summary>
    /// The name of the pub-sub stream provider.
    /// </summary>
    public const string PubSubProvider = "PubSubProvider";
    /// <summary>
    /// The name of the pub-sub storage provider.
    /// </summary>
    public const string PubSubStore = "PubSubStore";

    internal const int MaxTuplesPerShard = 5000;
    internal const int MaxStackSize = 1024;

    internal const int ByteCount_Int128 = 16;                   // maximum value can be represented by this number of bytes.
    internal const int ByteCount_UInt128 = 16;                  // maximum value can be represented by this number of bytes.

    internal const int MaxFieldCharLength_Bool = 5;             // False
    internal const int MaxFieldCharLength_Char = 1;             // a
    internal const int MaxFieldCharLength_Byte = 3;             // 255
    internal const int MaxFieldCharLength_SByte = 4;            // -128
    internal const int MaxFieldCharLength_Short = 6;            // -32768
    internal const int MaxFieldCharLength_UShort = 5;           // 65535
    internal const int MaxFieldCharLength_Int = 11;             // -2147483648
    internal const int MaxFieldCharLength_UInt = 10;            // 4294967295
    internal const int MaxFieldCharLength_Long = 20;            // -9223372036854775808
    internal const int MaxFieldCharLength_ULong = 20;           // 18446744073709551615
    internal const int MaxFieldCharLength_Huge = 40;            // -170141183460469231731687303715884105728
    internal const int MaxFieldCharLength_UHuge = 39;           // 170141183460469231731687303715884105728
    internal const int MaxFieldCharLength_DateTime = 22;        // 12/31/9999 11:59:59 PM
    internal const int MaxFieldCharLength_DateTimeOffset = 29;  // 12/31/9999 11:59:59 PM +00:00
    internal const int MaxFieldCharLength_TimeSpan = 26;        // -10675199.02:48:05.4775808
    internal const int MaxFieldCharLength_Guid = 36;            // 00000000-0000-0000-0000-000000000000
    internal const int MaxFieldCharLength_Double = 24;          // -1.7976931348623157E+308
    internal const int MaxFieldCharLength_Decimal = 30;         // -79228162514264337593543950335
    internal const int MaxFieldCharLength_Float = 24;           // -3.4028235E+38

    internal const string RealmKey_Space = "space-realm";
    internal const string RealmKey_Bool = "bool-realm";
    internal const string RealmKey_Char = "char-realm";
    internal const string RealmKey_Byte = "byte-realm";
    internal const string RealmKey_SByte = "sbyte-realm";
    internal const string RealmKey_Short = "short-realm";
    internal const string RealmKey_UShort = "ushort-realm";
    internal const string RealmKey_Int = "int-realm";
    internal const string RealmKey_UInt = "uint-realm";
    internal const string RealmKey_Long = "long-realm";
    internal const string RealmKey_ULong = "ulong-realm";
    internal const string RealmKey_Huge = "huge-realm";
    internal const string RealmKey_UHuge = "uhuge-realm";
    internal const string RealmKey_DateTime = "datetime-realm";
    internal const string RealmKey_DateTimeOffset = "datetimeoffset-realm";
    internal const string RealmKey_TimeSpan = "timespan-realm";
    internal const string RealmKey_Guid = "guid-realm";
    internal const string RealmKey_Double = "double-realm";
    internal const string RealmKey_Decimal = "decimal-realm";
    internal const string RealmKey_Float = "float-realm";
}