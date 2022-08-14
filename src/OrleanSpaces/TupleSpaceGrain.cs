using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task Put(SpaceTuple tuple)
        {
            space.State.Tuples.Add(tuple);
            await space.WriteStateAsync();
        }

        public SpaceTuple Read(SpaceTemplate template)
        {
            throw new System.NotImplementedException();
        }

        public SpaceResult TryRead(SpaceTemplate template)
        {
            IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

            foreach (var tuple in tuples)
            {
                if (TupleMatcher.IsMatch(tuple, template))
                {
                    return SpaceResult.Success(tuple);
                }
            }

            return SpaceResult.Fail();
        }

        public Task<SpaceTuple> Take(SpaceTemplate template)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SpaceResult> TryTake(SpaceTemplate template)
        {
            IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

            foreach (var tuple in tuples)
            {
                if (TupleMatcher.IsMatch(tuple, template))
                {
                    space.State.Tuples.Remove(tuple);
                    await space.WriteStateAsync();

                    return SpaceResult.Success(tuple);
                }
            }

            return SpaceResult.Fail();
        }

        public IEnumerable<SpaceTuple> Scan(SpaceTemplate template = default)
        {
            IEnumerable<SpaceTuple> tuples = space.State.Tuples.Where(x => x.Length == template.Length);

            foreach (var tuple in tuples)
            {
                if (TupleMatcher.IsMatch(tuple, template))
                {
                    yield return tuple;
                }
            }
        }

        public int Count() => space.State.Tuples.Count;

        public int Count(SpaceTemplate template) => space.State.Tuples
            .Count(sp => sp.Length == template.Length && TupleMatcher.IsMatch(sp, template));

        public Task Eval()
        {
            throw new System.NotImplementedException();
        }
    }
}
