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

        public Task<bool> TryReadAsync(ref SpaceTemplate template, CancellationToken cancellationToken)
        {
            IEnumerable<SpaceTuple> matching = space.State.Tuples.Where(x => x.Length == template.Length);
            if (!matching.Any())
            {
                return Task.FromResult(false);
            }
        }

        public Task<SpaceTuple> TakeAsync(SpaceTemplate template, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
       
        public Task<bool> TryTakeAsync(SpaceTemplate template, out SpaceTuple tuple, CancellationToken cancellationToken)
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

        private bool Match(object[] pattern, object[] tuple)
        {
            if (tuple.Length != pattern.Length)
            {
                return false;
            }
            bool result = true;
            for (int idx = 0; idx < tuple.Length; idx++)
            {
                if (pattern[idx] is Type)
                {
                    Type tupleType = tuple[idx] is Type ? (Type)tuple[idx] : tuple[idx].GetType();
                    result &= this.IsOfType(tupleType, (Type)pattern[idx]);
                }
                else
                {
                    result &= tuple[idx].Equals(pattern[idx]);
                }
                if (!result) return false;
            }

            return result;
        }
        private bool IsOfType(Type tupleType, Type patternType)
        {
            return tupleType == patternType;
        }
    }
}
