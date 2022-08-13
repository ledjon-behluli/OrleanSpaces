using Orleans.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

namespace OrleanSpaces
{
    public interface ITupleSpace
    {
        /// <summary>
        /// <para>Used to write a <see cref="SpaceTuple"/> in the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "OUT" primitive in the TupleSpace model.</i></para>
        /// </summary>
        Task PutAsync(SpaceTuple tuple, CancellationToken cancellationToken = default);

        /// <summary>
        /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "RDP" primitive in the TupleSpace model.</i></para>
        /// </summary>
        Task<SpaceTuple> ReadAsync(SpaceTemplate template, CancellationToken cancellationToken = default);

        /// <summary>
        /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "RD" primitive in the TupleSpace model.</i></para>
        /// </summary>
        Task<bool> TryReadAsync(ref SpaceTemplate template, CancellationToken cancellationToken = default);

        /// <summary>
        /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "INP" primitive in the TupleSpace model.</i></para>
        /// </summary>
        Task<SpaceTuple> TakeAsync(SpaceTemplate template, CancellationToken cancellationToken = default);

        /// <summary>
        /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "IN" primitive in the TupleSpace model.</i></para>
        /// </summary>
        Task<bool> TryTakeAsync(SpaceTemplate template, out SpaceTuple tuple, CancellationToken cancellationToken = default);

        Task<IEnumerable<SpaceTuple>> Scan(SpaceTemplate template, CancellationToken cancellationToken = default);
        Task<int> Count(SpaceTemplate template, CancellationToken cancellationToken = default);

        Task Eval();
    }
}
