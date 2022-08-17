using Orleans;

namespace OrleanSpaces;

public interface ISpaceObserver : IGrainObserver
{
    void OnExpansion();
    void OnContraction();
}
