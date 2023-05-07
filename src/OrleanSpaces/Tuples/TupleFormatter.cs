namespace OrleanSpaces.Tuples;

internal struct TupleFormatter<T, TSelf> : IBufferConsumer<char>
    where T : struct, ISpanFormattable
    where TSelf : ISpaceTuple<T, TSelf>
{
    private readonly int maxFieldCharLength;
    private readonly ISpaceTuple<T, TSelf> tuple;

    private int charsWritten;

    public TupleFormatter(ISpaceTuple<T, TSelf> tuple, int maxFieldCharLength, ref int charsWritten)
    {
        this.tuple = tuple;
        this.maxFieldCharLength = maxFieldCharLength;
        this.charsWritten = charsWritten;
    }

    public bool Consume(ref Span<char> buffer)
    {   
        buffer.Clear();  // we dont know if the memory represented by the span might comes from the runtime and it may contain garbage values, so we clear it.

        charsWritten = 0;

        if (tuple.Length == 0)
        {
            buffer[charsWritten++] = '(';
            buffer[charsWritten++] = ')';

            return true;
        }

        // its safe to allocate memory on the stack because the maxFieldCharLength is a constant on all tuples,
        // and has a finite value: [2 bytes (since 'char') * maxFieldCharLength <= 1024 bytes]

        Span<char> fieldSpan = stackalloc char[maxFieldCharLength];

        buffer.Clear();
        fieldSpan.Clear();

        int tupleLength = tuple.Length;
        if (tupleLength == 1)
        {
            buffer[charsWritten++] = '(';

            if (!TryFormatField(in tuple[0], fieldSpan, buffer, ref charsWritten))
            {
                return false;
            }

            buffer[charsWritten++] = ')';

            return true;
        }

        buffer[charsWritten++] = '(';

        for (int i = 0; i < tupleLength; i++)
        {
            if (i > 0)
            {
                buffer[charsWritten++] = ',';
                buffer[charsWritten++] = ' ';

                fieldSpan.Clear();
            }

            if (!TryFormatField(in tuple[i], fieldSpan, buffer, ref charsWritten))
            {
                return false;
            }
        }

        buffer[charsWritten++] = ')';

        return true;
    }

    private static bool TryFormatField(in T field, Span<char> fieldSpan, Span<char> destination, ref int charsWritten)
    {
        if (!field.TryFormat(fieldSpan, out int fieldCharsWritten, default, null))
        {
            charsWritten = 0;
            return false;
        }

        fieldSpan[..fieldCharsWritten].CopyTo(destination.Slice(charsWritten, fieldCharsWritten));
        charsWritten += fieldCharsWritten;

        return true;
    }
}