using Orleans.Configuration;

public static class Configs
{
    public static Action<SiloAzureQueueStreamConfigurator> QueueConfig = configurator =>
    {
        configurator.ConfigureAzureQueue(queueOptions => queueOptions.Configure(options =>
        {
            options.QueueNames = new List<string> { OrleanSpaces.Constants.Store_StreamNamespace };
            options.ConfigureQueueServiceClient("UseDevelopmentStorage=true");
        }));
    };

    public static Action<AzureTableStorageOptions> TableConfig = options =>
    {
        options.TableName = OrleanSpaces.Constants.Store_StorageName;
        options.ConfigureTableServiceClient("UseDevelopmentStorage=true");
    };

    public static Action<AzureBlobStorageOptions> BlobConfig = options =>
    {
        options.ContainerName = OrleanSpaces.Constants.Store_StorageName.ToLower(); // ToLower because of blob container naming rules
        options.ConfigureBlobServiceClient("UseDevelopmentStorage=true");
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