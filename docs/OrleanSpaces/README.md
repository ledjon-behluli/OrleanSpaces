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
