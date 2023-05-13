using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Runtime.CompilerServices;

//Span<char> chars = stackalloc char[48];
//bool result = new IntTuple(1, 1, 1).TryFormat(chars, out int w);

//foreach (ref readonly int item in new IntTuple(1, 1, 1))
//{
//    item = 1;
//    Console.WriteLine(item);
//}

EnumTuple<ByteEnum> byteEnums1 = new(ByteEnum.A, ByteEnum.B, ByteEnum.C);
EnumTuple<ByteEnum> byteEnums2 = new(ByteEnum.A, ByteEnum.B, ByteEnum.C);

Console.WriteLine(byteEnums1 == byteEnums2);


EnumTuple<SByteEnum> sbyteEnums1 = new(SByteEnum.A, SByteEnum.B, SByteEnum.C);
EnumTuple<SByteEnum> sbyteEnums2 = new(SByteEnum.A, SByteEnum.B, SByteEnum.C);

Console.WriteLine(sbyteEnums1 == sbyteEnums2);


EnumTuple<ShortEnum> shortEnums1 = new(ShortEnum.A, ShortEnum.B, ShortEnum.C);
EnumTuple<ShortEnum> shortEnums2 = new(ShortEnum.A, ShortEnum.B, ShortEnum.C);

Console.WriteLine(shortEnums1 == shortEnums2);


EnumTuple<UShortEnum> ushortEnums1 = new(UShortEnum.A, UShortEnum.B, UShortEnum.C);
EnumTuple<UShortEnum> ushortEnums2 = new(UShortEnum.A, UShortEnum.B, UShortEnum.C);

Console.WriteLine(ushortEnums1 == ushortEnums2);


EnumTuple<IntEnum> intEnums1 = new(IntEnum.A, IntEnum.B, IntEnum.C);
EnumTuple<IntEnum> intEnums2 = new(IntEnum.A, IntEnum.B, IntEnum.C);

Console.WriteLine(intEnums1 == intEnums2);


EnumTuple<UIntEnum> uintEnums1 = new(UIntEnum.A, UIntEnum.B, UIntEnum.C);
EnumTuple<UIntEnum> uintEnums2 = new(UIntEnum.A, UIntEnum.B, UIntEnum.C);

Console.WriteLine(uintEnums1 == uintEnums2);


EnumTuple<LongEnum> longEnums1 = new(LongEnum.A, LongEnum.B, LongEnum.C);
EnumTuple<LongEnum> longEnums2 = new(LongEnum.A, LongEnum.B, LongEnum.C);

Console.WriteLine(longEnums1 == longEnums2);


EnumTuple<ULongEnum> ulongEnums1 = new(ULongEnum.A, ULongEnum.B, ULongEnum.C);
EnumTuple<ULongEnum> ulongEnums2 = new(ULongEnum.A, ULongEnum.B, ULongEnum.A);

Console.WriteLine(ulongEnums1 == ulongEnums2);



Console.ReadKey();


public enum ByteEnum : byte
{
    A = 0,
    B = 1,
    C = 2
}

public enum SByteEnum : sbyte
{
    A = -1,
    B = 0,
    C = 1
}

public enum ShortEnum : short
{
    A = -1,
    B = 0,
    C = 1
}

public enum UShortEnum : ushort
{
    A = 0,
    B = 1,
    C = 2
}

public enum IntEnum : int
{
    A = -1,
    B = 0,
    C = 1
}

public enum UIntEnum : uint
{
    A = 0,
    B = 1,
    C = 2
}

public enum LongEnum : long
{
    A = -1,
    B = 0,
    C = 1
}

public enum ULongEnum : ulong
{
    A = 0,
    B = 1,
    C = 2
}