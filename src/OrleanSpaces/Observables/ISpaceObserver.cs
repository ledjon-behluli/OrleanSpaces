using Orleans;

namespace OrleanSpaces.Observables;

public interface ISpaceObserver : IGrainObserver
{
    void OnExpansion();
    void OnContraction();
}
