using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OrleanSpaces;
using OrleanSpaces.Tuples;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore;

namespace WebApplication_5_0
{
    public class Program
    {
        SpaceTemplate template = new(1);

        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
               .ConfigureServices(services => { })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               }).Build().Run();

            WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => { })
               .UseStartup<Startup>()
               .Build()
               .Run();
        }

        public async Task Test1(ISpaceAgent agent)
        {
            await agent.PeekAsync(template);
            await agent.PopAsync(template);

            await agent.PeekAsync(template, _ => Task.CompletedTask);

            await agent.PeekAsync(template, tuple => Task.CompletedTask);
            await agent.PopAsync(template, tuple => Task.CompletedTask);

            await agent.EvaluateAsync(() => Task.FromResult(new SpaceTuple(1)));

            await agent.EvaluateAsync(async () => new(1));
        }

        public async Task Test2()
        {
            await GetAgent().PeekAsync(template);
            await GetAgent().PopAsync(template);

            await GetAgent().PeekAsync(template, tuple => Task.CompletedTask);
            await GetAgent().PopAsync(template, tuple => Task.CompletedTask);

            await GetAgent().EvaluateAsync(async () => new(1));
        }

        public async Task Test3()
        {
            AgentFactory x = new();

            // Field

            await x.agent.PeekAsync(template);
            await x.agent.PopAsync(template);

            await x.agent.PeekAsync(template, tuple => Task.CompletedTask);
            await x.agent.PopAsync(template, tuple => Task.CompletedTask);

            await x.agent.EvaluateAsync(async () => new(1));

            // Property

            await x.Agent.PeekAsync(template);
            await x.Agent.PopAsync(template);

            await x.Agent.PeekAsync(template, tuple => Task.CompletedTask);
            await x.Agent.PopAsync(template, tuple => Task.CompletedTask);

            await x.Agent.EvaluateAsync(async () => new(1));

            // Func

            await x.AgentProvider().PeekAsync(template);
            await x.AgentProvider().PopAsync(template);

            await x.AgentProvider().PeekAsync(template, tuple => Task.CompletedTask);
            await x.AgentProvider().PopAsync(template, tuple => Task.CompletedTask);

            await x.AgentProvider().EvaluateAsync(async () => new(1));

            // Method 

            await x.GetAgent().PeekAsync(template);
            await x.GetAgent().PopAsync(template);

            await x.GetAgent().PeekAsync(template, tuple => Task.CompletedTask);
            await x.GetAgent().PopAsync(template, tuple => Task.CompletedTask);

            await x.GetAgent().EvaluateAsync(async () => new(1));
        }

        private static ISpaceAgent GetAgent() => (ISpaceAgent)new object();

        class AgentFactory
        {
            public ISpaceAgent agent = (ISpaceAgent)new object();
            public ISpaceAgent Agent => (ISpaceAgent)new object();
            public Func<ISpaceAgent> AgentProvider => () => (ISpaceAgent)new object();
            public ISpaceAgent GetAgent() => (ISpaceAgent)new object();
        }
    }
}
