namespace OrleanSpaces.Internals;

internal interface ITupleFunctionExecutor
{
    Task Execute(byte[] serializedFunction);
}
