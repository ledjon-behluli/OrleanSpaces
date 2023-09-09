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
|[OSA001](https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA001.md)|Usage|Interface is intended for internal use only.|<span title='Warning'>⚠️</span>|❌|
|[OSA002](https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA002.md)|Performance|Avoid constructor instantiation having only `null` type, or no arguments.|<span title='Info'>ℹ️</span>|✔️|
|[OSA003](https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA003.md)|Usage|One or more of the supplied argument types for `SpaceTuple` or `SpaceTemplate` is not supported.|<span title='Error'>⛔</span>|✔️|
|[OSA004](https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA004.md)|Performance|Prefer using specialized over generic type.|<span title='Info'>ℹ️</span>|✔️|