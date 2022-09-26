using OrleanSpaces.Tuples;
using System.Collections;

namespace OrleanSpaces.Tests;

public class TupleGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> data = new()
    {
        new object[] { new SpaceTuple(1) },
        new object[] { new SpaceTuple(1, "a") },
        new object[] { new SpaceTuple(1, "a", 1.5f) },
        new object[] { new SpaceTuple(1, "a", 1.5f, true) }
    };

    public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}