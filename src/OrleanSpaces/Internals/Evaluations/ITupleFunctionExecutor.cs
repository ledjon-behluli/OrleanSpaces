namespace OrleanSpaces.Internals.Evaluations;

internal interface ITupleFunctionExecutor
{
    Task ExecuteAsync(byte[] serializedFunction);
}