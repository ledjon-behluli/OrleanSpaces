﻿using System.Linq.Expressions;
using System.Reflection;
using OrleanSpaces.Core.Primitives;
using Serialize.Linq.Serializers;

namespace OrleanSpaces.Core.Utils;

internal class LambdaSerializer
{
    private readonly ExpressionSerializer serializer;

    public LambdaSerializer()
    {
        serializer = new ExpressionSerializer(new BinarySerializer());

        serializer.AddKnownType(typeof(SpaceTuple));
        serializer.AddKnownType(typeof(SpaceTemplate));
    }

    public byte[] Serialize(Func<SpaceTuple> func)
    {
        if (func == null)
        {
            throw new ArgumentNullException("func");
        }

        if (!IsLamda(func.Method))
        {
            throw new ArgumentException("The provided func delegate must be a lamda expression");
        }

        Expression<Func<SpaceTuple>> expression = () => func();
        return serializer.SerializeBinary(expression);

        static bool IsLamda(MethodInfo method)
        {
            var invalidChars = new[] { '<', '>' };
            return method.Name.Any(invalidChars.Contains);
        }
    }

    public Func<SpaceTuple> Deserialize(byte[] serializedFunc)
    {
        if (serializedFunc == null || serializedFunc.Length == 0)
        {
            throw new ArgumentException("The provided serialized func is either null or an empty byte array.");
        }

        var lambda = (LambdaExpression)serializer.DeserializeBinary(serializedFunc);
        return (Func<SpaceTuple>)lambda.Compile();
    }
}