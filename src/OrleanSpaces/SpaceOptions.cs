namespace OrleanSpaces;

/// <summary>
/// Options to configure general space functionalities
/// </summary>
public sealed class SpaceOptions
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
    /// The agent options.
    /// </summary>
    public SpaceAgentOptions AgentOptions { get; set; } = new();
}

/// <summary>
/// Options to configure agent functionalities.
/// </summary>
public sealed class SpaceAgentOptions
{
    /// <summary>
    /// If set to <see langword="true"/>, allows multiple consumers (client code) to read from the stream of tuples provided by:
    /// <list type="bullet">
    /// <item><description><see cref="ISpaceAgent.PeekAsync()"/></description></item>
    /// <item><description><see cref="ISpaceAgent{T, TTuple, TTemplate}.PeekAsync()"/></description></item>
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
    /// The execution mode.
    /// </summary>
    public AgentExecutionMode ExecutionMode { get; set; } = AgentExecutionMode.Adaptable;

    /// <summary>
    /// The period to trigger an optimization of the <see cref="AgentExecutionMode"/>.
    /// </summary>
    /// <remarks><i>Value is ignored if <see cref="ExecutionMode"/> is not <see cref="AgentExecutionMode.Adaptable"/>.</i></remarks>
    public TimeSpan OptimizationTriggerPeriod { get; set; } = TimeSpan.FromMinutes(1);
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
/// The mode in which an <see cref="ISpaceAgent"/> or <see cref="ISpaceAgent{T, TTuple, TTemplate}"/> runs.
/// </summary>
public enum AgentExecutionMode
{
    /// <summary>
    /// Agent is calibrated once to perform better on read operations.
    /// </summary>
    ReadOptimized = 0,
    /// <summary>
    /// Agent is calibrated once to perform better on write operations.
    /// </summary>
    WriteOptimized = 1,
    /// <summary>
    /// Agent is calibrated periodically to balance performance between read and write operations.
    /// </summary>
    Adaptable = 2
}