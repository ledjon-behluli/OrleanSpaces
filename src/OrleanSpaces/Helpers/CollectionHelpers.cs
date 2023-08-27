using OrleanSpaces.Collections;
using OrleanSpaces.Tuples;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Helpers;

internal static class CollectionHelpers
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ITupleCollection<TTuple, TTemplate> Switch<TTuple, TTemplate>(
        this ITupleCollection<TTuple, TTemplate> collection,
        CollectionStatistics statistics,
        Func<ITupleCollection<TTuple, TTemplate>> readOptimizedCollectionFactory,
        Func<ITupleCollection<TTuple, TTemplate>> writeOptimizedCollectionFactory)

        where TTuple : ISpaceTuple
        where TTemplate : ISpaceTemplate
    {
        /*
           1) If the total number of tuples is small -
              Use WriteOptimizedCollection regardless of tuple lengths.
           
           2) If the total number of tuples is large -
              2.1) If the standard deviation of tuple lengths is high -
                   Use WriteOptimizedCollection since there's a wide variation in lengths, which makes it closer to having distinct dictionary keys.
           
              2.2) If the standard deviation of tuple lengths is low -
                 Use ReadOptimizedCollection since lengths are relatively uniform, and we can benefit from the dictionary's key-based filtering for better find performance. 
        */

        if (collection.Count < Constants.MinAgentCollectionCountThreshold)
        {
            if (collection is ReadOptimizedCollection)
            {
                return writeOptimizedCollectionFactory();
            }

            return collection;
        }

        if (collection is ReadOptimizedCollection &&
            statistics.TupleLengthRelativeStdDev > Constants.RelStdDevAgentMaxThreshold)
        {
            return writeOptimizedCollectionFactory();
        }

        if (collection is WriteOptimizedCollection &&
            statistics.TupleLengthRelativeStdDev <= Constants.RelStdDevAgentMinThreshold)
        {
            return readOptimizedCollectionFactory();
        }

        return collection;
    }
}
