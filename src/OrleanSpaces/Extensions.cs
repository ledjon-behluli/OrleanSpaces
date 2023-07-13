﻿using OrleanSpaces.Continuations;
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
    /// <param name="configureOptions">An optional delegate to configure the <see cref="SpaceOptions"/></param>
    public static ISiloBuilder AddOrleanSpaces(this ISiloBuilder builder, Action<SpaceOptions>? configureOptions = null)
    {
        builder.Services.AddOrleanSpaces(configureOptions);
        return builder;
    }

    /// <summary>
    /// Configures the tuple space on the client.
    /// </summary>
    /// <param name="builder">The orleans client builder.</param>
    /// <param name="configureOptions">An optional delegate to configure the <see cref="SpaceOptions"/></param>
    public static IClientBuilder AddOrleanSpaces(this IClientBuilder builder, Action<SpaceOptions>? configureOptions = null)
    {
        builder.Services.AddOrleanSpaces(configureOptions);
        return builder;
    }

    private static void AddOrleanSpaces(this IServiceCollection services, Action<SpaceOptions>? configureOptions = null)
    {
        SpaceOptions options = new();
        configureOptions?.Invoke(options);

        services.AddSingleton(options);

        if (options.EnabledSpaces.HasFlag(SpaceKind.Generic))
        {
            services.AddSingleton<ObserverRegistry<SpaceTuple>>();
            services.AddSingleton<CallbackRegistry>();

            services.AddSingleton<EvaluationChannel<SpaceTuple>>();
            services.AddSingleton<ObserverChannel<SpaceTuple>>();
            services.AddSingleton<CallbackChannel<SpaceTuple>>();
            services.AddSingleton<ContinuationChannel<SpaceTuple, SpaceTemplate>>();

            services.AddSingleton<SpaceAgent>();
            services.AddSingleton<ISpaceAgent>(sp => sp.GetRequiredService<SpaceAgent>());
            services.AddSingleton<IAgentProcessorBridge<SpaceTuple>>(sp => sp.GetRequiredService<SpaceAgent>());
            services.AddSingleton<ITupleRouter<SpaceTuple, SpaceTemplate>>(sp => sp.GetRequiredService<SpaceAgent>());

            services.AddHostedService<SpaceProcessor>();
            services.AddHostedService<CallbackProcessor>();
            services.AddHostedService<ObserverProcessor<SpaceTuple>>();
            services.AddHostedService<EvaluationProcessor<SpaceTuple, SpaceTemplate>>();
            services.AddHostedService<ContinuationProcessor<SpaceTuple, SpaceTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Bool))
        {
            services.AddSingleton<ObserverRegistry<BoolTuple>>();
            services.AddSingleton<CallbackRegistry<bool, BoolTuple, BoolTemplate>>();

            services.AddSingleton<EvaluationChannel<BoolTuple>>();
            services.AddSingleton<ObserverChannel<BoolTuple>>();
            services.AddSingleton<CallbackChannel<BoolTuple>>();
            services.AddSingleton<ContinuationChannel<BoolTuple, BoolTemplate>>();

            services.AddSingleton<BoolAgent>();
            services.AddSingleton<ISpaceAgent<bool, BoolTuple, BoolTemplate>>(sp => sp.GetRequiredService<BoolAgent>());
            services.AddSingleton<IAgentProcessorBridge<BoolTuple>>(sp => sp.GetRequiredService<BoolAgent>());
            services.AddSingleton<ITupleRouter<BoolTuple, BoolTemplate>>(sp => sp.GetRequiredService<BoolAgent>());

            services.AddHostedService<BoolProcessor>();
            services.AddHostedService<ObserverProcessor<BoolTuple>>();
            services.AddHostedService<CallbackProcessor<bool, BoolTuple, BoolTemplate>>();
            services.AddHostedService<EvaluationProcessor<BoolTuple, BoolTemplate>>();
            services.AddHostedService<ContinuationProcessor<BoolTuple, BoolTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Byte))
        {
            services.AddSingleton<ObserverRegistry<ByteTuple>>();
            services.AddSingleton<CallbackRegistry<byte, ByteTuple, ByteTemplate>>();

            services.AddSingleton<EvaluationChannel<ByteTuple>>();
            services.AddSingleton<ObserverChannel<ByteTuple>>();
            services.AddSingleton<CallbackChannel<ByteTuple>>();
            services.AddSingleton<ContinuationChannel<ByteTuple, ByteTemplate>>();

            services.AddSingleton<ByteAgent>();
            services.AddSingleton<ISpaceAgent<byte, ByteTuple, ByteTemplate>>(sp => sp.GetRequiredService<ByteAgent>());
            services.AddSingleton<IAgentProcessorBridge<ByteTuple>>(sp => sp.GetRequiredService<ByteAgent>());
            services.AddSingleton<ITupleRouter<ByteTuple, ByteTemplate>>(sp => sp.GetRequiredService<ByteAgent>());

            services.AddHostedService<ByteProcessor>();
            services.AddHostedService<ObserverProcessor<ByteTuple>>();
            services.AddHostedService<CallbackProcessor<byte, ByteTuple, ByteTemplate>>();
            services.AddHostedService<EvaluationProcessor<ByteTuple, ByteTemplate>>();
            services.AddHostedService<ContinuationProcessor<ByteTuple, ByteTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Char))
        {
            services.AddSingleton<ObserverRegistry<CharTuple>>();
            services.AddSingleton<CallbackRegistry<char, CharTuple, CharTemplate>>();

            services.AddSingleton<EvaluationChannel<CharTuple>>();
            services.AddSingleton<ObserverChannel<CharTuple>>();
            services.AddSingleton<CallbackChannel<CharTuple>>();
            services.AddSingleton<ContinuationChannel<CharTuple, CharTemplate>>();

            services.AddSingleton<CharAgent>();
            services.AddSingleton<ISpaceAgent<char, CharTuple, CharTemplate>>(sp => sp.GetRequiredService<CharAgent>());
            services.AddSingleton<IAgentProcessorBridge<CharTuple>>(sp => sp.GetRequiredService<CharAgent>());
            services.AddSingleton<ITupleRouter<CharTuple, CharTemplate>>(sp => sp.GetRequiredService<CharAgent>());

            services.AddHostedService<CharProcessor>();
            services.AddHostedService<ObserverProcessor<CharTuple>>();
            services.AddHostedService<CallbackProcessor<char, CharTuple, CharTemplate>>();
            services.AddHostedService<EvaluationProcessor<CharTuple, CharTemplate>>();
            services.AddHostedService<ContinuationProcessor<CharTuple, CharTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.DateTimeOffset))
        {
            services.AddSingleton<ObserverRegistry<DateTimeOffsetTuple>>();
            services.AddSingleton<CallbackRegistry<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>();

            services.AddSingleton<EvaluationChannel<DateTimeOffsetTuple>>();
            services.AddSingleton<ObserverChannel<DateTimeOffsetTuple>>();
            services.AddSingleton<CallbackChannel<DateTimeOffsetTuple>>();
            services.AddSingleton<ContinuationChannel<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();

            services.AddSingleton<DateTimeOffsetAgent>();
            services.AddSingleton<ISpaceAgent<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>(sp => sp.GetRequiredService<DateTimeOffsetAgent>());
            services.AddSingleton<IAgentProcessorBridge<DateTimeOffsetTuple>>(sp => sp.GetRequiredService<DateTimeOffsetAgent>());
            services.AddSingleton<ITupleRouter<DateTimeOffsetTuple, DateTimeOffsetTemplate>>(sp => sp.GetRequiredService<DateTimeOffsetAgent>());

            services.AddHostedService<DateTimeOffsetProcessor>();
            services.AddHostedService<ObserverProcessor<DateTimeOffsetTuple>>();
            services.AddHostedService<CallbackProcessor<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
            services.AddHostedService<EvaluationProcessor<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
            services.AddHostedService<ContinuationProcessor<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.DateTime))
        {
            services.AddSingleton<ObserverRegistry<DateTimeTuple>>();
            services.AddSingleton<CallbackRegistry<DateTime, DateTimeTuple, DateTimeTemplate>>();

            services.AddSingleton<EvaluationChannel<DateTimeTuple>>();
            services.AddSingleton<ObserverChannel<DateTimeTuple>>();
            services.AddSingleton<CallbackChannel<DateTimeTuple>>();
            services.AddSingleton<ContinuationChannel<DateTimeTuple, DateTimeTemplate>>();

            services.AddSingleton<DateTimeAgent>();
            services.AddSingleton<ISpaceAgent<DateTime, DateTimeTuple, DateTimeTemplate>>(sp => sp.GetRequiredService<DateTimeAgent>());
            services.AddSingleton<IAgentProcessorBridge<DateTimeTuple>>(sp => sp.GetRequiredService<DateTimeAgent>());
            services.AddSingleton<ITupleRouter<DateTimeTuple, DateTimeTemplate>>(sp => sp.GetRequiredService<DateTimeAgent>());

            services.AddHostedService<DateTimeProcessor>();
            services.AddHostedService<ObserverProcessor<DateTimeTuple>>();
            services.AddHostedService<CallbackProcessor<DateTime, DateTimeTuple, DateTimeTemplate>>();
            services.AddHostedService<EvaluationProcessor<DateTimeTuple, DateTimeTemplate>>();
            services.AddHostedService<ContinuationProcessor<DateTimeTuple, DateTimeTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Decimal))
        {
            services.AddSingleton<ObserverRegistry<DecimalTuple>>();
            services.AddSingleton<CallbackRegistry<decimal, DecimalTuple, DecimalTemplate>>();

            services.AddSingleton<EvaluationChannel<DecimalTuple>>();
            services.AddSingleton<ObserverChannel<DecimalTuple>>();
            services.AddSingleton<CallbackChannel<DecimalTuple>>();
            services.AddSingleton<ContinuationChannel<DecimalTuple, DecimalTemplate>>();

            services.AddSingleton<DecimalAgent>();
            services.AddSingleton<ISpaceAgent<decimal, DecimalTuple, DecimalTemplate>>(sp => sp.GetRequiredService<DecimalAgent>());
            services.AddSingleton<IAgentProcessorBridge<DecimalTuple>>(sp => sp.GetRequiredService<DecimalAgent>());
            services.AddSingleton<ITupleRouter<DecimalTuple, DecimalTemplate>>(sp => sp.GetRequiredService<DecimalAgent>());

            services.AddHostedService<DecimalProcessor>();
            services.AddHostedService<ObserverProcessor<DecimalTuple>>();
            services.AddHostedService<CallbackProcessor<decimal, DecimalTuple, DecimalTemplate>>();
            services.AddHostedService<EvaluationProcessor<DecimalTuple, DecimalTemplate>>();
            services.AddHostedService<ContinuationProcessor<DecimalTuple, DecimalTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Double))
        {
            services.AddSingleton<ObserverRegistry<DoubleTuple>>();
            services.AddSingleton<CallbackRegistry<double, DoubleTuple, DoubleTemplate>>();

            services.AddSingleton<EvaluationChannel<DoubleTuple>>();
            services.AddSingleton<ObserverChannel<DoubleTuple>>();
            services.AddSingleton<CallbackChannel<DoubleTuple>>();
            services.AddSingleton<ContinuationChannel<DoubleTuple, DoubleTemplate>>();

            services.AddSingleton<DoubleAgent>();
            services.AddSingleton<ISpaceAgent<double, DoubleTuple, DoubleTemplate>>(sp => sp.GetRequiredService<DoubleAgent>());
            services.AddSingleton<IAgentProcessorBridge<DoubleTuple>>(sp => sp.GetRequiredService<DoubleAgent>());
            services.AddSingleton<ITupleRouter<DoubleTuple, DoubleTemplate>>(sp => sp.GetRequiredService<DoubleAgent>());

            services.AddHostedService<DoubleProcessor>();
            services.AddHostedService<ObserverProcessor<DoubleTuple>>();
            services.AddHostedService<CallbackProcessor<double, DoubleTuple, DoubleTemplate>>();
            services.AddHostedService<EvaluationProcessor<DoubleTuple, DoubleTemplate>>();
            services.AddHostedService<ContinuationProcessor<DoubleTuple, DoubleTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Float))
        {
            services.AddSingleton<ObserverRegistry<FloatTuple>>();
            services.AddSingleton<CallbackRegistry<float, FloatTuple, FloatTemplate>>();

            services.AddSingleton<EvaluationChannel<FloatTuple>>();
            services.AddSingleton<ObserverChannel<FloatTuple>>();
            services.AddSingleton<CallbackChannel<FloatTuple>>();
            services.AddSingleton<ContinuationChannel<FloatTuple, FloatTemplate>>();

            services.AddSingleton<FloatAgent>();
            services.AddSingleton<ISpaceAgent<float, FloatTuple, FloatTemplate>>(sp => sp.GetRequiredService<FloatAgent>());
            services.AddSingleton<IAgentProcessorBridge<FloatTuple>>(sp => sp.GetRequiredService<FloatAgent>());
            services.AddSingleton<ITupleRouter<FloatTuple, FloatTemplate>>(sp => sp.GetRequiredService<FloatAgent>());

            services.AddHostedService<FloatProcessor>();
            services.AddHostedService<ObserverProcessor<FloatTuple>>();
            services.AddHostedService<CallbackProcessor<float, FloatTuple, FloatTemplate>>();
            services.AddHostedService<EvaluationProcessor<FloatTuple, FloatTemplate>>();
            services.AddHostedService<ContinuationProcessor<FloatTuple, FloatTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Guid))
        {
            services.AddSingleton<ObserverRegistry<GuidTuple>>();
            services.AddSingleton<CallbackRegistry<Guid, GuidTuple, GuidTemplate>>();

            services.AddSingleton<EvaluationChannel<GuidTuple>>();
            services.AddSingleton<ObserverChannel<GuidTuple>>();
            services.AddSingleton<CallbackChannel<GuidTuple>>();
            services.AddSingleton<ContinuationChannel<GuidTuple, GuidTemplate>>();

            services.AddSingleton<GuidAgent>();
            services.AddSingleton<ISpaceAgent<Guid, GuidTuple, GuidTemplate>>(sp => sp.GetRequiredService<GuidAgent>());
            services.AddSingleton<IAgentProcessorBridge<GuidTuple>>(sp => sp.GetRequiredService<GuidAgent>());
            services.AddSingleton<ITupleRouter<GuidTuple, GuidTemplate>>(sp => sp.GetRequiredService<GuidAgent>());

            services.AddHostedService<GuidProcessor>();
            services.AddHostedService<ObserverProcessor<GuidTuple>>();
            services.AddHostedService<CallbackProcessor<Guid, GuidTuple, GuidTemplate>>();
            services.AddHostedService<EvaluationProcessor<GuidTuple, GuidTemplate>>();
            services.AddHostedService<ContinuationProcessor<GuidTuple, GuidTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Huge))
        {
            services.AddSingleton<ObserverRegistry<HugeTuple>>();
            services.AddSingleton<CallbackRegistry<Int128, HugeTuple, HugeTemplate>>();

            services.AddSingleton<EvaluationChannel<HugeTuple>>();
            services.AddSingleton<ObserverChannel<HugeTuple>>();
            services.AddSingleton<CallbackChannel<HugeTuple>>();
            services.AddSingleton<ContinuationChannel<HugeTuple, HugeTemplate>>();

            services.AddSingleton<HugeAgent>();
            services.AddSingleton<ISpaceAgent<Int128, HugeTuple, HugeTemplate>>(sp => sp.GetRequiredService<HugeAgent>());
            services.AddSingleton<IAgentProcessorBridge<HugeTuple>>(sp => sp.GetRequiredService<HugeAgent>());
            services.AddSingleton<ITupleRouter<HugeTuple, HugeTemplate>>(sp => sp.GetRequiredService<HugeAgent>());

            services.AddHostedService<HugeProcessor>();
            services.AddHostedService<ObserverProcessor<HugeTuple>>();
            services.AddHostedService<CallbackProcessor<Int128, HugeTuple, HugeTemplate>>();
            services.AddHostedService<EvaluationProcessor<HugeTuple, HugeTemplate>>();
            services.AddHostedService<ContinuationProcessor<HugeTuple, HugeTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Int))
        {
            services.AddSingleton<ObserverRegistry<IntTuple>>();
            services.AddSingleton<CallbackRegistry<int, IntTuple, IntTemplate>>();

            services.AddSingleton<EvaluationChannel<IntTuple>>();
            services.AddSingleton<ObserverChannel<IntTuple>>();
            services.AddSingleton<CallbackChannel<IntTuple>>();
            services.AddSingleton<ContinuationChannel<IntTuple, IntTemplate>>();

            services.AddSingleton<IntAgent>();
            services.AddSingleton<ISpaceAgent<int, IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());
            services.AddSingleton<IAgentProcessorBridge<IntTuple>>(sp => sp.GetRequiredService<IntAgent>());
            services.AddSingleton<ITupleRouter<IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());

            services.AddHostedService<IntProcessor>();
            services.AddHostedService<ObserverProcessor<IntTuple>>();
            services.AddHostedService<CallbackProcessor<int, IntTuple, IntTemplate>>();
            services.AddHostedService<EvaluationProcessor<IntTuple, IntTemplate>>();
            services.AddHostedService<ContinuationProcessor<IntTuple, IntTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Long))
        {
            services.AddSingleton<ObserverRegistry<LongTuple>>();
            services.AddSingleton<CallbackRegistry<long, LongTuple, LongTemplate>>();

            services.AddSingleton<EvaluationChannel<LongTuple>>();
            services.AddSingleton<ObserverChannel<LongTuple>>();
            services.AddSingleton<CallbackChannel<LongTuple>>();
            services.AddSingleton<ContinuationChannel<LongTuple, LongTemplate>>();

            services.AddSingleton<LongAgent>();
            services.AddSingleton<ISpaceAgent<long, LongTuple, LongTemplate>>(sp => sp.GetRequiredService<LongAgent>());
            services.AddSingleton<IAgentProcessorBridge<LongTuple>>(sp => sp.GetRequiredService<LongAgent>());
            services.AddSingleton<ITupleRouter<LongTuple, LongTemplate>>(sp => sp.GetRequiredService<LongAgent>());

            services.AddHostedService<LongProcessor>();
            services.AddHostedService<ObserverProcessor<LongTuple>>();
            services.AddHostedService<CallbackProcessor<long, LongTuple, LongTemplate>>();
            services.AddHostedService<EvaluationProcessor<LongTuple, LongTemplate>>();
            services.AddHostedService<ContinuationProcessor<LongTuple, LongTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.SByte))
        {
            services.AddSingleton<ObserverRegistry<SByteTuple>>();
            services.AddSingleton<CallbackRegistry<sbyte, SByteTuple, SByteTemplate>>();

            services.AddSingleton<EvaluationChannel<SByteTuple>>();
            services.AddSingleton<ObserverChannel<SByteTuple>>();
            services.AddSingleton<CallbackChannel<SByteTuple>>();
            services.AddSingleton<ContinuationChannel<SByteTuple, SByteTemplate>>();

            services.AddSingleton<SByteAgent>();
            services.AddSingleton<ISpaceAgent<sbyte, SByteTuple, SByteTemplate>>(sp => sp.GetRequiredService<SByteAgent>());
            services.AddSingleton<IAgentProcessorBridge<SByteTuple>>(sp => sp.GetRequiredService<SByteAgent>());
            services.AddSingleton<ITupleRouter<SByteTuple, SByteTemplate>>(sp => sp.GetRequiredService<SByteAgent>());

            services.AddHostedService<SByteProcessor>();
            services.AddHostedService<ObserverProcessor<SByteTuple>>();
            services.AddHostedService<CallbackProcessor<sbyte, SByteTuple, SByteTemplate>>();
            services.AddHostedService<EvaluationProcessor<SByteTuple, SByteTemplate>>();
            services.AddHostedService<ContinuationProcessor<SByteTuple, SByteTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Short))
        {
            services.AddSingleton<ObserverRegistry<ShortTuple>>();
            services.AddSingleton<CallbackRegistry<short, ShortTuple, ShortTemplate>>();

            services.AddSingleton<EvaluationChannel<ShortTuple>>();
            services.AddSingleton<ObserverChannel<ShortTuple>>();
            services.AddSingleton<CallbackChannel<ShortTuple>>();
            services.AddSingleton<ContinuationChannel<ShortTuple, ShortTemplate>>();

            services.AddSingleton<ShortAgent>();
            services.AddSingleton<ISpaceAgent<short, ShortTuple, ShortTemplate>>(sp => sp.GetRequiredService<ShortAgent>());
            services.AddSingleton<IAgentProcessorBridge<ShortTuple>>(sp => sp.GetRequiredService<ShortAgent>());
            services.AddSingleton<ITupleRouter<ShortTuple, ShortTemplate>>(sp => sp.GetRequiredService<ShortAgent>());

            services.AddHostedService<ShortProcessor>();
            services.AddHostedService<ObserverProcessor<ShortTuple>>();
            services.AddHostedService<CallbackProcessor<short, ShortTuple, ShortTemplate>>();
            services.AddHostedService<EvaluationProcessor<ShortTuple, ShortTemplate>>();
            services.AddHostedService<ContinuationProcessor<ShortTuple, ShortTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.TimeSpan))
        {
            services.AddSingleton<ObserverRegistry<TimeSpanTuple>>();
            services.AddSingleton<CallbackRegistry<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>();

            services.AddSingleton<EvaluationChannel<TimeSpanTuple>>();
            services.AddSingleton<ObserverChannel<TimeSpanTuple>>();
            services.AddSingleton<CallbackChannel<TimeSpanTuple>>();
            services.AddSingleton<ContinuationChannel<TimeSpanTuple, TimeSpanTemplate>>();

            services.AddSingleton<TimeSpanAgent>();
            services.AddSingleton<ISpaceAgent<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>(sp => sp.GetRequiredService<TimeSpanAgent>());
            services.AddSingleton<IAgentProcessorBridge<TimeSpanTuple>>(sp => sp.GetRequiredService<TimeSpanAgent>());
            services.AddSingleton<ITupleRouter<TimeSpanTuple, TimeSpanTemplate>>(sp => sp.GetRequiredService<TimeSpanAgent>());

            services.AddHostedService<TimeSpanProcessor>();
            services.AddHostedService<ObserverProcessor<TimeSpanTuple>>();
            services.AddHostedService<CallbackProcessor<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>();
            services.AddHostedService<EvaluationProcessor<TimeSpanTuple, TimeSpanTemplate>>();
            services.AddHostedService<ContinuationProcessor<TimeSpanTuple, TimeSpanTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.UHuge))
        {
            services.AddSingleton<ObserverRegistry<UHugeTuple>>();
            services.AddSingleton<CallbackRegistry<UInt128, UHugeTuple, UHugeTemplate>>();

            services.AddSingleton<EvaluationChannel<UHugeTuple>>();
            services.AddSingleton<ObserverChannel<UHugeTuple>>();
            services.AddSingleton<CallbackChannel<UHugeTuple>>();
            services.AddSingleton<ContinuationChannel<UHugeTuple, UHugeTemplate>>();

            services.AddSingleton<UHugeAgent>();
            services.AddSingleton<ISpaceAgent<UInt128, UHugeTuple, UHugeTemplate>>(sp => sp.GetRequiredService<UHugeAgent>());
            services.AddSingleton<IAgentProcessorBridge<UHugeTuple>>(sp => sp.GetRequiredService<UHugeAgent>());
            services.AddSingleton<ITupleRouter<UHugeTuple, UHugeTemplate>>(sp => sp.GetRequiredService<UHugeAgent>());

            services.AddHostedService<UHugeProcessor>();
            services.AddHostedService<ObserverProcessor<UHugeTuple>>();
            services.AddHostedService<CallbackProcessor<UInt128, UHugeTuple, UHugeTemplate>>();
            services.AddHostedService<EvaluationProcessor<UHugeTuple, UHugeTemplate>>();
            services.AddHostedService<ContinuationProcessor<UHugeTuple, UHugeTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.UInt))
        {
            services.AddSingleton<ObserverRegistry<UIntTuple>>();
            services.AddSingleton<CallbackRegistry<uint, UIntTuple, UIntTemplate>>();

            services.AddSingleton<EvaluationChannel<UIntTuple>>();
            services.AddSingleton<ObserverChannel<UIntTuple>>();
            services.AddSingleton<CallbackChannel<UIntTuple>>();
            services.AddSingleton<ContinuationChannel<UIntTuple, UIntTemplate>>();

            services.AddSingleton<UIntAgent>();
            services.AddSingleton<ISpaceAgent<uint, UIntTuple, UIntTemplate>>(sp => sp.GetRequiredService<UIntAgent>());
            services.AddSingleton<IAgentProcessorBridge<UIntTuple>>(sp => sp.GetRequiredService<UIntAgent>());
            services.AddSingleton<ITupleRouter<UIntTuple, UIntTemplate>>(sp => sp.GetRequiredService<UIntAgent>());

            services.AddHostedService<UIntProcessor>();
            services.AddHostedService<ObserverProcessor<UIntTuple>>();
            services.AddHostedService<CallbackProcessor<uint, UIntTuple, UIntTemplate>>();
            services.AddHostedService<EvaluationProcessor<UIntTuple, UIntTemplate>>();
            services.AddHostedService<ContinuationProcessor<UIntTuple, UIntTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.ULong))
        {
            services.AddSingleton<ObserverRegistry<ULongTuple>>();
            services.AddSingleton<CallbackRegistry<ulong, ULongTuple, ULongTemplate>>();

            services.AddSingleton<EvaluationChannel<ULongTuple>>();
            services.AddSingleton<ObserverChannel<ULongTuple>>();
            services.AddSingleton<CallbackChannel<ULongTuple>>();
            services.AddSingleton<ContinuationChannel<ULongTuple, ULongTemplate>>();

            services.AddSingleton<ULongAgent>();
            services.AddSingleton<ISpaceAgent<ulong, ULongTuple, ULongTemplate>>(sp => sp.GetRequiredService<ULongAgent>());
            services.AddSingleton<IAgentProcessorBridge<ULongTuple>>(sp => sp.GetRequiredService<ULongAgent>());
            services.AddSingleton<ITupleRouter<ULongTuple, ULongTemplate>>(sp => sp.GetRequiredService<ULongAgent>());

            services.AddHostedService<ULongProcessor>();
            services.AddHostedService<ObserverProcessor<ULongTuple>>();
            services.AddHostedService<CallbackProcessor<ulong, ULongTuple, ULongTemplate>>();
            services.AddHostedService<EvaluationProcessor<ULongTuple, ULongTemplate>>();
            services.AddHostedService<ContinuationProcessor<ULongTuple, ULongTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.UShort))
        {
            services.AddSingleton<ObserverRegistry<UShortTuple>>();
            services.AddSingleton<CallbackRegistry<ushort, UShortTuple, UShortTemplate>>();

            services.AddSingleton<EvaluationChannel<UShortTuple>>();
            services.AddSingleton<ObserverChannel<UShortTuple>>();
            services.AddSingleton<CallbackChannel<UShortTuple>>();
            services.AddSingleton<ContinuationChannel<UShortTuple, UShortTemplate>>();

            services.AddSingleton<UShortAgent>();
            services.AddSingleton<ISpaceAgent<ushort, UShortTuple, UShortTemplate>>(sp => sp.GetRequiredService<UShortAgent>());
            services.AddSingleton<IAgentProcessorBridge<UShortTuple>>(sp => sp.GetRequiredService<UShortAgent>());
            services.AddSingleton<ITupleRouter<UShortTuple, UShortTemplate>>(sp => sp.GetRequiredService<UShortAgent>());

            services.AddHostedService<UShortProcessor>();
            services.AddHostedService<ObserverProcessor<UShortTuple>>();
            services.AddHostedService<CallbackProcessor<ushort, UShortTuple, UShortTemplate>>();
            services.AddHostedService<EvaluationProcessor<UShortTuple, UShortTemplate>>();
            services.AddHostedService<ContinuationProcessor<UShortTuple, UShortTemplate>>();
        }
    }
}