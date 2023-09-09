
# Installation

Installation is performed via [NuGet](https://www.nuget.org/packages/OrleanSpaces/)

From Package Manager:

> PS> Install-Package OrleanSpaces

.Net CLI:

> \# dotnet add package OrleanSpaces

Paket:

> \# paket add OrleanSpaces

# Configuration

## Client

Configuration of the **Tuple Space Client** is done by configuring the **Orleans Client** and calling the `AddOrleanSpaces()` extension method. Below we can see a typical configuration with *localhost clustering* and *memory streams*.

```cs
await new HostBuilder()
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces();
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .Build()
    .StartAsync();
```

The `AddOrleanSpaces` extension method accepts an optional `Action<SpaceClientOptions>` parameter that is can be used to configure the `SpaceClientOptions`.

Detailed explanations on what each of the options mean is provided via XML documentation of the `SpaceClientOptions` class itself. But here I want to point out, that <u>by default only the generic agent is configured to run</u>. If you need multiple agents, you can configure them via bitwise OR operation, like this:

```cs
AddOrleanSpaces(options => options.EnabledSpaces = SpaceKind.Generic | SpaceKind.Int | SpaceKind.Bool);
```

Or if you want to run all agents you can use the `All` enum value.

```cs
AddOrleanSpaces(options => options.EnabledSpaces = SpaceKind.All);
```

## Server

Configuration of the **Tuple Space Server** is done by configuring the **Orleans Silo** and calling the `AddOrleanSpaces()` extension method. Below we can see a typical configuration with *localhost clustering*, *in-memory persistence* and *memory streams*.

```cs
await Host.CreateDefaultBuilder(args)
    .UseOrleans(builder =>
    {
	builder.AddOrleanSpaces();
	builder.UseLocalhostClustering();
	builder.AddMemoryStreams(Constants.PubSubProvider);
        builder.AddMemoryGrainStorage(Constants.PubSubStore);
        builder.AddMemoryGrainStorage(Constants.StorageName); 
    })
    .Build()
    .StartAsync();
```

The `AddOrleanSpaces` extension method accepts two optional parameters.

* `Action<SpaceServerOptions>` is used to configure the `SpaceServerOptions` which are specific to the spaces.
* `Action<SpaceClientOptions>` is used to configure the `SpaceClientOptions` which are specific to the agents. This allows for code hosted on the silo to be able to interact with the spaces, if needed.

```cs
siloBuilder.AddOrleanSpaces(
   configureServerOptions: serverOptions =>
   {
       ...
   },
   configureClientOptions: clientOptions =>
   {   
       clientOptions.EnabledSpaces = SpaceKind.Generic; // the generic agent is now accessible to the silo too
   });
```
Whether you should configure the *client options* or not, depends how you want to make use of the tuple space(s). Below are some rule of thumbs:

* DO NOT call it, when you are hosting the silo in the [same process](https://learn.microsoft.com/en-us/dotnet/orleans/host/client?pivots=orleans-7-0#co-hosted-clients:~:text=If%20the%20client%20code%20is%20hosted%20in%20the%20same%20process%20as%20the%20grain%20code) as the client.
* DO call it, when you are hosting the silo in a [different process](https://learn.microsoft.com/en-us/dotnet/orleans/host/client?pivots=orleans-7-0#co-hosted-clients:~:text=Client%20code%20can%20run%20outside%20of%20the%20Orleans%20cluster%20where%20grain%20code%20is%20hosted.) than the client, and you DO want to interact with the tuple space from the silo.
* DO NOT call it, when you are hosting the silo in a [different process](https://learn.microsoft.com/en-us/dotnet/orleans/host/client?pivots=orleans-7-0#co-hosted-clients:~:text=Client%20code%20can%20run%20outside%20of%20the%20Orleans%20cluster%20where%20grain%20code%20is%20hosted.) than the client, and you DO NOT want to interact with the tuple space from the silo.

Here I want to point out, that <u>by default no agent is configured to run</u>. If you need to have access to the agents, you can configure them via the `configureClientOptions` in the same way as we did for the client.

## ⚠️ Orleans Serialization Configuration ⚠️

An important note on the users of this library!!!

By default Orleans use `Newtonsoft.Json` as the serializer, but it also gives the users the option to choose what serializer they want Orleans to use. The tuple and template constructs in this library make use of some attributes found in `Newtonsoft.Json` in order to be compliant with the default serializer option of Orleans. When users want to pick a different serializer they should exclude the *OrleanSpaces* namespace from that serializer.

This is unavoidable until Orleans adds full support for `DataMemberAttribute`.

Read more on: [Orleans Serialization Configuration](https://learn.microsoft.com/en-us/dotnet/orleans/host/configuration-guide/serialization-configuration?pivots=orleans-7-0).

# Partitioning

Each space kind automatically partitions the data (*i.e. stored tuples*). This avoids performance impacts due to frequent updates to a large list of stored tuples, because each update would involve serializing and persisting the entire dataset.

Partitioning is configurable on the server via the `SpaceServerOptions.PartitioningThreshold` which defines the maximum number of tuples that should be stored within a partition per type of space.

# Usage

Below you can see some simple examples on how to use this library. If you want to dive deeper, than a good amount of examples are available in the [samples](https://github.com/ledjon-behluli/OrleanSpaces/tree/master/samples) directory of the project, so go ahead and have fun with it 🙂.

## Tuples & Templates

There are a variety of tuples & templates (*which are first class citizens*) that the client can use to interact with the tuple space. Each tuple type has its corresponding template type.

*Fields* - Are the individual elements of any tuple / template.

* `SpaceTuple` & `SpaceTemplate` - Are generic type tuples that can be used when you want to store different kind of field types together.
* There are also specialized (*thereby strongly-typed*) tuples & templates that accept only a specific kind of field type. The library comes with a plethora of tuples and corresponding templates for the most used .NET types: `bool`, `byte`, `char`, `DateTime`, `DateTimeOffset`, `decimal`, `double`, `float`, `Guid`, `Int128`, `int`, `long`, `sbyte`, `short`, `TimeSpan`, `UInt128`, `uint`, `ulong`, `ushort`.

## Space Agents

The agent is the core component that the client can use to interact with the tuple space. There is only **one instance** of an agent type running **per process**. For every type of tuple-template pair, there exists a corresponding agent. The agents can be retrieved from the `IServiceProvider`.
```cs
var genericAgent = await host.Services.GetRequiredService<ISpaceAgent>();
var intAgent = host.Services.GetRequiredService<ISpaceAgent<int, IntTuple, IntTemplate>>();
```
>⚠️ All agents are isolated between each other, therefor the tuple spaces are also isolated. An action performed on say the `int` agent, has not effect whatsoever on say the `bool` agent, generic agent and so on.  

## Startup Behavior

By default once the agent(s) are started they automatically load the respective tuple space contents (*i.e. the tuples*) into memory, and than perform various operations to keep it in-sync with the actual tuple space.

In certain cases, like where the agent is used to perform only writes, or the application needs fast startup times, the default behavior may not be suitable. This loading behavior is configurable via the `SpaceClientOptions.LoadSpaceContentsUponStartup` flag. The client can continue to operate without the loaded space if its intent is to only write to the space, or subscribe to space events.

It should be noted that it is always possible to load the space via the `ReloadAsync` method, found on every agent type. As the name suggests, the method can be called (*for whatever reason*) even if the space was loaded upon startup.

## Communication Models

At the core of this library, programing is done in tuples, but the library empowers the user to work with via different communication models. OrleanSpaces does implement the following models:

* Synchronous Request-Reply model.
* Asynchronous Request-Reply model.
* Streaming model.
* Publish-Subscribe model.

#### Synchronous Request-Reply

The answer is made available at the moment of invoking the respective methods. If a tuple is found that matches the template, then it gets returned, if not than an empty tuple is returned.

```cs
SpaceTuple tuple = new(1, "a", 1.5f);
SpaceTemplate template = new (1, null, 1.5f);

await agent.WriteAsync(tuple);

SpaceTuple peekedTuple = agent.Peek(template);
SpaceTuple popedTuple = await agent.PopAsync(template);

int count = agent.Count;

IEnumerable<SpaceTuple> allTuples = agent.Enumerate();
IEnumerable<SpaceTuple> someTuples = agent.Enumerate(template);

await agent.ClearAsync();
await agent.ReloadAsync();
```

#### Asynchronous Request-Reply

The answer is made available at the moment of invoking the respective methods OR whenever the condition is satisfied. If a tuple is found that matches the template, then it gets returned, if not than eventually the callback will get invoked whenever the provided template matches a tuple.

```cs
SpaceTuple tuple = new(1, "a", 1.5f);
SpaceTemplate template = new (1, null, 1.5f);

await agent.EvaluateAsync(async () =>
{
    await Task.Delay(100);  // evaluating...
    return tuple;
});

await agent.PeekAsync(template, tuple =>
{
    // do something
    return Task.CompletedTask;
});

await agent.PopAsync(template, tuple =>
{
    // Doing something with the tuple.
    return Task.CompletedTask;
});
```

#### Streaming

The `EnumerateAsync` method accepts a `SpaceTemplate` or `default`, and returns an `IAsyncEnumerable` of tuples. The agent will stream all (*or the once that match the template, provided it is not `default`*) tuples that get written to the respective tuple space type.

```cs
_ = Task.Run(async () =>
{
    await foreach (var tuple in agent.EnumerateAsync())
    {
        Console.WriteLine(tuple);
    }
});

int i = 0;
while (true)
{
    SpaceTuple tuple = new(i);
    await agent.WriteAsync(tuple);
    await Task.Delay(1000);

    i++;
}
```

#### Publish-Subscribe

The client can subscribe to space events by implementing the `ISpaceObserver` interface, and explicitly subscribing via the `ISpaceAgent`. In cases where observers have *partial* or *time-varying* interests on specific events, you can inherit from the `SpaceObserver` class (*which enables dynamic observation capabilities*).

>⚠️ By default, derived classes of `SpaceObserver` are configured to listen to all types of events.

>✅ `ISpaceAgent.Subscribe` is idempotent, so multiple invocations of it, will result in a *single* registration of the observer.

```cs
Guid id1 = agent.Subscribe(new Observer1());
Guid id2 = agent.Subscribe(new Observer2());
Guid id3 = agent.Subscribe(new Observer3());
Guid id4 = agent.Subscribe(new Observer4());

agent.Unsubscribe(id1);
agent.Unsubscribe(id2);
agent.Unsubscribe(id3);
agent.Unsubscribe(id4);

public class Observer1 : ISpaceObserver<SpaceTuple>
{
    public Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) 
    {
	// do something
        return Task.CompletedTask;
    }

    public Task OnContractionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
	// do something
        return Task.CompletedTask;
    }

    public Task OnFlatteningAsync(CancellationToken cancellationToken) =>    
    {
	// do something
        return Task.CompletedTask;
    }
}

public class Observer2 : SpaceObserver<SpaceTuple>
{
    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) 
    {
	// do something
        return Task.CompletedTask;
    }

    public override Task OnContractionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
	// do something
        return Task.CompletedTask;
    }

    public override Task OnFlatteningAsync(CancellationToken cancellationToken) =>    
    {
	// do something
        return Task.CompletedTask;
    }
}

public class Observer3 : SpaceObserver<SpaceTuple>
{
    public Observer3() => ListenTo(Expansions);
    
    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) 
    {
	// do something
        return Task.CompletedTask;
    }

    public override Task OnContractionAsync(SpaceTuple tuple, CancellationToken cancellationToken) => throw new NotImplementedException();
    public override Task OnFlatteningAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
}

public class Observer4 : SpaceObserver<SpaceTuple>
{
    public Observer4() => ListenTo(Expansions | Contractions);
    
    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) 
    {
	// do something
        return Task.CompletedTask;
    }

    public override Task OnContractionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
	// do something
        return Task.CompletedTask;
    }
        
    public override Task OnFlatteningAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
}
```

---

If you find it helpful, please consider giving it a ⭐ and share it!

Copyright © Ledjon Behluli
