<p align="center">
  <img src="https://github.com/ledjon-behluli/OrleanSpaces/blob/master/OrleansLogo.png" alt="OrleanSpaces" width="200px"> 
  <h1>OrleanSpaces</h1>
</p>

[![CI](https://github.com/ledjon-behluli/OrleanSpaces/actions/workflows/ci.yml/badge.svg)](https://github.com/ledjon-behluli/OrleanSpaces/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/OrleanSpaces?color=blue)](https://www.nuget.org/packages/OrleanSpaces)
[![Coverage](https://coveralls.io/repos/github/ledjon-behluli/OrleanSpaces/badge.svg?branch=master)](https://coveralls.io/github/ledjon-behluli/OrleanSpaces?branch=master)
[![License](https://img.shields.io/github/license/ledjon-behluli/OrleanSpaces.svg)](https://github.com/ledjon-behluli/OrleanSpaces/blob/master/LICENSE.txt)

[Tuple space](https://en.wikipedia.org/wiki/Tuple_space) is an implementation of the associative memory paradigm for distributed computing. It provides a repository of tuples that can be accessed concurrently.

[Orleans](https://dotnet.github.io/orleans/docs/index.html) is a framework that provides a straight-forward approach to building distributed high-scale computing applications, without the need to learn and apply complex concurrency or other scaling patterns. 

**OrleanSpaces** is a package that brings the power of the tuple space programming model into the .NET world, by using Orleans as the backbone.

### Why virtual?
> Much the same way as actors in *Orleans*, the tuple space in *OrleanSpaces* can't be explicitly created or destroyed, it always exists, virtually! Its existence
transcends the lifetime of any of its in-memory instantiations, and thus the lifetime of any particular server. This alleviates the developer from a lot of ceremonial/infrastructural work that would have been neccessary.

### Why fully-asynchronous?
> The `IN` and `RD` operations in the tuple space paradigm, are inherently *blocking* operations. If there is no matching tuple found in the space, then the process which has called these operations, has to wait until it gets a matching tuple. This, intrinsically has an impact on the general availability of the whole system. In *OrleanSpaces* these operations are done in a fully-asynchronous way, via callback channels.

> *When I say that `IN` and `RD` are implemented in a fully-asynchronous way, I am not referring to the *non-blocking* versions `INP` and `RDP`! These are sepparate concepts, which by the way are also implemented in *OrleanSpaces*.*

# Motivation

While the tuple space paradigm offers unparalled computing capabilities, it is quite the feat to get it right! A proper implementation ideally should support capabilities like:

* Scalability
* Availability
* Resiliency
* Persistence
* Transactions
* Concurrency
* Location transparancy

Hobby implementations of the tuple space fail to deliver on a lot of those, and while enterprise solutions ([JavaSpaces](https://www.oracle.com/technical-resources/articles/javase/javaspaces.html), [GigaSpaces](https://www.gigaspaces.com/), IBM TSpaces, etc.) do a better job at it, they usually come with a price, and put the burden of managing the spaces' state to the developer.

All of the above-mentioned capabilities come out-of-the-box with Orleans. The idea was not to reinvent the wheel, but instead leverage Orleans, while providing an abstraction to the client, and build features upon it.

# Installation

Installation is performed via [NuGet](https://www.nuget.org/packages/OrleanSpaces/)

From Package Manager:

> PS> Install-Package OrleanSpaces

.Net CLI:

> \# dotnet add package OrleanSpaces

Paket:

> \# paket add OrleanSpaces

# Configuration
## Server
Configuration of the **Tuple Space Server** is done by configuring the **Orleans Silo** and calling the `AddTupleSpace()` extension method. Below we can see a typical configuration with *localhost clustering*, *in-memory persistence* and *simple message streaming*.

```cs
var host = new SiloHostBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
    .AddMemoryGrainStorage(Constants.PubSubStore)
    .AddMemoryGrainStorage(Constants.TupleSpaceStore)
    .AddTupleSpace()
    .Build();

await host.StartAsync();
```

## Client
Configuration of the **Tuple Space Client** is done by configuring the **Orleans Client** and calling the `AddTupleSpace()` extension method.
The client can be configured in two ways, depending on how you want to consume **OrleanSpaces**.

### Partial functionality

With this option you are skipping the setup of the `IHost`. *This is more of a testing scenario, as opposed to a real-world application!*

>⚠️ The equivalent functions of `IN`, `RD`, `EVAL` and the `Subscribe`/`Unsubscribe` methods, all will throw runtime exceptions! This comes with the intent of failing-fast in case of partial functionality configuration, while full functionality is intended to be used.

```cs
var client = new ClientBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
    .AddTupleSpace()
    .Build();

await client.Connect();   // If not called explicitly, it is handle by the library.
```

### Full functionality

With this option you are setting up the `IHost`. *This is more on the lines of a real-world application!*

```cs
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => services.AddTupleSpace())
    .Build();

await host.RunAsync();
```

The `AddTupleSpace` extension method on the `IServiceCollection`, accepts an optional `Func<IClusterClient>` delegate which you can provide to override the default client creation, which uses *localhost clustering*, *in-memory persistence* and *simple message streaming*.

```cs
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => 
        services.AddTupleSpace(() => 
        {
            IClusterClient client = ... // Build the cluster client based on your needs!
            return client;
        }))
    .Build();

await host.RunAsync();
```
# Usage

Below you can see some simple examples on how to use the **OrleanSpace** API. If you want to dive deeper, than a good amount of examples are available in the [samples](https://github.com/ledjon-behluli/OrleanSpaces/tree/master/samples) directory of the project, so go ahead and have fun with it 🙂.

## ISpaceAgentProvider
Get hold of the `ISpaceAgentProvider` which is made available through DI. The provider is registered as a singleton, so it is safe to inject it anywhere needed.
```cs
ISpaceAgentProvider provider = serviceProvider.GetRequiredService<ISpaceAgentProvider>();
```
## ISpaceAgent
The agent is the core component that the client can use to interact with the tuple space. It can be retrieved **only** through the `ISpaceAgentProvider`.
```cs
ISpaceAgent agent = await provider.GetAsync();
```

>❌ `ISpaceAgent` is **not** available through **DI**! It has to be retrieved through the **provider**.

>✅ `GetAsync` can **safely** be accessed in a **multi-threaded** environment.

>✅ There is only **one instance** of an agent running **per process**. This means that calling `GetAsync` multiple times, results in the same instance being returned. *It is better to call `GetAsync` multiple times, as opposed to calling it once in synchronous code like constructors!*

```cs
await agent.WriteAsync(new SpaceTuple(1, "a", 1.5f));
await agent.EvaluateAsync(async () =>
{
    await Task.Delay(100);  // Evaluating...
    return new SpaceTuple(2, "b", 2.5f);
});

SpaceTemplate template1 = new SpaceTemplate(1, typeof(string), typeof(float));

await agent.PeekAsync(template1);
await agent.PeekAsync(template1, async tuple =>
{
    // Doing something with the 'peeked' tuple.
    await Task.Delay(100);
});

await agent.PopAsync(template1);
await agent.PopAsync(template1, async tuple =>
{
    // Doing something with the 'poped' tuple.
    await Task.Delay(100);
});

SpaceTemplate template2 = new SpaceTemplate(1, typeof(string), SpaceUnit.Null);

IEnumerable<SpaceTuple> tuples = await agent.ScanAsync(template2);
int count = await agent.CountAsync(template2);
int totalCount = await agent.CountAsync();
```
## ISpaceObserver
The client can subscribe to space events by implementing the `ISpaceObserver` interface, and explicity subscribing via the `ISpaceAgent`. In cases where observers have *partial* or *time-varying* interests on specific events, you can inherit from the `SpaceObserver` class (*which enables dynamic observation capabilities*).

>⚠️ By default, derived classes of `SpaceObserver` are configured to **not** listen to any type of events.

>✅ `ISpaceAgent.Subscribe` is idempotant, so multiple invocations of it, will result in a *single* registration of the observer.

```cs
Guid id1 = agent.Subscribe(new Observer1());
Guid id2 = agent.Subscribe(new Observer2());
Guid id3 = agent.Subscribe(new Observer3());
Guid id4 = agent.Subscribe(new Observer4());

agent.Unsubscribe(id1);
agent.Unsubscribe(id2);
agent.Unsubscribe(id3);
agent.Unsubscribe(id4);

public class Observer1 : ISpaceObserver
{
    public Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) =>
        throw new NotImplementedException();

    public Task OnFlatteningAsync(CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}

public class Observer2 : SpaceObserver
{
    public Observer2() => ListenTo(Everything);
}

public class Observer3 : SpaceObserver
{
    public Observer3() => ListenTo(Expansions);
    
    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) =>
        base.OnExpansionAsync(tuple, cancellationToken);
}

public class Observer4 : SpaceObserver
{
    public Observer4() => ListenTo(Expansions | Contractions);
    
    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken) =>
        base.OnExpansionAsync(tuple, cancellationToken);
        
    public override Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken) =>
        base.OnContractionAsync(template, cancellationToken);
}
```
---

If you find it helpful, please consider giving it a ⭐ and share it!

Copyright © Ledjon Behluli
