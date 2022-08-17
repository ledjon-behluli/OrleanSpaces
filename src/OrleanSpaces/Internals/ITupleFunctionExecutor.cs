namespace OrleanSpaces.Internals;

internal interface ITupleFunctionExecutor
{
    Task ExecuteAsync(byte[] serializedFunction);
}