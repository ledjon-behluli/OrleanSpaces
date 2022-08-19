using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace OrleanSpaces.Internals.Agents;

internal class SpaceAgentManager : ISpaceAgentRegistry, ISpaceAgentNotifier
{
    private readonly ILogger<SpaceAgentManager> logger;
    private readonly ConcurrentDictionary<ISpaceAgent, DateTime> agents;

    public SpaceAgentManager(ILogger<SpaceAgentManager> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.agents = new ConcurrentDictionary<ISpaceAgent, DateTime>();
    }

    public void Register(ISpaceAgent agent)
    {
        if (agents.TryAdd(agent, DateTime.UtcNow))
        {
            logger.LogInformation("Agent Registration - Current number of agents: {AgentCount}", agents.Count);
        }
    }

    public void Deregister(ISpaceAgent observer)
    {
        if (agents.TryRemove(observer, out _))
        {
            logger.LogInformation("Agent Deregistration - Current number of agents: {AgentCount}", agents.Count);
        }
    }

    public void Broadcast(Action<ISpaceAgent> action)
    {
        List<ISpaceAgent> defected = new();

        foreach (var agent in agents)
        {
            try
            {
                action(agent.Key);
            }
            catch (Exception)
            {
                defected ??= new List<ISpaceAgent>();
                defected.Add(agent.Key);
            }
        }

        if (defected.Count > 0)
        {
            foreach (var agent in defected)
            {
                Deregister(agent);
            }
        }
    }
}
