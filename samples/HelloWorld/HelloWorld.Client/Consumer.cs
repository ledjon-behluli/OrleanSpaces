using OrleanSpaces.Clients;

public class Consumer
{
    private readonly ISpaceClient client;

    public Consumer(ISpaceClient client)
    {
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<int> GetTotalTupleCount()
    {
        return await client.CountAsync();
    }
}