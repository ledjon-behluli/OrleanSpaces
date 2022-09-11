using System.Runtime.CompilerServices;

namespace OrleanSpaces.Continuations;

internal interface ITupleRouter
{
    Task RouteAsync(ITuple tuple);
}
