using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces;
using OrleanSpaces.Tuples;
using System.Collections.Immutable;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class TupleCollectionBenchmarks
{
    private static readonly Random random = new();

    private SpaceTuple[] tuples;
    private SpaceTemplate template;

    private TupleCollection collection;
    private ImmutableArray<SpaceTuple> immutableArray;

    [Params(10, 100, 1_000)]
    public int NumTuples { get; set; }

    [Params(1, 10, 30)] 
    public int TupleSize { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        tuples = new SpaceTuple[NumTuples];

        for (int i = 0; i < NumTuples; i++)
        {
            int[] values = new int[TupleSize];
            for (int j = 0; j < TupleSize; j++)
            {
                values[j] = random.Next();
            }

            tuples[i] = new SpaceTuple(values);
        }

        int[] templateValues = new int[TupleSize];
        for (int j = 0; j < TupleSize; j++)
        {
            templateValues[j] = random.Next();
        }

        template = new SpaceTemplate(templateValues);

        foreach (var tuple in tuples)
        {
            collection.Add(tuple);
        }

        immutableArray = ImmutableArray.Create(tuples);
    }

    [Benchmark]
    public void Add_TupleCollection()
    {
        for (int i = 0; i < NumTuples; i++)
        {
            collection.Add(new SpaceTuple(new int[TupleSize]));
        }
    }

    [Benchmark]
    public void Add_ImmutableArray()
    {
        for (int i = 0; i < NumTuples; i++)
        {
            immutableArray = immutableArray.Add(new SpaceTuple(new int[TupleSize]));
        }
    }

    [Benchmark]
    public void Remove_TupleCollection()
    {
        foreach (var tuple in tuples)
        {
            collection.Remove(tuple);
        }
    }

    [Benchmark]
    public void Remove_ImmutableArray()
    {
        foreach (var tuple in tuples)
        {
            immutableArray = immutableArray.Remove(tuple);
        }
    }

    [Benchmark]
    public void Find_TupleCollection()
    {
        _ = collection.Find(template);
    }

    [Benchmark]
    public void Find_ImmutableArray()
    {
        _ = immutableArray.FirstOrDefault(template.Matches);
    }
}