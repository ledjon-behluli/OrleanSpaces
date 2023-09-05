using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests;

public class ExtensionTests
{
    [Fact]
    public void Should_Register_Generic_Services()
    {
        TestServices(CreateClientHost(SpaceKind.Generic));
        TestServices(CreateSiloHost(SpaceKind.Generic));

        static void TestServices(IHost host)
        {
            Assert.NotNull(host.Services.GetService<SpaceClientOptions>());

            Assert.NotNull(host.Services.GetService<CallbackRegistry>());
            Assert.NotNull(host.Services.GetService<ObserverRegistry<SpaceTuple>>());

            Assert.NotNull(host.Services.GetService<CallbackChannel<SpaceTuple>>());
            Assert.NotNull(host.Services.GetService<EvaluationChannel<SpaceTuple>>());
            Assert.NotNull(host.Services.GetService<ContinuationChannel<SpaceTuple, SpaceTemplate>>());
            Assert.NotNull(host.Services.GetService<ObserverChannel<SpaceTuple>>());

            Assert.NotNull(host.Services.GetService<ISpaceAgent>());
            Assert.NotNull(host.Services.GetService<ISpaceRouter<SpaceTuple, SpaceTemplate>>());
        }
    }

    [Fact]
    public void Should_Register_Specialized_Services()
    {
        TestServices(CreateClientHost(SpaceKind.All));
        TestServices(CreateSiloHost(SpaceKind.All));

        static void TestServices(IHost host)
        {
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
    }

    static void EnsureAdded<T, TTuple, TTemplate>(IHost host)
        where T : unmanaged
        where TTuple : struct, ISpaceTuple<T>, ISpaceConvertible<T, TTemplate>
        where TTemplate : struct, ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
    {
        Assert.NotNull(host.Services.GetService<SpaceClientOptions>());

        Assert.NotNull(host.Services.GetService<CallbackRegistry<T, TTuple, TTemplate>>());
        Assert.NotNull(host.Services.GetService<ObserverRegistry<TTuple>>());

        Assert.NotNull(host.Services.GetService<CallbackChannel<TTuple>>());
        Assert.NotNull(host.Services.GetService<EvaluationChannel<TTuple>>());
        Assert.NotNull(host.Services.GetService<ContinuationChannel<TTuple, TTemplate>>());
        Assert.NotNull(host.Services.GetService<ObserverChannel<TTuple>>());

        Assert.NotNull(host.Services.GetService<ISpaceAgent<T, TTuple, TTemplate>>());
        Assert.NotNull(host.Services.GetService<ISpaceRouter<TTuple, TTemplate>>());
    }

    static IHost CreateClientHost(SpaceKind kind)
        => new HostBuilder()
            .UseOrleansClient(builder => 
                builder.AddOrleanSpaces(options => options.EnabledSpaces = kind))
            .Build();

    static IHost CreateSiloHost(SpaceKind kind)
        => new HostBuilder()
            .UseOrleans(builder => 
                builder.AddOrleanSpaces(configureClientOptions: options => options.EnabledSpaces = kind))
            .Build();
}