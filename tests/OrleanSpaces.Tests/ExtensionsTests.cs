using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests;

public class ExtensionsTests
{
    [Fact]
    public void Should_Register_Generic_Services()
    {
        IHost host = CreateHost(SpaceKind.Generic);

        Assert.NotNull(host.Services.GetService<CallbackRegistry>());
        Assert.NotNull(host.Services.GetService<ObserverRegistry<SpaceTuple>>());

        Assert.NotNull(host.Services.GetService<CallbackChannel<SpaceTuple>>());
        Assert.NotNull(host.Services.GetService<EvaluationChannel<SpaceTuple>>());
        Assert.NotNull(host.Services.GetService<ContinuationChannel<SpaceTuple, SpaceTemplate>>());
        Assert.NotNull(host.Services.GetService<ObserverChannel<SpaceTuple>>());

        Assert.NotNull(host.Services.GetService<ISpaceAgent>());
        Assert.NotNull(host.Services.GetService<ISpaceAgentProvider>());
        Assert.NotNull(host.Services.GetService<ITupleRouter<SpaceTuple, SpaceTemplate>>());

        Assert.All(host.Services.GetService<IEnumerable<IHostedService>>(), service =>
        {
            Assert.NotNull(service);
            Assert.True(
                service.GetType() == typeof(CallbackProcessor) ||
                service.GetType() == typeof(EvaluationProcessor<SpaceTuple, SpaceTemplate>) ||
                service.GetType() == typeof(ContinuationProcessor<SpaceTuple, SpaceTemplate>) ||
                service.GetType() == typeof(ObserverProcessor<SpaceTuple>));
        });
    }

    [Fact]
    public void Should_Register_Specialized_Services()
    {
        IHost host = CreateHost(SpaceKind.All);

        EnsureAdded<bool, BoolTuple, BoolTemplate>(host);
        EnsureAdded<byte, ByteTuple, ByteTemplate>(host);
        EnsureAdded<sbyte, SByteTuple, SByteTemplate>(host);
        EnsureAdded<char, CharTuple, CharTemplate>(host);
        EnsureAdded<decimal, DecimalTuple, DecimalTemplate>(host);
        EnsureAdded<double, DoubleTuple, DoubleTemplate>(host);
        EnsureAdded<float, FloatTuple, FloatTemplate>(host);
        EnsureAdded<int, IntTuple, IntTemplate>(host);
        EnsureAdded<uint, UIntTuple, UIntTemplate>(host);
        EnsureAdded<long, LongTuple, LongTemplate>(host);
        EnsureAdded<ulong, ULongTuple, ULongTemplate>(host);
        EnsureAdded<short, ShortTuple, ShortTemplate>(host);
        EnsureAdded<ushort, UShortTuple, UShortTemplate>(host);
        EnsureAdded<Guid, GuidTuple, GuidTemplate>(host);
        EnsureAdded<TimeSpan, TimeSpanTuple, TimeSpanTemplate>(host);
        EnsureAdded<DateTime, DateTimeTuple, DateTimeTemplate>(host);
        EnsureAdded<DateTimeOffset, DateTimeOffsetTuple, DateTimeOffsetTemplate>(host);
        EnsureAdded<Int128, HugeTuple, HugeTemplate>(host);
        EnsureAdded<UInt128, UHugeTuple, UHugeTemplate>(host);
    }

    static void EnsureAdded<T, TTuple, TTemplate>(IHost host)
        where T : unmanaged
        where TTuple : struct, ISpaceTuple<T>
        where TTemplate : struct, ISpaceTemplate<T>
    {
        Assert.NotNull(host.Services.GetService<CallbackRegistry<T, TTuple, TTemplate>>());
        Assert.NotNull(host.Services.GetService<ObserverRegistry<TTuple>>());

        Assert.NotNull(host.Services.GetService<CallbackChannel<TTuple>>());
        Assert.NotNull(host.Services.GetService<EvaluationChannel<TTuple>>());
        Assert.NotNull(host.Services.GetService<ContinuationChannel<TTuple, TTemplate>>());
        Assert.NotNull(host.Services.GetService<ObserverChannel<TTuple>>());

        Assert.NotNull(host.Services.GetService<ISpaceAgent<T, TTuple, TTemplate>>());
        Assert.NotNull(host.Services.GetService<ISpaceAgentProvider<T, TTuple, TTemplate>>());
        Assert.NotNull(host.Services.GetService<ITupleRouter<TTuple, TTemplate>>());

        Assert.All(host.Services.GetService<IEnumerable<IHostedService>>(), service =>
        {
            Assert.NotNull(service);
            Assert.True(
                service.GetType() == typeof(CallbackProcessor<T, TTuple, TTemplate>) ||
                service.GetType() == typeof(EvaluationProcessor<TTuple, TTemplate>) ||
                service.GetType() == typeof(ContinuationProcessor<TTuple, TTemplate>) ||
                service.GetType() == typeof(ObserverProcessor<TTuple>));
        });
    }

    static IHost CreateHost(SpaceKind kind)
    {
        return new HostBuilder()
            .UseOrleansClient(builder =>
                builder.AddOrleanSpaces(options => options.EnabledSpaces = kind))
            .Build();
    }
}