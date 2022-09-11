using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public readonly struct SpaceTupleAlloc //: ISpaceTuple
{
    private readonly ITuple fields;

    public readonly object this[int index] => fields[index];

    public int Length => fields?.Length ?? 0;
    public bool IsEmpty => Length == 0;

    private SpaceTupleAlloc(ITuple fields)
    {
        this.fields = fields;
    }

    public static SpaceTupleAlloc Create(ValueType value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
      
        if (value is ITuple tuple)
        {
            for (int i = 0; i < tuple.Length; i++)
            {
                Type type = tuple[i].GetType();

                if (type == typeof(string) ||
                   (type.IsValueType && !type.IsPrimitive && !type.IsEnum))
                {
                    continue;
                }

                throw new ArgumentException("Only primitive and string types are allowed space tuple fields");
            }

            return new(tuple);
        }

        return new(new ValueTuple<ValueType>(value));
    }

    public static SpaceTupleAlloc Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new(new ValueTuple<string>(value));
    }
}



//[Serializable]
//public readonly struct SpaceTupleAlloc //: ISpaceTuple, IEquatable<SpaceTupleAlloc>
//{
//    private readonly ValueType[] fields;

//    public ref readonly ValueType this[int index] => ref fields[index];

//    public int Length => fields.Length;
//    public bool IsEmpty => Length == 0;

//    public SpaceTupleAlloc() : this(null) { }

//    private SpaceTupleAlloc(ValueType[]? fields)
//    {
//        this.fields = fields ?? Array.Empty<ValueType>();
//    }

//    public static SpaceTupleAlloc Create(ITuple tuple)
//    {
//        ValueType[] fields = new ValueType[] {};
//        int length = tuple.Length;

//        for (int i = 0; i < length; i++)
//        {
//            if (tuple[i] is ValueType value)
//            {
//                fields[i] = value;
//            }
//            else if (tuple[i] is string str)
//            {
//                ValueType[] chars = ConvertString(str);
//                length += chars.Length - 1;

//                foreach (char @char in chars)
//                {
//                    fields[i] = @char;
//                    i++;
//                }
//            }
//            else
//            {
//                throw new ArgumentException($"Reference types other than '{nameof(String)}' are not valid '{nameof(SpaceTuple)}' fields");
//            }
//        }

//        return new(fields);
//    }

//    public static SpaceTupleAlloc CreateSingle(ValueType value)
//    {
//        if (value is null)
//        {
//            throw new ArgumentNullException(nameof(value));
//        }

//        return new(new ValueType[1] { value });
//    }

//    public static SpaceTupleAlloc CreateSingle(string value)
//        => new(ConvertString(value));

//    private static ValueType[] ConvertString(string value)
//    {
//        if (string.IsNullOrEmpty(value))
//        {
//            throw new ArgumentNullException(nameof(value));
//        }

//        ReadOnlySpan<char> chars = value.AsSpan();
//        int totalChars = chars.Length;

//        ValueType[] values = new ValueType[totalChars + 2];

//        values[0] = '|';
//        values[totalChars + 1] = '|';

//        for (int i = 1; i <= totalChars; i++)
//        {
//            values[i] = chars[i];
//        }

//        return values;
//    }
//}






//[Serializable]
//public readonly struct SpaceTupleAlloc //: ISpaceTuple, IEquatable<SpaceTupleAlloc>
//{
//    private readonly ValueType[] fields;
//    private readonly string[] stringFields;

//    public ref readonly ValueType this[int index] => ref fields[index];

//    public int Length => fields.Length + stringFields.Length;
//    public bool IsEmpty => Length == 0;

//    public SpaceTupleAlloc() : this(null, null) { }

//    private SpaceTupleAlloc(ValueType[]? fields, string[]? stringFields)
//    {
//        this.fields = fields ?? Array.Empty<ValueType>();
//        this.stringFields = stringFields ?? Array.Empty<string>();
//    }

//    public static SpaceTupleAlloc Create(ITuple tuple)
//    {
//        var fields = new ValueType[tuple.Length];
//        var stringFields = new string[tuple.Length];

//        for (int i = 0; i < tuple.Length; i++)
//        {
//            if (tuple[i] is ValueType value)
//            {
//                fields[i] = value;
//            }
//            else if (tuple[i] is string str)
//            {
//                stringFields[i] = str;
//            }
//            else
//            {
//                throw new ArgumentException($"Reference types other than '{nameof(String)}' are not valid '{nameof(SpaceTuple)}' fields");
//            }
//        }

//        return new(fields, stringFields);
//    }

//    //public static SpaceTupleAlloc CreatePool(ITuple tuple)
//    //{
//    //    if (tuple is null)
//    //    {
//    //        throw new ArgumentNullException(nameof(tuple));
//    //    }

//    //    var pool = ArrayPool<object>.Shared;
//    //    var fields = pool.Rent(tuple.Length);

//    //    try
//    //    {
//    //        for (int i = 0; i < tuple.Length; i++)
//    //        {
//    //            if (tuple[i] is not ValueType &&
//    //                tuple[i] is not string)
//    //            {
//    //                throw new ArgumentException($"Reference types are not valid '{nameof(SpaceTupleAlloc)}' fields");
//    //            }

//    //            if (tuple[i] is SpaceUnit)
//    //            {
//    //                throw new ArgumentException($"'{nameof(SpaceUnit.Null)}' is not a valid '{nameof(SpaceTupleAlloc)}' field");
//    //            }

//    //            fields[i] = tuple[i];
//    //        }

//    //        return new(fields);
//    //    }
//    //    finally
//    //    {
//    //        pool.Return(fields);
//    //    }
//    //}

//    public static SpaceTupleAlloc CreateSingle(ValueType value)
//    {
//        if (value is null)
//        {
//            throw new ArgumentNullException(nameof(value));
//        }

//        return new(new ValueType[1] { value }, null);
//    }

//    public static SpaceTupleAlloc CreateSingle(string value)
//    {
//        if (string.IsNullOrEmpty(value))
//        {
//            throw new ArgumentNullException(nameof(value));
//        }

//        return new(null, new string[1] { value });
//    }
//}
