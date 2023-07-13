namespace OrleanSpaces;

public sealed class SpaceOptions
{
    public SpaceKind EnabledSpaces { get; set; } = SpaceKind.Generic;
    public bool HandleEvaluationExceptions { get; set; } = true;
    public bool HandleCallbackExceptions { get; set; } = true;
    public bool AllowMultipleAgentStreamConsumers { get; set; } = true;
}

[Flags]
public enum SpaceKind
{
    All = Generic | Bool | Byte | Char | DateTimeOffset | DateTime | Decimal | Double | Float | Guid | Huge | Int | Long | SByte | Short | TimeSpan | UHuge | UInt | ULong | UShort,
    Generic = 1,
    Bool = 2,
    Byte = 4,
    Char = 8,
    DateTimeOffset = 16,
    DateTime = 32,
    Decimal = 64,
    Double = 128,
    Float = 256,
    Guid = 512,
    Huge = 1024,
    Int = 2048,
    Long = 4096,
    SByte = 8192,
    Short = 16384,
    TimeSpan = 32768,
    UHuge = 65536,
    UInt = 131072,
    ULong = 262144,
    UShort = 524288
}
