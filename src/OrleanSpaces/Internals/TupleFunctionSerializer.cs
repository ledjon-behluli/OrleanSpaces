using System.Linq.Expressions;
using OrleanSpaces.Types;
using Serialize.Linq.Serializers;

namespace OrleanSpaces.Internals;

internal class TupleFunctionSerializer
{
    private readonly ExpressionSerializer serializer;

    public TupleFunctionSerializer()
    {
        serializer = new ExpressionSerializer(new BinarySerializer());

        serializer.AddKnownType(typeof(SpaceTuple));
        serializer.AddKnownType(typeof(SpaceTemplate));
    }

    public byte[] Serialize(TupleFunction function)
    {
        Expression<TupleFunction> expression = provider => function(provider);
        return serializer.SerializeBinary(expression);
    }

    public TupleFunction? Deserialize(byte[] expression)
    {
        if (serializer.DeserializeBinary(expression) is LambdaExpression lambda)
        {
            return lambda.Compile() as TupleFunction;
        }

        return null;
    }
}