using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces;
using OrleanSpaces.Collections;
using OrleanSpaces.Tuples;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class TupleCollectionBenchmarks
{
    private static readonly Random random = new();

    private SpaceTuple[] tuples;
    private SpaceTemplate template;

    private readonly TupleCollection readCollection;
    private readonly WriteOptimizedCollection writeCollection;

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
            readCollection.Add(tuple);
            writeCollection.Add(tuple);
        }
    }

    [Benchmark]
    public void Add_ReadOptimized()
    {
        for (int i = 0; i < NumTuples; i++)
        {
            readCollection.Add(new SpaceTuple(new int[TupleSize]));
        }
    }

    [Benchmark]
    public void Add_WriteOptimized()
    {
        for (int i = 0; i < NumTuples; i++)
        {
            writeCollection.Add(new SpaceTuple(new int[TupleSize]));
        }
    }

    [Benchmark]
    public void Remove_ReadOptimized()
    {
        foreach (var tuple in tuples)
        {
            readCollection.Remove(tuple);
        }
    }

    [Benchmark]
    public void Remove_WriteOptimized()
    {
        foreach (var tuple in tuples)
        {
            writeCollection.Remove(tuple);
        }
    }

    [Benchmark]
    public void Find_ReadOptimized()
    {
        _ = readCollection.Find(template);
    }

    [Benchmark]
    public void Find_WriteOptimized()
    {
        _ = writeCollection.FirstOrDefault(template.Matches);
    }
}