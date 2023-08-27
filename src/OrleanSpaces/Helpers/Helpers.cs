using System.Buffers;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Helpers;

internal static class Helpers
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TOut CastAs<TIn, TOut>(in TIn value)
        where TIn : struct => ref Unsafe.As<TIn, TOut>(ref Unsafe.AsRef(in value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSupportedType(this Type type) =>
       type.IsPrimitive ||
       type.IsEnum ||
       type == typeof(string) ||
       type == typeof(decimal) ||
       type == typeof(Int128) ||
       type == typeof(UInt128) ||
       type == typeof(DateTime) ||
       type == typeof(DateTimeOffset) ||
       type == typeof(TimeSpan) ||
       type == typeof(Guid);
   
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllocateAndExecute<T, TConsumer>(this TConsumer consumer, int capacity)
        where T : unmanaged
        where TConsumer : IBufferConsumer<T>
    {
        if (capacity * Unsafe.SizeOf<T>() <= 1024) // It is good practice not to allocate more than 1 kilobyte of memory on the stack
        {
            Span<T> buffer = stackalloc T[capacity];
            return consumer.Consume(ref buffer);
        }
        else
        {
            T[] array = ArrayPool<T>.Shared.Rent(capacity);
            Span<T> buffer = array.AsSpan();

            bool result = consumer.Consume(ref buffer);

            ArrayPool<T>.Shared.Return(array);

            return result;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task WriteIfNotNull<T>(this Channel<T>? channel, T tuple) 
        where T : ISpaceTuple
    {
        if (channel is not null)
        {
            await channel.Writer.WriteAsync(tuple);
        }
    }
}