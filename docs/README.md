<p align="center">
  <img src="https://github.com/ledjon-behluli/OrleanSpaces/blob/master/OrleansLogo.png" alt="OrleanSpaces" width="200px"> 
  <h1>OrleanSpaces</h1>
</p>

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

# Packages

|Package|Description|Statuses|
|-|-|-|
| [OrleanSpaces](https://www.nuget.org/packages/OrleanSpaces) | Main library. | [![CI](https://github.com/ledjon-behluli/OrleanSpaces/actions/workflows/ci.yml/badge.svg)](https://github.com/ledjon-behluli/OrleanSpaces/actions/workflows/ci.yml) [![NuGet](https://img.shields.io/nuget/v/OrleanSpaces?color=blue)](https://www.nuget.org/packages/OrleanSpaces) [![Coverage](https://coveralls.io/repos/github/ledjon-behluli/OrleanSpaces/badge.svg?branch=master)](https://coveralls.io/github/ledjon-behluli/OrleanSpaces?branch=master) |
| [OrleanSpaces.Analyzers](https://www.nuget.org/packages/OrleanSpaces.Analyzers) | Code analysis and fixes for OrleanSpaces. | [![CI](https://github.com/ledjon-behluli/OrleanSpaces/actions/workflows/ci.yml/badge.svg)](https://github.com/ledjon-behluli/OrleanSpaces/actions/workflows/ci.yml) [![NuGet](https://img.shields.io/nuget/v/OrleanSpaces?color=blue)](https://www.nuget.org/packages/OrleanSpaces) [![Coverage](https://coveralls.io/repos/github/ledjon-behluli/OrleanSpaces/badge.svg?branch=master)](https://coveralls.io/github/ledjon-behluli/OrleanSpaces?branch=master) |

---

# Documentation

* [OrleanSpaces](https://github.com/ledjon-behluli/OrleanSpaces/docs/OrleanSpaces/README.md)
* [OrleanSpaces.Analyzers](https://github.com/ledjon-behluli/OrleanSpaces/docs/OrleanSpaces.Analyzers/README.md)

---

If you find it helpful, please consider giving it a ⭐ and share it!

Copyright © Ledjon Behluli
