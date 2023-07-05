namespace OrleanSpaces;

public sealed class SpaceOptions
{
    public SpaceType EnabledSpaces { get; set; } = SpaceType.Generic;
}

[Flags]
public enum SpaceType
{
    Generic = 1,
    Bools = 2,
    Bytes = 4,
    Chars = 8,
    DateTimeOffsets = 16,
    DateTimes = 32,
    Decimals = 64,
    Doubles = 128,
    Floats = 256,
    Guids = 512,
    Huges = 1024,
    Ints = 2048,
    Longs = 4096,
    SBytes = 8192,
    Shorts = 16384,
    TimeSpans = 32768,
    UHuges = 65536,
    UInts = 131072,
    ULongs = 262144,
    UShorts = 524288
}
