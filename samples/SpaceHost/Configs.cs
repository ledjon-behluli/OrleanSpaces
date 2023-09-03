using Orleans.Configuration;

public static class Configs
{
    private const string ConnectionString = "UseDevelopmentStorage=true";

    public static Action<SiloAzureQueueStreamConfigurator> QueueConfig = configurator =>
    {
        configurator.ConfigureAzureQueue(queueOptions => queueOptions.Configure(options =>
        {
            options.QueueNames = new List<string> { OrleanSpaces.Constants.StreamName };
            options.ConfigureQueueServiceClient(ConnectionString);
        }));
    };

    public static Action<AzureTableStorageOptions> TableConfig = options =>
    {
        options.TableName = OrleanSpaces.Constants.StorageName;
        options.ConfigureTableServiceClient(ConnectionString);
    };

    public static Action<AzureBlobStorageOptions> BlobConfig = options =>
    {
        options.ContainerName = OrleanSpaces.Constants.StorageName.ToLower(); // ToLower because of blob container naming rules
        options.ConfigureBlobServiceClient(ConnectionString);
    };

    /// <summary>
    /// More infos on ADO.NET: https://dotnet.github.io/orleans/docs/host/configuration_guide/adonet_configuration.html
    /// </summary>
    public static Action<AdoNetGrainStorageOptions> AdoNetConfig = options =>
    {
        options.Invariant = "Npgsql";
        options.ConnectionString = "Host=localhost;Database=OrleanSpaces;Username=postgres;Password=postgres";
    };
}