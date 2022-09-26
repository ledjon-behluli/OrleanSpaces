using Orleans.Configuration;
using Orleans.Hosting;

public static class Configs
{
    public static Action<SiloAzureQueueStreamConfigurator> QueueConfig = configurator =>
    {
        configurator.ConfigureAzureQueue(queueOptions => queueOptions.Configure(options =>
        {
            options.QueueNames = new List<string> { "pub-sub" };
            options.ConfigureQueueServiceClient("UseDevelopmentStorage=true");
        }));
        configurator.ConfigureCacheSize(1024);
        configurator.ConfigurePullingAgent(ob => ob.Configure(options =>
        {
            options.GetQueueMsgsTimerPeriod = TimeSpan.FromMilliseconds(200);
        }));
    };

    public static Action<AzureTableStorageOptions> TableConfig = options =>
    {
        options.UseJson = true;
        options.ConfigureTableServiceClient("UseDevelopmentStorage=true");
    };

    public static Action<AzureBlobStorageOptions> BlobConfig = options =>
    {
        options.UseJson = true;
        options.ConfigureBlobServiceClient("UseDevelopmentStorage=true");
    };

    /// <summary>
    /// More infos on ADO.NET: https://dotnet.github.io/orleans/docs/host/configuration_guide/adonet_configuration.html
    /// </summary>
    public static Action<AdoNetGrainStorageOptions> AdoNetConfig = options =>
    {
        options.UseJsonFormat = true;
        options.Invariant = "Npgsql";
        options.ConnectionString = "Host=localhost;Database=OrleanSpaces;Username=postgres;Password=postgres";
    };
}