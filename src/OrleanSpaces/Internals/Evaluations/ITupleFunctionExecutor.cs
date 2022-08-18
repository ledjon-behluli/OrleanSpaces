using Orleans.Services;

namespace OrleanSpaces.Internals.Evaluations;

internal interface ITupleFunctionExecutor //: IGrainService
{
    Task ExecuteAsync(byte[] serializedFunction);
}

//public interface ITupleFunctionExecutorClient : IGrainServiceClient<ITupleFunctionExecutor>, ITupleFunctionExecutor
//{

//}