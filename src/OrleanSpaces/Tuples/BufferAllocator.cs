using System.Buffers;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples;

internal static class BufferAllocator
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Execute<T>(int slots, IBufferConsumer<T> consumer)
       where T : unmanaged
    {
        if (slots * Unsafe.SizeOf<T>() <= 1024) // It is good practice not to allocate more than 1 kilobyte of memory on the stack
        {
            Span<T> buffer = stackalloc T[slots];
            return consumer.Consume(ref buffer);
        }
        else if (slots <= 1048576)  // 1,048,576 is the maximum array length of ArrayPool.Shared
        {
            T[] pooledArray = ArrayPool<T>.Shared.Rent(slots);
            Span<T> buffer = pooledArray.AsSpan();

            bool result = consumer.Consume(ref buffer);

            ArrayPool<T>.Shared.Return(pooledArray);

            return result;
        }
        else
        {
            T[] array = new T[slots];
            Span<T> buffer = array.AsSpan();

            return consumer.Consume(ref buffer);
        }
    }
}