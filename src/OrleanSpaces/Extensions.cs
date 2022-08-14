using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System;

namespace OrleanSpaces
{
    public static class Extensions
    {
        public static IServiceCollection AddTupleSpace(this IServiceCollection services)
        {
            services.AddSingleton(sp => (ITupleSpace)sp.GetRequiredService<IGrainFactory>().GetGrain(typeof(TupleSpaceGrain), Guid.Empty));
            return services;
        }
    }
}
