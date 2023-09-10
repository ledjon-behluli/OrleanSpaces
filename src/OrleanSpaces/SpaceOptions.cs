namespace OrleanSpaces;

/// <summary>
/// Options to configure server functionalities.
/// </summary>
public sealed class SpaceServerOptions
{
    /// <summary>
    /// Defines the maximum number of tuples that should be stored within a partition per <see cref="SpaceKind"/>.
    /// </summary>
    public int PartitioningThreshold { get; set; } = 1_000;
}

/// <summary>
/// Options to configure client functionalities.
/// </summary>
public sealed class SpaceClientOptions
{
    /// <summary>
    /// Determines which of the <see cref="SpaceKind"/>(s) are enabled.
    /// </summary>
    public SpaceKind EnabledSpaces { get; set; } = SpaceKind.Generic;

    /// <summary>
    /// If set to <see langword="true"/>, catches all exceptions that happen inside:
    /// <list type="bullet">
    /// <item><description><see cref="ISpaceAgent.EvaluateAsync(Func{Task{Tuples.SpaceTuple}})"/></description></item>
    /// <item><description><see cref="ISpaceAgent{T, TTuple, TTemplate}.EvaluateAsync(Func{Task{TTuple}})"/></description></item>
    /// </list>
    /// </summary>
    public bool HandleEvaluationExceptions { get; set; } = true;

    /// <summary>
    /// If set to <see langword="true"/>, catches all exceptions that happen inside:
    /// <list type="bullet">
    /// <item><description><see cref="ISpaceAgent.PeekAsync(Tuples.SpaceTemplate, Func{Tuples.SpaceTuple, Task})"/></description></item>
    /// <item><description><see cref="ISpaceAgent.PopAsync(Tuples.SpaceTemplate, Func{Tuples.SpaceTuple, Task})"/></description></item>
    /// <item><description><see cref="ISpaceAgent{T, TTuple, TTemplate}.PeekAsync(TTemplate, Func{TTuple, Task})"/></description></item>
    ///  <item><description><see cref="ISpaceAgent{T, TTuple, TTemplate}.PopAsync(TTemplate, Func{TTuple, Task})"/></description></item>
    /// </list>
    /// </summary>
    public bool HandleCallbackExceptions { get; set; } = true;

    /// <summary>
    /// If set to <see langword="true"/>, allows multiple consumers (client code) to read from the stream of tuples provided by:
    /// <list type="bullet">
    /// <item><description><see cref="ISpaceAgent.EnumerateAsync()"/></description></item>
    /// <item><description><see cref="ISpaceAgent{T, TTuple, TTemplate}.EnumerateAsync()"/></description></item>
    /// </list>
    /// </summary>
    public bool AllowMultipleAgentStreamConsumers { get; set; } = true;

    /// <summary>
    /// If set to <see langword="true"/>, allows implementations of <see cref="ISpaceObserver{T}"/> that live in the same
    /// process as the agent, to receive notifications of tuples that have been written by the same agent, otherwise if <see langword="false"/>
    /// than the <see cref="ISpaceObserver{T}"/> will receive only notifications that have been written by an agent in a different process.
    /// </summary>
    public bool SubscribeToSelfGeneratedTuples { get; set; } = true;

    /// <summary>
    /// If set to <see langword="true"/>, the agent loads the space contents (<i>i.e. the tuples</i>) upon its startup.
    /// </summary>
    /// <remarks><i>
    /// It is useful in cases where the agent is used to perform only writes, or the application needs fast startup times.
    /// Space contents can always be reloaded via <see cref="ISpaceAgent.ReloadAsync"/> or 
    /// <see cref="ISpaceAgent{T, TTuple, TTemplate}.ReloadAsync"/>, depending on the agent type.
    /// </i></remarks>
    public bool LoadSpaceContentsUponStartup { get; set; } = true;

    /// <summary>
    /// Defines the way how the space contents (<i>i.e. the tuples</i>) are loaded by the agent.
    /// </summary>
    public SpaceLoadingStrategy LoadingStrategy { get; set; } = SpaceLoadingStrategy.Parallel;
}

/// <summary>
/// The kind of the tuple space.
/// </summary>
[Flags]
public enum SpaceKind
{
    All = Generic | Bool | Byte | Char | DateTimeOffset | DateTime | Decimal | Double | Float | Guid | Huge | Int | Long | SByte | Short | TimeSpan | UHuge | UInt | ULong | UShort,
    Generic = 1,
    Bool = 2,
    Byte = 4,
    Char = 8,
    DateTimeOffset = 16,
    DateTime = 32,
    Decimal = 64,
    Double = 128,
    Float = 256,
    Guid = 512,
    Huge = 1024,
    Int = 2048,
    Long = 4096,
    SByte = 8192,
    Short = 16384,
    TimeSpan = 32768,
    UHuge = 65536,
    UInt = 131072,
    ULong = 262144,
    UShort = 524288
}

/// <summary>
/// Strategies for space loading.
/// </summary>
public enum SpaceLoadingStrategy
{
    /// <summary>
    /// <para>Content from all partitions are loaded sequentially.</para>
    /// <para>Results in a longer time to load the space. But ultimately results in less resource contention, and avoids potential <see cref="ThreadPool"/> starvation.</para>
    /// </summary>
    /// <remarks><i>Use if fast loading time is not important, and the space is heavily partitioned.</i></remarks>
    Sequential,
    /// <summary>
    /// <para>Content from all partitions are loaded in parallel.</para>
    /// <para>Results in less resource contention, and avoids potential <see cref="ThreadPool"/> starvation. But ultimately results in a longer time to load the space.</para>
    /// </summary>
    /// <remarks><i>Use if fast loading time is important, and the space is not heavily partitioned.</i></remarks>
    Parallel
}