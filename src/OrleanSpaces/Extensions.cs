using OrleanSpaces.Agents;
using OrleanSpaces.Tuples;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Processors;
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Processors.Spaces;

namespace OrleanSpaces;

public static class Extensions
{
    /// <summary>
    /// Configures the tuple space on the host.
    /// </summary>
    /// <param name="builder">The orleans silo builder.</param>
    /// <param name="configureServerOptions">An optional delegate to configure the <see cref="SpaceServerOptions"/></param>
    /// <param name="configureClientOptions">An optional delegate to configure the <see cref="SpaceClientOptions"/></param>
    public static ISiloBuilder AddOrleanSpaces(
        this ISiloBuilder builder,
        Action<SpaceServerOptions>? configureServerOptions = null,
        Action<SpaceClientOptions>? configureClientOptions = null)
    {
        SpaceServerOptions serverOptions = new();
        configureServerOptions?.Invoke(serverOptions);
        if (serverOptions.PartitioningThreshold < 1)
        {
            throw new InvalidOperationException("Partition threshold must be greater than zero.");
        }
        
        builder.Services.AddSingleton(serverOptions);

        if (configureClientOptions != null)
        {
            SpaceClientOptions clientOptions = new();
            configureClientOptions?.Invoke(clientOptions);
            
            builder.Services.AddSingleton(clientOptions);
            builder.Services.AddClientServices(clientOptions.EnabledSpaces);
        }

        return builder;
    }

    /// <summary>
    /// Configures the tuple space on the client.
    /// </summary>
    /// <param name="builder">The orleans client builder.</param>
    /// <param name="configureOptions">An optional delegate to configure the <see cref="SpaceClientOptions"/></param>
    public static IClientBuilder AddOrleanSpaces(this IClientBuilder builder, Action<SpaceClientOptions>? configureOptions = null)
    {
        SpaceClientOptions options = new() { EnabledSpaces = SpaceKind.Generic };
        configureOptions?.Invoke(options);

        builder.Services.AddSingleton(options);
        builder.Services.AddClientServices(options.EnabledSpaces);

        return builder;
    }

    private static void AddClientServices(this IServiceCollection services, SpaceKind spaceKind)
    {
        if (spaceKind.HasFlag(SpaceKind.Generic))
        {
            services.AddSingleton<ObserverRegistry<SpaceTuple>>();
            services.AddSingleton<CallbackRegistry>();

            services.AddSingleton<EvaluationChannel<SpaceTuple>>();
            services.AddSingleton<ObserverChannel<SpaceTuple>>();
            services.AddSingleton<CallbackChannel<SpaceTuple>>();
            services.AddSingleton<ContinuationChannel<SpaceTuple, SpaceTemplate>>();

            services.AddSingleton<SpaceAgent>();
            services.AddSingleton<ISpaceAgent>(sp => sp.GetRequiredService<SpaceAgent>());
            services.AddSingleton<ISpaceRouter<SpaceTuple, SpaceTemplate>>(sp => sp.GetRequiredService<SpaceAgent>());

            services.AddHostedService<SpaceProcessor>();
            services.AddHostedService<ObserverProcessor<SpaceTuple>>();
            services.AddHostedService<CallbackProcessor>();
            services.AddHostedService<EvaluationProcessor<SpaceTuple, SpaceTemplate>>();
            services.AddHostedService<ContinuationProcessor<SpaceTuple, SpaceTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Bool))
        {
            services.AddSingleton<ObserverRegistry<BoolTuple>>();
            services.AddSingleton<CallbackRegistry<bool, BoolTuple, BoolTemplate>>();

            services.AddSingleton<EvaluationChannel<BoolTuple>>();
            services.AddSingleton<ObserverChannel<BoolTuple>>();
            services.AddSingleton<CallbackChannel<BoolTuple>>();
            services.AddSingleton<ContinuationChannel<BoolTuple, BoolTemplate>>();

            services.AddSingleton<BoolAgent>();
            services.AddSingleton<ISpaceAgent<bool, BoolTuple, BoolTemplate>>(sp => sp.GetRequiredService<BoolAgent>());
            services.AddSingleton<ISpaceRouter<BoolTuple, BoolTemplate>>(sp => sp.GetRequiredService<BoolAgent>());

            services.AddHostedService<BoolProcessor>();
            services.AddHostedService<ObserverProcessor<BoolTuple>>();
            services.AddHostedService<CallbackProcessor<bool, BoolTuple, BoolTemplate>>();
            services.AddHostedService<EvaluationProcessor<BoolTuple, BoolTemplate>>();
            services.AddHostedService<ContinuationProcessor<BoolTuple, BoolTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Byte))
        {
            services.AddSingleton<ObserverRegistry<ByteTuple>>();
            services.AddSingleton<CallbackRegistry<byte, ByteTuple, ByteTemplate>>();

            services.AddSingleton<EvaluationChannel<ByteTuple>>();
            services.AddSingleton<ObserverChannel<ByteTuple>>();
            services.AddSingleton<CallbackChannel<ByteTuple>>();
            services.AddSingleton<ContinuationChannel<ByteTuple, ByteTemplate>>();

            services.AddSingleton<ByteAgent>();
            services.AddSingleton<ISpaceAgent<byte, ByteTuple, ByteTemplate>>(sp => sp.GetRequiredService<ByteAgent>());
            services.AddSingleton<ISpaceRouter<ByteTuple, ByteTemplate>>(sp => sp.GetRequiredService<ByteAgent>());

            services.AddHostedService<ByteProcessor>();
            services.AddHostedService<ObserverProcessor<ByteTuple>>();
            services.AddHostedService<CallbackProcessor<byte, ByteTuple, ByteTemplate>>();
            services.AddHostedService<EvaluationProcessor<ByteTuple, ByteTemplate>>();
            services.AddHostedService<ContinuationProcessor<ByteTuple, ByteTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Char))
        {
            services.AddSingleton<ObserverRegistry<CharTuple>>();
            services.AddSingleton<CallbackRegistry<char, CharTuple, CharTemplate>>();

            services.AddSingleton<EvaluationChannel<CharTuple>>();
            services.AddSingleton<ObserverChannel<CharTuple>>();
            services.AddSingleton<CallbackChannel<CharTuple>>();
            services.AddSingleton<ContinuationChannel<CharTuple, CharTemplate>>();

            services.AddSingleton<CharAgent>();
            services.AddSingleton<ISpaceAgent<char, CharTuple, CharTemplate>>(sp => sp.GetRequiredService<CharAgent>());
            services.AddSingleton<ISpaceRouter<CharTuple, CharTemplate>>(sp => sp.GetRequiredService<CharAgent>());

            services.AddHostedService<CharProcessor>();
            services.AddHostedService<ObserverProcessor<CharTuple>>();
            services.AddHostedService<CallbackProcessor<char, CharTuple, CharTemplate>>();
            services.AddHostedService<EvaluationProcessor<CharTuple, CharTemplate>>();
            services.AddHostedService<ContinuationProcessor<CharTuple, CharTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.DateTimeOffset))
        {
            services.AddSingleton<ObserverRegistry<DateTimeOffsetTuple>>();
            services.AddSingleton<CallbackRegistry<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>();

            services.AddSingleton<EvaluationChannel<DateTimeOffsetTuple>>();
            services.AddSingleton<ObserverChannel<DateTimeOffsetTuple>>();
            services.AddSingleton<CallbackChannel<DateTimeOffsetTuple>>();
            services.AddSingleton<ContinuationChannel<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();

            services.AddSingleton<DateTimeOffsetAgent>();
            services.AddSingleton<ISpaceAgent<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>(sp => sp.GetRequiredService<DateTimeOffsetAgent>());
            services.AddSingleton<ISpaceRouter<DateTimeOffsetTuple, DateTimeOffsetTemplate>>(sp => sp.GetRequiredService<DateTimeOffsetAgent>());

            services.AddHostedService<DateTimeOffsetProcessor>();
            services.AddHostedService<ObserverProcessor<DateTimeOffsetTuple>>();
            services.AddHostedService<CallbackProcessor<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
            services.AddHostedService<EvaluationProcessor<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
            services.AddHostedService<ContinuationProcessor<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.DateTime))
        {
            services.AddSingleton<ObserverRegistry<DateTimeTuple>>();
            services.AddSingleton<CallbackRegistry<DateTime, DateTimeTuple, DateTimeTemplate>>();

            services.AddSingleton<EvaluationChannel<DateTimeTuple>>();
            services.AddSingleton<ObserverChannel<DateTimeTuple>>();
            services.AddSingleton<CallbackChannel<DateTimeTuple>>();
            services.AddSingleton<ContinuationChannel<DateTimeTuple, DateTimeTemplate>>();

            services.AddSingleton<DateTimeAgent>();
            services.AddSingleton<ISpaceAgent<DateTime, DateTimeTuple, DateTimeTemplate>>(sp => sp.GetRequiredService<DateTimeAgent>());
            services.AddSingleton<ISpaceRouter<DateTimeTuple, DateTimeTemplate>>(sp => sp.GetRequiredService<DateTimeAgent>());

            services.AddHostedService<DateTimeProcessor>();
            services.AddHostedService<ObserverProcessor<DateTimeTuple>>();
            services.AddHostedService<CallbackProcessor<DateTime, DateTimeTuple, DateTimeTemplate>>();
            services.AddHostedService<EvaluationProcessor<DateTimeTuple, DateTimeTemplate>>();
            services.AddHostedService<ContinuationProcessor<DateTimeTuple, DateTimeTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Decimal))
        {
            services.AddSingleton<ObserverRegistry<DecimalTuple>>();
            services.AddSingleton<CallbackRegistry<decimal, DecimalTuple, DecimalTemplate>>();

            services.AddSingleton<EvaluationChannel<DecimalTuple>>();
            services.AddSingleton<ObserverChannel<DecimalTuple>>();
            services.AddSingleton<CallbackChannel<DecimalTuple>>();
            services.AddSingleton<ContinuationChannel<DecimalTuple, DecimalTemplate>>();

            services.AddSingleton<DecimalAgent>();
            services.AddSingleton<ISpaceAgent<decimal, DecimalTuple, DecimalTemplate>>(sp => sp.GetRequiredService<DecimalAgent>());
            services.AddSingleton<ISpaceRouter<DecimalTuple, DecimalTemplate>>(sp => sp.GetRequiredService<DecimalAgent>());

            services.AddHostedService<DecimalProcessor>();
            services.AddHostedService<ObserverProcessor<DecimalTuple>>();
            services.AddHostedService<CallbackProcessor<decimal, DecimalTuple, DecimalTemplate>>();
            services.AddHostedService<EvaluationProcessor<DecimalTuple, DecimalTemplate>>();
            services.AddHostedService<ContinuationProcessor<DecimalTuple, DecimalTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Double))
        {
            services.AddSingleton<ObserverRegistry<DoubleTuple>>();
            services.AddSingleton<CallbackRegistry<double, DoubleTuple, DoubleTemplate>>();

            services.AddSingleton<EvaluationChannel<DoubleTuple>>();
            services.AddSingleton<ObserverChannel<DoubleTuple>>();
            services.AddSingleton<CallbackChannel<DoubleTuple>>();
            services.AddSingleton<ContinuationChannel<DoubleTuple, DoubleTemplate>>();

            services.AddSingleton<DoubleAgent>();
            services.AddSingleton<ISpaceAgent<double, DoubleTuple, DoubleTemplate>>(sp => sp.GetRequiredService<DoubleAgent>());
            services.AddSingleton<ISpaceRouter<DoubleTuple, DoubleTemplate>>(sp => sp.GetRequiredService<DoubleAgent>());

            services.AddHostedService<DoubleProcessor>();
            services.AddHostedService<ObserverProcessor<DoubleTuple>>();
            services.AddHostedService<CallbackProcessor<double, DoubleTuple, DoubleTemplate>>();
            services.AddHostedService<EvaluationProcessor<DoubleTuple, DoubleTemplate>>();
            services.AddHostedService<ContinuationProcessor<DoubleTuple, DoubleTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Float))
        {
            services.AddSingleton<ObserverRegistry<FloatTuple>>();
            services.AddSingleton<CallbackRegistry<float, FloatTuple, FloatTemplate>>();

            services.AddSingleton<EvaluationChannel<FloatTuple>>();
            services.AddSingleton<ObserverChannel<FloatTuple>>();
            services.AddSingleton<CallbackChannel<FloatTuple>>();
            services.AddSingleton<ContinuationChannel<FloatTuple, FloatTemplate>>();

            services.AddSingleton<FloatAgent>();
            services.AddSingleton<ISpaceAgent<float, FloatTuple, FloatTemplate>>(sp => sp.GetRequiredService<FloatAgent>());
            services.AddSingleton<ISpaceRouter<FloatTuple, FloatTemplate>>(sp => sp.GetRequiredService<FloatAgent>());

            services.AddHostedService<FloatProcessor>();
            services.AddHostedService<ObserverProcessor<FloatTuple>>();
            services.AddHostedService<CallbackProcessor<float, FloatTuple, FloatTemplate>>();
            services.AddHostedService<EvaluationProcessor<FloatTuple, FloatTemplate>>();
            services.AddHostedService<ContinuationProcessor<FloatTuple, FloatTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Guid))
        {
            services.AddSingleton<ObserverRegistry<GuidTuple>>();
            services.AddSingleton<CallbackRegistry<Guid, GuidTuple, GuidTemplate>>();

            services.AddSingleton<EvaluationChannel<GuidTuple>>();
            services.AddSingleton<ObserverChannel<GuidTuple>>();
            services.AddSingleton<CallbackChannel<GuidTuple>>();
            services.AddSingleton<ContinuationChannel<GuidTuple, GuidTemplate>>();

            services.AddSingleton<GuidAgent>();
            services.AddSingleton<ISpaceAgent<Guid, GuidTuple, GuidTemplate>>(sp => sp.GetRequiredService<GuidAgent>());
            services.AddSingleton<ISpaceRouter<GuidTuple, GuidTemplate>>(sp => sp.GetRequiredService<GuidAgent>());

            services.AddHostedService<GuidProcessor>();
            services.AddHostedService<ObserverProcessor<GuidTuple>>();
            services.AddHostedService<CallbackProcessor<Guid, GuidTuple, GuidTemplate>>();
            services.AddHostedService<EvaluationProcessor<GuidTuple, GuidTemplate>>();
            services.AddHostedService<ContinuationProcessor<GuidTuple, GuidTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Huge))
        {
            services.AddSingleton<ObserverRegistry<HugeTuple>>();
            services.AddSingleton<CallbackRegistry<Int128, HugeTuple, HugeTemplate>>();

            services.AddSingleton<EvaluationChannel<HugeTuple>>();
            services.AddSingleton<ObserverChannel<HugeTuple>>();
            services.AddSingleton<CallbackChannel<HugeTuple>>();
            services.AddSingleton<ContinuationChannel<HugeTuple, HugeTemplate>>();

            services.AddSingleton<HugeAgent>();
            services.AddSingleton<ISpaceAgent<Int128, HugeTuple, HugeTemplate>>(sp => sp.GetRequiredService<HugeAgent>());
            services.AddSingleton<ISpaceRouter<HugeTuple, HugeTemplate>>(sp => sp.GetRequiredService<HugeAgent>());

            services.AddHostedService<HugeProcessor>();
            services.AddHostedService<ObserverProcessor<HugeTuple>>();
            services.AddHostedService<CallbackProcessor<Int128, HugeTuple, HugeTemplate>>();
            services.AddHostedService<EvaluationProcessor<HugeTuple, HugeTemplate>>();
            services.AddHostedService<ContinuationProcessor<HugeTuple, HugeTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Int))
        {
            services.AddSingleton<ObserverRegistry<IntTuple>>();
            services.AddSingleton<CallbackRegistry<int, IntTuple, IntTemplate>>();

            services.AddSingleton<EvaluationChannel<IntTuple>>();
            services.AddSingleton<ObserverChannel<IntTuple>>();
            services.AddSingleton<CallbackChannel<IntTuple>>();
            services.AddSingleton<ContinuationChannel<IntTuple, IntTemplate>>();

            services.AddSingleton<IntAgent>();
            services.AddSingleton<ISpaceAgent<int, IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());
            services.AddSingleton<ISpaceRouter<IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());

            services.AddHostedService<IntProcessor>();
            services.AddHostedService<ObserverProcessor<IntTuple>>();
            services.AddHostedService<CallbackProcessor<int, IntTuple, IntTemplate>>();
            services.AddHostedService<EvaluationProcessor<IntTuple, IntTemplate>>();
            services.AddHostedService<ContinuationProcessor<IntTuple, IntTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Long))
        {
            services.AddSingleton<ObserverRegistry<LongTuple>>();
            services.AddSingleton<CallbackRegistry<long, LongTuple, LongTemplate>>();

            services.AddSingleton<EvaluationChannel<LongTuple>>();
            services.AddSingleton<ObserverChannel<LongTuple>>();
            services.AddSingleton<CallbackChannel<LongTuple>>();
            services.AddSingleton<ContinuationChannel<LongTuple, LongTemplate>>();

            services.AddSingleton<LongAgent>();
            services.AddSingleton<ISpaceAgent<long, LongTuple, LongTemplate>>(sp => sp.GetRequiredService<LongAgent>());
            services.AddSingleton<ISpaceRouter<LongTuple, LongTemplate>>(sp => sp.GetRequiredService<LongAgent>());

            services.AddHostedService<LongProcessor>();
            services.AddHostedService<ObserverProcessor<LongTuple>>();
            services.AddHostedService<CallbackProcessor<long, LongTuple, LongTemplate>>();
            services.AddHostedService<EvaluationProcessor<LongTuple, LongTemplate>>();
            services.AddHostedService<ContinuationProcessor<LongTuple, LongTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.SByte))
        {
            services.AddSingleton<ObserverRegistry<SByteTuple>>();
            services.AddSingleton<CallbackRegistry<sbyte, SByteTuple, SByteTemplate>>();

            services.AddSingleton<EvaluationChannel<SByteTuple>>();
            services.AddSingleton<ObserverChannel<SByteTuple>>();
            services.AddSingleton<CallbackChannel<SByteTuple>>();
            services.AddSingleton<ContinuationChannel<SByteTuple, SByteTemplate>>();

            services.AddSingleton<SByteAgent>();
            services.AddSingleton<ISpaceAgent<sbyte, SByteTuple, SByteTemplate>>(sp => sp.GetRequiredService<SByteAgent>());
            services.AddSingleton<ISpaceRouter<SByteTuple, SByteTemplate>>(sp => sp.GetRequiredService<SByteAgent>());

            services.AddHostedService<SByteProcessor>();
            services.AddHostedService<ObserverProcessor<SByteTuple>>();
            services.AddHostedService<CallbackProcessor<sbyte, SByteTuple, SByteTemplate>>();
            services.AddHostedService<EvaluationProcessor<SByteTuple, SByteTemplate>>();
            services.AddHostedService<ContinuationProcessor<SByteTuple, SByteTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.Short))
        {
            services.AddSingleton<ObserverRegistry<ShortTuple>>();
            services.AddSingleton<CallbackRegistry<short, ShortTuple, ShortTemplate>>();

            services.AddSingleton<EvaluationChannel<ShortTuple>>();
            services.AddSingleton<ObserverChannel<ShortTuple>>();
            services.AddSingleton<CallbackChannel<ShortTuple>>();
            services.AddSingleton<ContinuationChannel<ShortTuple, ShortTemplate>>();

            services.AddSingleton<ShortAgent>();
            services.AddSingleton<ISpaceAgent<short, ShortTuple, ShortTemplate>>(sp => sp.GetRequiredService<ShortAgent>());
            services.AddSingleton<ISpaceRouter<ShortTuple, ShortTemplate>>(sp => sp.GetRequiredService<ShortAgent>());

            services.AddHostedService<ShortProcessor>();
            services.AddHostedService<ObserverProcessor<ShortTuple>>();
            services.AddHostedService<CallbackProcessor<short, ShortTuple, ShortTemplate>>();
            services.AddHostedService<EvaluationProcessor<ShortTuple, ShortTemplate>>();
            services.AddHostedService<ContinuationProcessor<ShortTuple, ShortTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.TimeSpan))
        {
            services.AddSingleton<ObserverRegistry<TimeSpanTuple>>();
            services.AddSingleton<CallbackRegistry<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>();

            services.AddSingleton<EvaluationChannel<TimeSpanTuple>>();
            services.AddSingleton<ObserverChannel<TimeSpanTuple>>();
            services.AddSingleton<CallbackChannel<TimeSpanTuple>>();
            services.AddSingleton<ContinuationChannel<TimeSpanTuple, TimeSpanTemplate>>();

            services.AddSingleton<TimeSpanAgent>();
            services.AddSingleton<ISpaceAgent<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>(sp => sp.GetRequiredService<TimeSpanAgent>());
            services.AddSingleton<ISpaceRouter<TimeSpanTuple, TimeSpanTemplate>>(sp => sp.GetRequiredService<TimeSpanAgent>());

            services.AddHostedService<TimeSpanProcessor>();
            services.AddHostedService<ObserverProcessor<TimeSpanTuple>>();
            services.AddHostedService<CallbackProcessor<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>();
            services.AddHostedService<EvaluationProcessor<TimeSpanTuple, TimeSpanTemplate>>();
            services.AddHostedService<ContinuationProcessor<TimeSpanTuple, TimeSpanTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.UHuge))
        {
            services.AddSingleton<ObserverRegistry<UHugeTuple>>();
            services.AddSingleton<CallbackRegistry<UInt128, UHugeTuple, UHugeTemplate>>();

            services.AddSingleton<EvaluationChannel<UHugeTuple>>();
            services.AddSingleton<ObserverChannel<UHugeTuple>>();
            services.AddSingleton<CallbackChannel<UHugeTuple>>();
            services.AddSingleton<ContinuationChannel<UHugeTuple, UHugeTemplate>>();

            services.AddSingleton<UHugeAgent>();
            services.AddSingleton<ISpaceAgent<UInt128, UHugeTuple, UHugeTemplate>>(sp => sp.GetRequiredService<UHugeAgent>());
            services.AddSingleton<ISpaceRouter<UHugeTuple, UHugeTemplate>>(sp => sp.GetRequiredService<UHugeAgent>());

            services.AddHostedService<UHugeProcessor>();
            services.AddHostedService<ObserverProcessor<UHugeTuple>>();
            services.AddHostedService<CallbackProcessor<UInt128, UHugeTuple, UHugeTemplate>>();
            services.AddHostedService<EvaluationProcessor<UHugeTuple, UHugeTemplate>>();
            services.AddHostedService<ContinuationProcessor<UHugeTuple, UHugeTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.UInt))
        {
            services.AddSingleton<ObserverRegistry<UIntTuple>>();
            services.AddSingleton<CallbackRegistry<uint, UIntTuple, UIntTemplate>>();

            services.AddSingleton<EvaluationChannel<UIntTuple>>();
            services.AddSingleton<ObserverChannel<UIntTuple>>();
            services.AddSingleton<CallbackChannel<UIntTuple>>();
            services.AddSingleton<ContinuationChannel<UIntTuple, UIntTemplate>>();

            services.AddSingleton<UIntAgent>();
            services.AddSingleton<ISpaceAgent<uint, UIntTuple, UIntTemplate>>(sp => sp.GetRequiredService<UIntAgent>());
            services.AddSingleton<ISpaceRouter<UIntTuple, UIntTemplate>>(sp => sp.GetRequiredService<UIntAgent>());

            services.AddHostedService<UIntProcessor>();
            services.AddHostedService<ObserverProcessor<UIntTuple>>();
            services.AddHostedService<CallbackProcessor<uint, UIntTuple, UIntTemplate>>();
            services.AddHostedService<EvaluationProcessor<UIntTuple, UIntTemplate>>();
            services.AddHostedService<ContinuationProcessor<UIntTuple, UIntTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.ULong))
        {
            services.AddSingleton<ObserverRegistry<ULongTuple>>();
            services.AddSingleton<CallbackRegistry<ulong, ULongTuple, ULongTemplate>>();

            services.AddSingleton<EvaluationChannel<ULongTuple>>();
            services.AddSingleton<ObserverChannel<ULongTuple>>();
            services.AddSingleton<CallbackChannel<ULongTuple>>();
            services.AddSingleton<ContinuationChannel<ULongTuple, ULongTemplate>>();

            services.AddSingleton<ULongAgent>();
            services.AddSingleton<ISpaceAgent<ulong, ULongTuple, ULongTemplate>>(sp => sp.GetRequiredService<ULongAgent>());
            services.AddSingleton<ISpaceRouter<ULongTuple, ULongTemplate>>(sp => sp.GetRequiredService<ULongAgent>());

            services.AddHostedService<ULongProcessor>();
            services.AddHostedService<ObserverProcessor<ULongTuple>>();
            services.AddHostedService<CallbackProcessor<ulong, ULongTuple, ULongTemplate>>();
            services.AddHostedService<EvaluationProcessor<ULongTuple, ULongTemplate>>();
            services.AddHostedService<ContinuationProcessor<ULongTuple, ULongTemplate>>();
        }

        if (spaceKind.HasFlag(SpaceKind.UShort))
        {
            services.AddSingleton<ObserverRegistry<UShortTuple>>();
            services.AddSingleton<CallbackRegistry<ushort, UShortTuple, UShortTemplate>>();

            services.AddSingleton<EvaluationChannel<UShortTuple>>();
            services.AddSingleton<ObserverChannel<UShortTuple>>();
            services.AddSingleton<CallbackChannel<UShortTuple>>();
            services.AddSingleton<ContinuationChannel<UShortTuple, UShortTemplate>>();

            services.AddSingleton<UShortAgent>();
            services.AddSingleton<ISpaceAgent<ushort, UShortTuple, UShortTemplate>>(sp => sp.GetRequiredService<UShortAgent>());
            services.AddSingleton<ISpaceRouter<UShortTuple, UShortTemplate>>(sp => sp.GetRequiredService<UShortAgent>());

            services.AddHostedService<UShortProcessor>();
            services.AddHostedService<ObserverProcessor<UShortTuple>>();
            services.AddHostedService<CallbackProcessor<ushort, UShortTuple, UShortTemplate>>();
            services.AddHostedService<EvaluationProcessor<UShortTuple, UShortTemplate>>();
            services.AddHostedService<ContinuationProcessor<UShortTuple, UShortTemplate>>();
        }
    }
}