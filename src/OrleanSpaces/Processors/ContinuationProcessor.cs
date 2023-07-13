using Microsoft.Extensions.Hosting;
using OrleanSpaces.Channels;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Processors;

internal sealed class ContinuationProcessor<TTuple, TTemplate> : BackgroundService
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly ContinuationChannel<TTuple, TTemplate> channel;
    private readonly ITupleRouter<TTuple, TTemplate> router;

    public ContinuationProcessor(
        ContinuationChannel<TTuple, TTemplate> channel,
        ITupleRouter<TTuple, TTemplate> router)
    {
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
        this.router = router ?? throw new ArgumentNullException(nameof(router));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var tupleTask = ConsumeTuples(cancellationToken);
        var templateTask = ConsumeTemplates(cancellationToken);

        await Task.WhenAll(tupleTask, templateTask);
    }

    private async Task ConsumeTuples(CancellationToken cancellationToken)
    {
        await foreach (TTuple tuple in channel.TupleReader.ReadAllAsync(cancellationToken))
        {
            await router.RouteAsync(tuple);
        }
    }

    private async Task ConsumeTemplates(CancellationToken cancellationToken)
    {
        await foreach (TTemplate template in channel.TemplateReader.ReadAllAsync(cancellationToken))
        {
            await router.RouteAsync(template);
        }
    }
}