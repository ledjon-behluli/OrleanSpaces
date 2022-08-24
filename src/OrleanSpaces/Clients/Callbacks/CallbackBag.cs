﻿using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Clients.Callbacks;

internal class CallbackBag
{
    public SpaceTuple Tuple { get; }
    public Func<SpaceTuple, Task> Callback { get; }

    public CallbackBag(SpaceTuple tuple, Func<SpaceTuple, Task> callback)
    {
        Tuple = tuple;
        Callback = callback;
    }

    public async Task DispatchAsync() => await Callback(Tuple);
}