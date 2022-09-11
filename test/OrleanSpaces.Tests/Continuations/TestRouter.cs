using OrleanSpaces.Continuations;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Continuations;

internal class TestRouter : ITupleRouter
{
    public ITuple Tuple { get; private set; }
    
    public Task RouteAsync(ITuple tuple)
    {
        Tuple = tuple;
        return Task.CompletedTask;
    }
}