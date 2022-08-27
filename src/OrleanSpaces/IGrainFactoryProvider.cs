using Orleans;

namespace OrleanSpaces;

internal interface IGrainFactoryProvider
{
    IGrainFactory GrainFactory { get; }
}
