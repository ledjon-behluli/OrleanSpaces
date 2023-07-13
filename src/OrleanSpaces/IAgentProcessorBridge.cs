using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal interface IAgentProcessorBridge<T> where T : ISpaceTuple
{
    void SetStore(ITupleStore<T> store);
    ValueTask ConsumeAsync(TupleAction<T> action);
}
