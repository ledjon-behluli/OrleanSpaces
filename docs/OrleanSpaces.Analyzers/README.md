# Installation

Installation is performed via [NuGet](https://www.nuget.org/packages/OrleanSpaces.Analyzers/)

From Package Manager:

> PS> Install-Package OrleanSpaces.Analyzers

.Net CLI:

> \# dotnet add package OrleanSpaces.Analyzers

Paket:

> \# paket add OrleanSpaces.Analyzers

# Rules

|Id|Category|Description|Severity|Code Fix|
|:-:|:-:|-|:-:|:-:|
|[OSA001](https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA001.md)|Performance|Avoid instantiation of empty 'SpaceTuple' by default constructor or expression.|<span title='Info'>ℹ</span>|✔️|
|[OSA002](https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA002.md)|Performance|Avoid constructor instantiation of `SpaceTemplate` having only `SpaceUnit` type arguments.|<span title='Info'>ℹ</span>|✔️|
|[OSA003](https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA003.md)|Usage|One or more of the supplied argument types for `SpaceTuple` or `SpaceTemplate` is not supported.|<span title='Warning'>⚠️</span>|✔️|