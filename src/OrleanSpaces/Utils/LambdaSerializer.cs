using System.Linq.Expressions;
using System.Reflection;
using OrleanSpaces.Primitives;
using Serialize.Linq.Serializers;

namespace OrleanSpaces.Utils;

internal static class LambdaSerializer
{
    private readonly static ExpressionSerializer serializer;

    static LambdaSerializer()
    {
        serializer = new ExpressionSerializer(new BinarySerializer());

        serializer.AddKnownType(typeof(SpaceTuple));
        serializer.AddKnownType(typeof(SpaceTemplate));
    }

    public static byte[] Serialize(Func<SpaceTuple> func)
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

    public static Func<SpaceTuple> Deserialize(byte[] serializedFunc)
    {
        if (serializedFunc == null || serializedFunc.Length == 0)
        {
            throw new ArgumentException("The provided serialized func is either null or an empty byte array.");
        }

        var lambda = (LambdaExpression)serializer.DeserializeBinary(serializedFunc);
        return (Func<SpaceTuple>)lambda.Compile();
    }
}