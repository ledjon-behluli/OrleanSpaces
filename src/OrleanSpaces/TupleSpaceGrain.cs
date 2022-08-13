using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace OrleanSpaces
{
    [Serializable]
    internal struct TupleSpaceState
    {
        public List<SpaceTuple> Tuples { get; set; }
    }

    internal sealed class TupleSpaceGrain : Grain, ITupleSpace
    {
        private readonly ILogger<TupleSpaceGrain> logger;
        private readonly IPersistentState<TupleSpaceState> space;

        public TupleSpaceGrain(
            ILogger<TupleSpaceGrain> logger,
            [PersistentState("tupleSpace", "tupleSpaceStore")] IPersistentState<TupleSpaceState> space)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.space = space ?? throw new ArgumentNullException(nameof(space));
        }

        public async Task PutAsync(SpaceTuple tuple, CancellationToken cancellationToken)
        {
            space.State.Tuples.Add(tuple);
            await space.WriteStateAsync();
        }

        public Task<SpaceTuple> ReadAsync(SpaceTemplate template, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<SpaceTuple> TakeAsync(SpaceTemplate template, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<SpaceResult> TryReadAsync(SpaceTemplate template, CancellationToken cancellationToken)
        {
            IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

            if (!tuples.Any())
            {
                return Task.FromResult(SpaceResult.Fail());
            }

            foreach (var tuple in tuples)
            {
                if (TupleMatcher.IsMatch(tuple, template))
                {
                    return Task.FromResult(SpaceResult.Success(tuple));
                }
            }

            return Task.FromResult(SpaceResult.Fail());
        }

        public Task<SpaceResult> TryTakeAsync(SpaceTemplate template, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<SpaceTuple>> Scan(SpaceTemplate template, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
        public Task<int> Count(SpaceTemplate template, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task Eval()
        {
            throw new System.NotImplementedException();
        }
    }
}
