using System.Linq.Expressions;
using OrleanSpaces.Core.Primitives;
using Serialize.Linq.Serializers;

namespace OrleanSpaces.Core.Utils;

internal class FuncSerializer
{
    private readonly ExpressionSerializer serializer;

    public FuncSerializer()
    {
        serializer = new ExpressionSerializer(new BinarySerializer());

        serializer.AddKnownType(typeof(SpaceTuple));
        serializer.AddKnownType(typeof(SpaceTemplate));
    }

    public byte[] Serialize(Func<SpaceTuple> func)
    {
        Expression<Func<SpaceTuple>> expression = () => func();
        return serializer.SerializeBinary(expression);
    }

    public Func<SpaceTuple>? Deserialize(byte[] expression)
    {
        if (serializer.DeserializeBinary(expression) is LambdaExpression lambda)
        {
            return lambda.Compile() as Func<SpaceTuple>;
        }

        return null;
    }
}