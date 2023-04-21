using Newtonsoft.Json.Linq;
using Orleans.Concurrency;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System;

namespace OrleanSpaces.Tests;


public class Class1
{
    [Fact]
    public void A()
    {
        

        //Assert.Equal("(1, 1, 1, 1)", new IntTuple(1, 1, 1, 1).ToString());
        Assert.Equal("(1, 1, 1, 1)", new TestTuple(1, 1, 1, 1).ToString());
    }

   

    [Immutable]
    public readonly struct TestTuple : INumericSpaceTuple<int, TestTuple>
    {
        private readonly int[] fields;

        public int this[int index] => fields[index];
        public int Length => fields.Length;

        Span<int> INumericSpaceTuple<int, TestTuple>.Fields => fields.AsSpan();

        public int MaxCharsWrittable => 10;
        public void WriteTo(int index, Span<char> destination, out int charsWritten) => fields[index].TryFormat(destination, out charsWritten);

        public TestTuple() : this(Array.Empty<int>()) { }
        public TestTuple(params int[] fields) => this.fields = fields;

        public static bool operator ==(TestTuple left, TestTuple right) => left.Equals(right);
        public static bool operator !=(TestTuple left, TestTuple right) => !(left == right);

        public override bool Equals(object obj) => obj is TestTuple tuple && Equals(tuple);
        public bool Equals(TestTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

        public int CompareTo(TestTuple other) => Length.CompareTo(other.Length);

        public override int GetHashCode() => fields.GetHashCode();

        public override string ToString()
        {
            int length = Length;
            if (length == 0)
            {
                return "()";
            }

            if (length == 1)
            {
                return $"({fields[0]})";
            }

            int separatorsCount = 2 * (length - 1);
            int bufferLength = length + separatorsCount + 2;
            int maxCharsWrittable = MaxCharsWrittable;

            return string.Create(bufferLength, this, (buffer, state) =>
            {
                buffer[0] = '(';
                int i = 1;

                Span<char> span = stackalloc char[maxCharsWrittable];

                for (int j = 0; j < length; j++)
                {
                    if (j > 0)
                    {
                        buffer[i++] = ',';
                        buffer[i++] = ' ';
                    }

                    //state[j].TryFormat(span, out int charsWritten);

                    state.WriteTo(j, span, out int charsWritten);
                    span.CopyTo(buffer.Slice(i, charsWritten));
                    i += charsWritten;
                }

                buffer[i] = ')';
            });
        }
    }
}

