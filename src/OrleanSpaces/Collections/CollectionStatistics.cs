using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Collections;

internal record struct CollectionStatistics(
    double TupleLengthMean, 
    ushort TupleLengthRelativeStdDev);

internal static class CollectionStatisticsCalculator
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CollectionStatistics Calculate<TTuple, TTemplate>(
        this ITupleCollection<TTuple, TTemplate> collection, 
        CollectionStatistics currentStatistics)
        where TTuple : ISpaceTuple
        where TTemplate : ISpaceTemplate
    {
        int tupleCount = collection.Count;
        ulong totalTupleLength = 0;
        double accumulatedSquaredDifference = 0;

        foreach (var tuple in collection)
        {
            int length = tuple.Length;

            totalTupleLength += (ulong)length;
            accumulatedSquaredDifference += 
                (length - currentStatistics.TupleLengthMean) * 
                (length - currentStatistics.TupleLengthMean);
        }

        double mean = tupleCount > 0 ? (double)totalTupleLength / tupleCount : 0;
        double stdDev = tupleCount > 0 ? Math.Sqrt(accumulatedSquaredDifference / tupleCount) : 0;
        ushort relativeStdDev = Convert.ToUInt16(mean > 0 ? 100 * (stdDev / mean) : 0); 

        return new(mean, relativeStdDev);
    }
}
