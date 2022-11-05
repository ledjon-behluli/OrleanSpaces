using OrleanSpaces;
using OrleanSpaces.Tuples;

namespace AnalyzersOffenders;

public class OSA004
{
	SpaceTemplate template = new(1);
	
	public async Task Test1(ISpaceAgent agent)
	{
        await agent.PeekAsync(template);
        await agent.PeekAsync(template, tuple => Task.CompletedTask);
	}

    public async Task Test2()
    {
        await GetAgent().PeekAsync(template);
        await GetAgent().PeekAsync(template, tuple => Task.CompletedTask);
    }

    public async Task Test3()
    {
        AgentFactory x = new();

        await x.Agent.PeekAsync(template);
        await x.Agent.PeekAsync(template, tuple => Task.CompletedTask);
    }

    private static ISpaceAgent GetAgent() => (ISpaceAgent)new object();

    class AgentFactory
    {
        public ISpaceAgent Agent => (ISpaceAgent)new object();
    }
}
