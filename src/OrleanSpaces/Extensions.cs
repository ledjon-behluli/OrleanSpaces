using OrleanSpaces.Observers;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Continuations;
using OrleanSpaces.Agents;
using OrleanSpaces.Tuples;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces;

public static class Extensions
{
    /// <summary>
    /// Configures the tuple space on the <see cref="IClientBuilder"/>.
    /// </summary>
    /// <param name="builder">The orleans client builder.</param>
    /// <param name="configureOptions">An optional delegate to configure the <see cref="SpaceOptions"/></param>
    public static IClientBuilder AddOrleanSpaces(this IClientBuilder builder, Action<SpaceOptions>? configureOptions = null)
    {
        SpaceOptions options = new();
        configureOptions?.Invoke(options);

        builder.Services.AddSingleton(options);

        if (options.EnabledSpaces.HasFlag(SpaceKind.Generic))
        {
            builder.Services.AddSingleton<ObserverRegistry<SpaceTuple>>();
            builder.Services.AddSingleton<CallbackRegistry>();

            builder.Services.AddSingleton<EvaluationChannel<SpaceTuple>>();
            builder.Services.AddSingleton<ObserverChannel<SpaceTuple>>();
            builder.Services.AddSingleton<CallbackChannel<SpaceTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<SpaceTuple, SpaceTemplate>>();

            builder.Services.AddSingleton<SpaceAgent>();
            builder.Services.AddSingleton<ISpaceAgent>(sp => sp.GetRequiredService<SpaceAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<SpaceTuple>>(sp => sp.GetRequiredService<SpaceAgent>());
            builder.Services.AddSingleton<ITupleRouter<SpaceTuple, SpaceTemplate>>(sp => sp.GetRequiredService<SpaceAgent>());

            builder.Services.AddHostedService<CallbackProcessor>();
            builder.Services.AddHostedService<ObserverProcessor<SpaceTuple>>();
            builder.Services.AddHostedService<EvaluationProcessor<SpaceTuple, SpaceTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<SpaceTuple, SpaceTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Bool))
        {
            builder.Services.AddSingleton<ObserverRegistry<BoolTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<bool, BoolTuple, BoolTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<BoolTuple>>();
            builder.Services.AddSingleton<ObserverChannel<BoolTuple>>();
            builder.Services.AddSingleton<CallbackChannel<BoolTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<BoolTuple, BoolTemplate>>();

            builder.Services.AddSingleton<BoolAgent>();
            builder.Services.AddSingleton<ISpaceAgent<bool, BoolTuple, BoolTemplate>>(sp => sp.GetRequiredService<BoolAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<BoolTuple>>(sp => sp.GetRequiredService<BoolAgent>());
            builder.Services.AddSingleton<ITupleRouter<BoolTuple, BoolTemplate>>(sp => sp.GetRequiredService<BoolAgent>());

            builder.Services.AddHostedService<ObserverProcessor<BoolTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<bool, BoolTuple, BoolTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<BoolTuple, BoolTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<BoolTuple, BoolTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Byte))
        {
            builder.Services.AddSingleton<ObserverRegistry<ByteTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<byte, ByteTuple, ByteTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<ByteTuple>>();
            builder.Services.AddSingleton<ObserverChannel<ByteTuple>>();
            builder.Services.AddSingleton<CallbackChannel<ByteTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<ByteTuple, ByteTemplate>>();

            builder.Services.AddSingleton<ByteAgent>();
            builder.Services.AddSingleton<ISpaceAgent<byte, ByteTuple, ByteTemplate>>(sp => sp.GetRequiredService<ByteAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<ByteTuple>>(sp => sp.GetRequiredService<ByteAgent>());
            builder.Services.AddSingleton<ITupleRouter<ByteTuple, ByteTemplate>>(sp => sp.GetRequiredService<ByteAgent>());

            builder.Services.AddHostedService<ObserverProcessor<ByteTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<byte, ByteTuple, ByteTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<ByteTuple, ByteTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<ByteTuple, ByteTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Char))
        {
            builder.Services.AddSingleton<ObserverRegistry<CharTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<char, CharTuple, CharTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<CharTuple>>();
            builder.Services.AddSingleton<ObserverChannel<CharTuple>>();
            builder.Services.AddSingleton<CallbackChannel<CharTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<CharTuple, CharTemplate>>();

            builder.Services.AddSingleton<CharAgent>();
            builder.Services.AddSingleton<ISpaceAgent<char, CharTuple, CharTemplate>>(sp => sp.GetRequiredService<CharAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<CharTuple>>(sp => sp.GetRequiredService<CharAgent>());
            builder.Services.AddSingleton<ITupleRouter<CharTuple, CharTemplate>>(sp => sp.GetRequiredService<CharAgent>());

            builder.Services.AddHostedService<ObserverProcessor<CharTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<char, CharTuple, CharTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<CharTuple, CharTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<CharTuple, CharTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.DateTimeOffset))
        {
            builder.Services.AddSingleton<ObserverRegistry<DateTimeOffsetTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<DateTimeOffsetTuple>>();
            builder.Services.AddSingleton<ObserverChannel<DateTimeOffsetTuple>>();
            builder.Services.AddSingleton<CallbackChannel<DateTimeOffsetTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();

            builder.Services.AddSingleton<DateTimeOffsetAgent>();
            builder.Services.AddSingleton<ISpaceAgent<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>(sp => sp.GetRequiredService<DateTimeOffsetAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<DateTimeOffsetTuple>>(sp => sp.GetRequiredService<DateTimeOffsetAgent>());
            builder.Services.AddSingleton<ITupleRouter<DateTimeOffsetTuple, DateTimeOffsetTemplate>>(sp => sp.GetRequiredService<DateTimeOffsetAgent>());

            builder.Services.AddHostedService<ObserverProcessor<DateTimeOffsetTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<DateTimeOffsetTuple, DateTimeOffsetTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.DateTime))
        {
            builder.Services.AddSingleton<ObserverRegistry<DateTimeTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<DateTime, DateTimeTuple, DateTimeTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<DateTimeTuple>>();
            builder.Services.AddSingleton<ObserverChannel<DateTimeTuple>>();
            builder.Services.AddSingleton<CallbackChannel<DateTimeTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<DateTimeTuple, DateTimeTemplate>>();

            builder.Services.AddSingleton<DateTimeAgent>();
            builder.Services.AddSingleton<ISpaceAgent<DateTime, DateTimeTuple, DateTimeTemplate>>(sp => sp.GetRequiredService<DateTimeAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<DateTimeTuple>>(sp => sp.GetRequiredService<DateTimeAgent>());
            builder.Services.AddSingleton<ITupleRouter<DateTimeTuple, DateTimeTemplate>>(sp => sp.GetRequiredService<DateTimeAgent>());

            builder.Services.AddHostedService<ObserverProcessor<DateTimeTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<DateTime, DateTimeTuple, DateTimeTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<DateTimeTuple, DateTimeTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<DateTimeTuple, DateTimeTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Decimal))
        {
            builder.Services.AddSingleton<ObserverRegistry<DecimalTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<decimal, DecimalTuple, DecimalTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<DecimalTuple>>();
            builder.Services.AddSingleton<ObserverChannel<DecimalTuple>>();
            builder.Services.AddSingleton<CallbackChannel<DecimalTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<DecimalTuple, DecimalTemplate>>();

            builder.Services.AddSingleton<DecimalAgent>();
            builder.Services.AddSingleton<ISpaceAgent<decimal, DecimalTuple, DecimalTemplate>>(sp => sp.GetRequiredService<DecimalAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<DecimalTuple>>(sp => sp.GetRequiredService<DecimalAgent>());
            builder.Services.AddSingleton<ITupleRouter<DecimalTuple, DecimalTemplate>>(sp => sp.GetRequiredService<DecimalAgent>());

            builder.Services.AddHostedService<ObserverProcessor<DecimalTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<decimal, DecimalTuple, DecimalTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<DecimalTuple, DecimalTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<DecimalTuple, DecimalTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Double))
        {
            builder.Services.AddSingleton<ObserverRegistry<DoubleTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<double, DoubleTuple, DoubleTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<DoubleTuple>>();
            builder.Services.AddSingleton<ObserverChannel<DoubleTuple>>();
            builder.Services.AddSingleton<CallbackChannel<DoubleTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<DoubleTuple, DoubleTemplate>>();

            builder.Services.AddSingleton<DoubleAgent>();
            builder.Services.AddSingleton<ISpaceAgent<double, DoubleTuple, DoubleTemplate>>(sp => sp.GetRequiredService<DoubleAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<DoubleTuple>>(sp => sp.GetRequiredService<DoubleAgent>());
            builder.Services.AddSingleton<ITupleRouter<DoubleTuple, DoubleTemplate>>(sp => sp.GetRequiredService<DoubleAgent>());

            builder.Services.AddHostedService<ObserverProcessor<DoubleTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<double, DoubleTuple, DoubleTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<DoubleTuple, DoubleTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<DoubleTuple, DoubleTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Float))
        {
            builder.Services.AddSingleton<ObserverRegistry<FloatTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<float, FloatTuple, FloatTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<FloatTuple>>();
            builder.Services.AddSingleton<ObserverChannel<FloatTuple>>();
            builder.Services.AddSingleton<CallbackChannel<FloatTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<FloatTuple, FloatTemplate>>();

            builder.Services.AddSingleton<FloatAgent>();
            builder.Services.AddSingleton<ISpaceAgent<float, FloatTuple, FloatTemplate>>(sp => sp.GetRequiredService<FloatAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<FloatTuple>>(sp => sp.GetRequiredService<FloatAgent>());
            builder.Services.AddSingleton<ITupleRouter<FloatTuple, FloatTemplate>>(sp => sp.GetRequiredService<FloatAgent>());

            builder.Services.AddHostedService<ObserverProcessor<FloatTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<float, FloatTuple, FloatTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<FloatTuple, FloatTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<FloatTuple, FloatTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Guid))
        {
            builder.Services.AddSingleton<ObserverRegistry<GuidTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<Guid, GuidTuple, GuidTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<GuidTuple>>();
            builder.Services.AddSingleton<ObserverChannel<GuidTuple>>();
            builder.Services.AddSingleton<CallbackChannel<GuidTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<GuidTuple, GuidTemplate>>();

            builder.Services.AddSingleton<GuidAgent>();
            builder.Services.AddSingleton<ISpaceAgent<Guid, GuidTuple, GuidTemplate>>(sp => sp.GetRequiredService<GuidAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<GuidTuple>>(sp => sp.GetRequiredService<GuidAgent>());
            builder.Services.AddSingleton<ITupleRouter<GuidTuple, GuidTemplate>>(sp => sp.GetRequiredService<GuidAgent>());

            builder.Services.AddHostedService<ObserverProcessor<GuidTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<Guid, GuidTuple, GuidTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<GuidTuple, GuidTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<GuidTuple, GuidTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Huge))
        {
            builder.Services.AddSingleton<ObserverRegistry<HugeTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<Int128, HugeTuple, HugeTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<HugeTuple>>();
            builder.Services.AddSingleton<ObserverChannel<HugeTuple>>();
            builder.Services.AddSingleton<CallbackChannel<HugeTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<HugeTuple, HugeTemplate>>();

            builder.Services.AddSingleton<HugeAgent>();
            builder.Services.AddSingleton<ISpaceAgent<Int128, HugeTuple, HugeTemplate>>(sp => sp.GetRequiredService<HugeAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<HugeTuple>>(sp => sp.GetRequiredService<HugeAgent>());
            builder.Services.AddSingleton<ITupleRouter<HugeTuple, HugeTemplate>>(sp => sp.GetRequiredService<HugeAgent>());

            builder.Services.AddHostedService<ObserverProcessor<HugeTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<Int128, HugeTuple, HugeTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<HugeTuple, HugeTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<HugeTuple, HugeTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Int))
        {
            builder.Services.AddSingleton<ObserverRegistry<IntTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<int, IntTuple, IntTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<IntTuple>>();
            builder.Services.AddSingleton<ObserverChannel<IntTuple>>();
            builder.Services.AddSingleton<CallbackChannel<IntTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<IntTuple, IntTemplate>>();

            builder.Services.AddSingleton<IntAgent>();
            builder.Services.AddSingleton<ISpaceAgent<int, IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<IntTuple>>(sp => sp.GetRequiredService<IntAgent>());
            builder.Services.AddSingleton<ITupleRouter<IntTuple, IntTemplate>>(sp => sp.GetRequiredService<IntAgent>());

            builder.Services.AddHostedService<ObserverProcessor<IntTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<int, IntTuple, IntTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<IntTuple, IntTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<IntTuple, IntTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Long))
        {
            builder.Services.AddSingleton<ObserverRegistry<LongTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<long, LongTuple, LongTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<LongTuple>>();
            builder.Services.AddSingleton<ObserverChannel<LongTuple>>();
            builder.Services.AddSingleton<CallbackChannel<LongTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<LongTuple, LongTemplate>>();

            builder.Services.AddSingleton<LongAgent>();
            builder.Services.AddSingleton<ISpaceAgent<long, LongTuple, LongTemplate>>(sp => sp.GetRequiredService<LongAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<LongTuple>>(sp => sp.GetRequiredService<LongAgent>());
            builder.Services.AddSingleton<ITupleRouter<LongTuple, LongTemplate>>(sp => sp.GetRequiredService<LongAgent>());

            builder.Services.AddHostedService<ObserverProcessor<LongTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<long, LongTuple, LongTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<LongTuple, LongTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<LongTuple, LongTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.SByte))
        {
            builder.Services.AddSingleton<ObserverRegistry<SByteTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<sbyte, SByteTuple, SByteTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<SByteTuple>>();
            builder.Services.AddSingleton<ObserverChannel<SByteTuple>>();
            builder.Services.AddSingleton<CallbackChannel<SByteTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<SByteTuple, SByteTemplate>>();

            builder.Services.AddSingleton<SByteAgent>();
            builder.Services.AddSingleton<ISpaceAgent<sbyte, SByteTuple, SByteTemplate>>(sp => sp.GetRequiredService<SByteAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<SByteTuple>>(sp => sp.GetRequiredService<SByteAgent>());
            builder.Services.AddSingleton<ITupleRouter<SByteTuple, SByteTemplate>>(sp => sp.GetRequiredService<SByteAgent>());

            builder.Services.AddHostedService<ObserverProcessor<SByteTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<sbyte, SByteTuple, SByteTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<SByteTuple, SByteTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<SByteTuple, SByteTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.Short))
        {
            builder.Services.AddSingleton<ObserverRegistry<ShortTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<short, ShortTuple, ShortTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<ShortTuple>>();
            builder.Services.AddSingleton<ObserverChannel<ShortTuple>>();
            builder.Services.AddSingleton<CallbackChannel<ShortTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<ShortTuple, ShortTemplate>>();

            builder.Services.AddSingleton<ShortAgent>();
            builder.Services.AddSingleton<ISpaceAgent<short, ShortTuple, ShortTemplate>>(sp => sp.GetRequiredService<ShortAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<ShortTuple>>(sp => sp.GetRequiredService<ShortAgent>());
            builder.Services.AddSingleton<ITupleRouter<ShortTuple, ShortTemplate>>(sp => sp.GetRequiredService<ShortAgent>());

            builder.Services.AddHostedService<ObserverProcessor<ShortTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<short, ShortTuple, ShortTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<ShortTuple, ShortTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<ShortTuple, ShortTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.TimeSpan))
        {
            builder.Services.AddSingleton<ObserverRegistry<TimeSpanTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<TimeSpanTuple>>();
            builder.Services.AddSingleton<ObserverChannel<TimeSpanTuple>>();
            builder.Services.AddSingleton<CallbackChannel<TimeSpanTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<TimeSpanTuple, TimeSpanTemplate>>();

            builder.Services.AddSingleton<TimeSpanAgent>();
            builder.Services.AddSingleton<ISpaceAgent<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>(sp => sp.GetRequiredService<TimeSpanAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<TimeSpanTuple>>(sp => sp.GetRequiredService<TimeSpanAgent>());
            builder.Services.AddSingleton<ITupleRouter<TimeSpanTuple, TimeSpanTemplate>>(sp => sp.GetRequiredService<TimeSpanAgent>());

            builder.Services.AddHostedService<ObserverProcessor<TimeSpanTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<TimeSpan, TimeSpanTuple, TimeSpanTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<TimeSpanTuple, TimeSpanTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<TimeSpanTuple, TimeSpanTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.UHuge))
        {
            builder.Services.AddSingleton<ObserverRegistry<UHugeTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<UInt128, UHugeTuple, UHugeTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<UHugeTuple>>();
            builder.Services.AddSingleton<ObserverChannel<UHugeTuple>>();
            builder.Services.AddSingleton<CallbackChannel<UHugeTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<UHugeTuple, UHugeTemplate>>();

            builder.Services.AddSingleton<UHugeAgent>();
            builder.Services.AddSingleton<ISpaceAgent<UInt128, UHugeTuple, UHugeTemplate>>(sp => sp.GetRequiredService<UHugeAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<UHugeTuple>>(sp => sp.GetRequiredService<UHugeAgent>());
            builder.Services.AddSingleton<ITupleRouter<UHugeTuple, UHugeTemplate>>(sp => sp.GetRequiredService<UHugeAgent>());

            builder.Services.AddHostedService<ObserverProcessor<UHugeTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<UInt128, UHugeTuple, UHugeTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<UHugeTuple, UHugeTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<UHugeTuple, UHugeTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.UInt))
        {
            builder.Services.AddSingleton<ObserverRegistry<UIntTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<uint, UIntTuple, UIntTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<UIntTuple>>();
            builder.Services.AddSingleton<ObserverChannel<UIntTuple>>();
            builder.Services.AddSingleton<CallbackChannel<UIntTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<UIntTuple, UIntTemplate>>();

            builder.Services.AddSingleton<UIntAgent>();
            builder.Services.AddSingleton<ISpaceAgent<uint, UIntTuple, UIntTemplate>>(sp => sp.GetRequiredService<UIntAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<UIntTuple>>(sp => sp.GetRequiredService<UIntAgent>());
            builder.Services.AddSingleton<ITupleRouter<UIntTuple, UIntTemplate>>(sp => sp.GetRequiredService<UIntAgent>());

            builder.Services.AddHostedService<ObserverProcessor<UIntTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<uint, UIntTuple, UIntTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<UIntTuple, UIntTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<UIntTuple, UIntTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.ULong))
        {
            builder.Services.AddSingleton<ObserverRegistry<ULongTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<ulong, ULongTuple, ULongTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<ULongTuple>>();
            builder.Services.AddSingleton<ObserverChannel<ULongTuple>>();
            builder.Services.AddSingleton<CallbackChannel<ULongTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<ULongTuple, ULongTemplate>>();

            builder.Services.AddSingleton<ULongAgent>();
            builder.Services.AddSingleton<ISpaceAgent<ulong, ULongTuple, ULongTemplate>>(sp => sp.GetRequiredService<ULongAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<ULongTuple>>(sp => sp.GetRequiredService<ULongAgent>());
            builder.Services.AddSingleton<ITupleRouter<ULongTuple, ULongTemplate>>(sp => sp.GetRequiredService<ULongAgent>());

            builder.Services.AddHostedService<ObserverProcessor<ULongTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<ulong, ULongTuple, ULongTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<ULongTuple, ULongTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<ULongTuple, ULongTemplate>>();
        }

        if (options.EnabledSpaces.HasFlag(SpaceKind.UShort))
        {
            builder.Services.AddSingleton<ObserverRegistry<UShortTuple>>();
            builder.Services.AddSingleton<CallbackRegistry<ushort, UShortTuple, UShortTemplate>>();

            builder.Services.AddSingleton<EvaluationChannel<UShortTuple>>();
            builder.Services.AddSingleton<ObserverChannel<UShortTuple>>();
            builder.Services.AddSingleton<CallbackChannel<UShortTuple>>();
            builder.Services.AddSingleton<ContinuationChannel<UShortTuple, UShortTemplate>>();

            builder.Services.AddSingleton<UShortAgent>();
            builder.Services.AddSingleton<ISpaceAgent<ushort, UShortTuple, UShortTemplate>>(sp => sp.GetRequiredService<UShortAgent>());
            builder.Services.AddSingleton<ITupleActionReceiver<UShortTuple>>(sp => sp.GetRequiredService<UShortAgent>());
            builder.Services.AddSingleton<ITupleRouter<UShortTuple, UShortTemplate>>(sp => sp.GetRequiredService<UShortAgent>());

            builder.Services.AddHostedService<ObserverProcessor<UShortTuple>>();
            builder.Services.AddHostedService<CallbackProcessor<ushort, UShortTuple, UShortTemplate>>();
            builder.Services.AddHostedService<EvaluationProcessor<UShortTuple, UShortTemplate>>();
            builder.Services.AddHostedService<ContinuationProcessor<UShortTuple, UShortTemplate>>();
        }

        return builder;
    }
}