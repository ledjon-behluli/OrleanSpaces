using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;
using OrleanSpaces.Core.Utils;

namespace OrleanSpaces.Hosts;

//internal class Interceptor : IIncomingGrainCallFilter
//{
//    private readonly IGrainFactory factory;
//    private readonly ObserverManager manager;

//    public Interceptor(
//        IGrainFactory factory,
//        ObserverManager manager)
//    {
//        this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
//        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
//    }

//    public async Task Invoke(IIncomingGrainCallContext context)
//    {
//        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceGrain.WriteAsync)))
//        {
//            await context.Invoke();
//            manager.Broadcast(agent => agent.Receive((SpaceTuple)context.Arguments[0]));

//            return;
//        }

//        if (string.Equals(context.InterfaceMethod.Name, nameof(ISpaceGrain.EvaluateAsync)))
//        {
//            Func<SpaceTuple> function = LambdaSerializer.Deserialize((byte[])context.Arguments[0]);
//            object result = function.DynamicInvoke();

//            if (result is SpaceTuple tuple)
//            {
//                await factory.GetSpaceGrain().WriteAsync(tuple);
//                manager.Broadcast(agent => agent.Receive(tuple));
//            }

//            return;
//        }

//        await context.Invoke();
//    }
//}
