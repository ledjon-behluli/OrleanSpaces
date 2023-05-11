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


Do(IntEnum.A);
Do(LongEnum.A);
Do(ByteEnum.A);

Console.ReadKey();

static void Do<T>(T obj) where T : unmanaged, Enum
{
    TypeCode underlyingType = Type.GetTypeCode(typeof(T));
    Console.WriteLine($"The underlying type of {typeof(T).Name} is {underlyingType}");
}


public enum IntEnum
{
    A
}

public enum LongEnum : long
{
    A
}

public enum ByteEnum : byte
{
    A
}
