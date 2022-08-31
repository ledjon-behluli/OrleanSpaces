using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrleanSpaces.Evaluations;

namespace OrleanSpaces.Tests;

public class ContainerFixture
{
    public IServiceProvider ServiceProvider { get; }

    public ContainerFixture()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddLogging();
        services.AddTupleSpace();

        ServiceProvider = services.BuildServiceProvider();
    }
}
