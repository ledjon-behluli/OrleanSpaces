using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tests;

public class Class1
{
    [Fact]
    public void A()
    {
        Assert.Equal("()", new IntTuple2().ToString());
        Assert.Equal("(1)", new IntTuple2(1).ToString());
        Assert.Equal("(1, 2)", new IntTuple2(1, 2).ToString());
        Assert.Equal("(1, 2, 3)", new IntTuple2(1, 2, 3).ToString());
        Assert.Equal("(1, 2, 3, 4)", new IntTuple2(1, 2, 3, 4).ToString());
        Assert.Equal("(1, 2, 3, 4, 5)", new IntTuple2(1, 2, 3, 4, 5).ToString());
        Assert.Equal("(1, 2, 3, 4, 5, 6)", new IntTuple2(1, 2, 3, 4, 5, 6).ToString());
    }

    public readonly struct IntTuple2 : INumericSpaceTuple<int, IntTuple2>
    {
        private readonly int[] fields;

        public int this[int index] => fields[index];
        public int Length => fields.Length;

        Span<int> INumericSpaceTuple<int, IntTuple2>.Data => fields.AsSpan();

        public IntTuple2() : this(Array.Empty<int>()) { }
        public IntTuple2(params int[] fields) => this.fields = fields;

        public static bool operator ==(IntTuple2 left, IntTuple2 right) => left.Equals(right);
        public static bool operator !=(IntTuple2 left, IntTuple2 right) => !(left == right);

        public override bool Equals(object? obj) => obj is IntTuple2 tuple && Equals(tuple);
        public bool Equals(IntTuple2 other) => true;

        public int CompareTo(IntTuple2 other) => Length.CompareTo(other.Length);

        public override int GetHashCode() => fields.GetHashCode();

        public override string ToString()
        {
            IntTuple2 intTuple = new();

            if (Length == 0)
            {
                return "()";
            }

            if (Length == 1)
            {
                return $"({fields[0]})";
            }

            const int parenthesesLength = 2;

            int thisLength = Length;
            int separatorsCount = 2 * (thisLength - 1);
            int bufferLength = thisLength + separatorsCount + parenthesesLength; 
 
            return string.Create(bufferLength, fields, (buffer, state) => {

                buffer[0] = '(';  
                int i = 1;

                for (int j = 0; j < thisLength; j++)
                {
                    if (j > 0)
                    {
                        buffer[i++] = ',';
                        buffer[i++] = ' ';
                    }

                    ReadOnlySpan<char> span = state[j].ToString().AsSpan();
                    span.CopyTo(buffer.Slice(i, span.Length));

                    i += span.Length;
                }
                
                buffer[i] = ')';
            });
        }

        public void ToString(Span<char> destination, out int charsWritten)
        {
            throw new NotImplementedException();
        }
    }
}

