using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleanSpaces
{
    public interface ITupleSpace
    {
        /// <summary>
        /// <para>Used to write a <see cref="SpaceTuple"/> in the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "OUT" primitive in the TupleSpace model.</i></para>
        /// </summary>
        Task Put(SpaceTuple tuple);

        /// <summary>
        /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "RDP" primitive in the TupleSpace model.</i></para>
        /// </summary>
        ValueTask<SpaceTuple> Peek(SpaceTemplate template);

        /// <summary>
        /// <para>Used to read a <see cref="SpaceTuple"/> from the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "RD" primitive in the TupleSpace model.</i></para>
        /// </summary>
        ValueTask<SpaceResult> TryPeek(SpaceTemplate template);

        /// <summary>
        /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "INP" primitive in the TupleSpace model.</i></para>
        /// </summary>
        Task<SpaceTuple> Read(SpaceTemplate template);

        /// <summary>
        /// <para>Used to read and remove a <see cref="SpaceTuple"/> from the <see cref="ITupleSpace"/>.</para>
        /// <para><i>Analogous to the "IN" primitive in the TupleSpace model.</i></para>
        /// </summary>
        Task<SpaceResult> TryRead(SpaceTemplate template);

        IEnumerable<SpaceTuple> Scan(SpaceTemplate template);

        ValueTask<int> Count();
        ValueTask<int> Count(SpaceTemplate template);

        Task Eval();
    }
}
