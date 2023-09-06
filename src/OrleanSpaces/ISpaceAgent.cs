using OrleanSpaces.Tuples;

namespace OrleanSpaces;

/// <summary>
/// Provides functionalities to interact with the tuple space.
/// </summary>
public interface ISpaceAgent
{
    /// <summary>
    /// Returns the total number of <see cref="SpaceTuple"/>'s in the space. 
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Enables the <paramref name="observer"/> to subscribe to events that happen in the tuple space.
    /// </summary>
    /// <param name="observer">Any space observer.</param>
    /// <remarks><i>Method is idempotant.</i></remarks>
    /// <returns>An ID that can be used to <see cref="Unsubscribe"/>.</returns>
    Guid Subscribe(ISpaceObserver<SpaceTuple> observer);

    /// <summary>
    /// Removes the observer with the corresponding <paramref name="observerId"/>.
    /// </summary>
    /// <param name="observerId">The ID obtained from calling <see cref="Subscribe"/>.</param>
    /// <remarks><i>Method is idempotant.</i></remarks>
    void Unsubscribe(Guid observerId);
    
    /// <summary>
    /// Directly writes the <paramref name="tuple"/> in the space.
    /// </summary>
    /// <param name="tuple">Any <see cref="SpaceTuple"/> with non-zero length.</param>
    Task WriteAsync(SpaceTuple tuple);
    
    /// <summary>
    /// Indirectly writes a <see cref="SpaceTuple"/> in the space.
    /// <list type="number">
    /// <item><description>Executes the <paramref name="evaluation"/> function.</description></item>
    /// <item><description>Proceeds to write the resulting <see cref="SpaceTuple"/> in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="evaluation">Any function that returns a <see cref="SpaceTuple"/> with non-zero length.</param>
    ValueTask EvaluateAsync(Func<Task<SpaceTuple>> evaluation);
    
    /// <summary>
    /// Reads a <see cref="SpaceTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, then a <u>copy</u> is returned thereby keeping the original in the space.</description></item>
    /// <item><description>Otherwise a <see cref="SpaceTuple"/> with zero length is returned to indicate a "no-match".</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <see cref="SpaceTuple"/>.</param>
    /// <returns><see cref="SpaceTuple"/> (potentially one with zero length).</returns>
    SpaceTuple Peek(SpaceTemplate template);
    
    /// <summary>
    /// Reads a <see cref="SpaceTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, the <paramref name="callback"/> is invoked immediately.</description></item>
    /// <item><description>Otherwise the operation is remembered and the <paramref name="callback"/> will eventually be invoked as soon as a matching <see cref="SpaceTuple"/> is written in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <see cref="SpaceTuple"/>.</param>
    /// <param name="callback">A callback function that will be executed, with the <see cref="SpaceTuple"/> as the argument.</param>
    /// <remarks><i>Same as with <see cref="Peek(SpaceTemplate)"/>, the original tuple is <u>kept</u> in the space once <paramref name="callback"/> gets invoked.</i></remarks>
    ValueTask PeekAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback);
    
    /// <summary>
    /// Reads a <see cref="SpaceTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, then the <u>original</u> is returned thereby removing it from the space.</description></item>
    /// <item><description>Otherwise a <see cref="SpaceTuple"/> with zero length is returned to indicate a "no-match".</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <see cref="SpaceTuple"/>.</param>
    /// <returns><see cref="SpaceTuple"/> (potentially one with zero length)>.</returns>
    ValueTask<SpaceTuple> PopAsync(SpaceTemplate template);
    
    /// <summary>
    /// Reads a <see cref="SpaceTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, the <paramref name="callback"/> is invoked immediately.</description></item>
    /// <item><description>Otherwise the operation is remembered and the <paramref name="callback"/> will eventually be invoked as soon as a matching <see cref="SpaceTuple"/> is written in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <see cref="SpaceTuple"/>.</param>
    /// <param name="callback">A callback function that will be executed, with the <see cref="SpaceTuple"/> as the argument.</param>
    /// <remarks><i>Same as with <see cref="PopAsync(SpaceTemplate)"/>, the original tuple is <u>removed</u> from the space once <paramref name="callback"/> gets invoked.</i></remarks>
    ValueTask PopAsync(SpaceTemplate template, Func<SpaceTuple, Task> callback);

    /// <summary>
    /// Enumerates over <see cref="SpaceTuple"/>'s that are potentially matched by the given <paramref name="template"/> in the space.
    /// </summary>
    /// <param name="template">A template that potentially matches multiple <see cref="SpaceTuple"/>'s.</param>
    /// <remarks><i>If <paramref name="template"/> is <see cref="default"/>, all tuples will be enumerated.</i></remarks>
    IEnumerable<SpaceTuple> Enumerate(SpaceTemplate template = default);

    /// <summary>
    /// Enumerates over a stream of <see cref="SpaceTuple"/>'s that are potentially matched by the given <paramref name="template"/> as they get written in the space.
    /// </summary>
    /// <param name="template">A template that potentially matches multiple <see cref="SpaceTuple"/>'s.</param>
    /// <remarks><i>If <paramref name="template"/> is <see cref="default"/>, all tuples will be enumerated.</i></remarks>
    IAsyncEnumerable<SpaceTuple> EnumerateAsync(SpaceTemplate template = default);

    /// <summary>
    /// Reloads all <see cref="SpaceTuple"/>'s from the space.
    /// </summary>
    Task ReloadAsync();

    /// <summary>
    /// Removes all <see cref="SpaceTuple"/>'s in the space.
    /// </summary>
    Task ClearAsync();
}

/// <summary>
/// Provides functionalities to interact with the tuple space with specialized tuples and templates.
/// </summary>
/// <typeparam name="T">The value type of the <typeparamref name="TTuple"/>.</typeparam>
/// <typeparam name="TTuple">The tuple type which must contains <typeparamref name="T"/> values only.</typeparam>
/// <typeparam name="TTemplate">The template type which must contains <typeparamref name="T"/> values only.</typeparam>
public interface ISpaceAgent<T, TTuple, TTemplate>
    where T : unmanaged
    where TTuple : struct, ISpaceTuple<T>
    where TTemplate : struct, ISpaceTemplate<T>
{
    /// <summary>
    /// Returns the total number of <see cref="TTuple"/>'s in the space. 
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Enables the <paramref name="observer"/> to subscribe to events that happen in the tuple space.
    /// </summary>
    /// <param name="observer">Any space observer.</param>
    /// <remarks><i>Method is idempotent.</i></remarks>
    /// <returns>An ID that can be used to <see cref="Unsubscribe"/>.</returns>
    Guid Subscribe(ISpaceObserver<TTuple> observer);

    /// <summary>
    /// Removes the observer with the corresponding <paramref name="observerId"/>.
    /// </summary>
    /// <param name="observerId">The ID obtained from calling <see cref="Subscribe"/>.</param>
    /// <remarks><i>Method is idempotent.</i></remarks>
    void Unsubscribe(Guid observerId);

    /// <summary>
    /// Directly writes the <paramref name="tuple"/> in the space.
    /// </summary>
    /// <param name="tuple">Any <typeparamref name="TTuple"/> with non-zero length.</param>
    Task WriteAsync(TTuple tuple);

    /// <summary>
    /// Indirectly writes a <typeparamref name="TTuple"/> in the space.
    /// <list type="number">
    /// <item><description>Executes the <paramref name="evaluation"/> function.</description></item>
    /// <item><description>Proceeds to write the resulting <typeparamref name="TTuple"/> in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="evaluation">Any function that returns a <typeparamref name="TTuple"/> with non-zero length.</param>
    ValueTask EvaluateAsync(Func<Task<TTuple>> evaluation);

    /// <summary>
    /// Reads a <typeparamref name="TTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, then a <u>copy</u> is returned, thereby keeping the original in the space.</description></item>
    /// <item><description>Otherwise, a <typeparamref name="TTuple"/> with zero length is returned to indicate a "no-match".</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <typeparamref name="TTuple"/>.</param>
    /// <returns><typeparamref name="TTuple"/> (potentially one with zero length).</returns>
    TTuple Peek(TTemplate template);

    /// <summary>
    /// Reads a <typeparamref name="TTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, the <paramref name="callback"/> is invoked immediately.</description></item>
    /// <item><description>Otherwise, the operation is remembered, and the <paramref name="callback"/> will eventually be invoked as soon as a matching <typeparamref name="TTuple"/> is written in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <typeparamref name="TTuple"/>.</param>
    /// <param name="callback">A callback function that will be executed, with the <typeparamref name="TTuple"/> as the argument.</param>
    /// <remarks><i>Same as with <see cref="Peek(TTemplate)"/>, the original tuple is <u>kept</u> in the space once <paramref name="callback"/> gets invoked.</i></remarks>
    ValueTask PeekAsync(TTemplate template, Func<TTuple, Task> callback);

    /// <summary>
    /// Reads a <typeparamref name="TTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, then the <u>original</u> is returned, thereby removing it from the space.</description></item>
    /// <item><description>Otherwise, a <typeparamref name="TTuple"/> with zero length is returned to indicate a "no-match".</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <typeparamref name="TTuple"/>.</param>
    /// <returns><typeparamref name="TTuple"/> (potentially one with zero length).</returns>
    ValueTask<TTuple> PopAsync(TTemplate template);

    /// <summary>
    /// Reads a <typeparamref name="TTuple"/> that is potentially matched by the given <paramref name="template"/>.
    /// <list type="bullet">
    /// <item><description>If one such tuple exists, the <paramref name="callback"/> is invoked immediately.</description></item>
    /// <item><description>Otherwise, the operation is remembered, and the <paramref name="callback"/> will eventually be invoked as soon as a matching <typeparamref name="TTuple"/> is written in the space.</description></item>
    /// </list>
    /// </summary>
    /// <param name="template">A template that potentially matches a <typeparamref name="TTuple"/>.</param>
    /// <param name="callback">A callback function that will be executed, with the <typeparamref name="TTuple"/> as the argument.</param>
    /// <remarks><i>Same as with <see cref="PopAsync(TTemplate)"/>, the original tuple is <u>removed</u> from the space once <paramref name="callback"/> gets invoked.</i></remarks>
    ValueTask PopAsync(TTemplate template, Func<TTuple, Task> callback);

    /// <summary>
    /// Enumerates over <typeparamref name="TTuple"/>'s that are potentially matched by the given <paramref name="template"/> in the space.
    /// </summary>
    /// <param name="template">A template that potentially matches multiple <typeparamref name="TTuple"/>'s.</param>
    /// <remarks><i>If <paramref name="template"/> is <see cref="default"/>, all tuples will be enumerated.</i></remarks>
    IEnumerable<TTuple> Enumerate(TTemplate template = default);

    /// <summary>
    /// Enumerates over a stream of <typeparamref name="TTuple"/>'s that are potentially matched by the given <paramref name="template"/> as they get written in the space.
    /// </summary>
    /// <param name="template">A template that potentially matches multiple <typeparamref name="TTuple"/>'s.</param>
    /// <remarks><i>If <paramref name="template"/> is <see cref="default"/>, all tuples will be enumerated.</i></remarks>
    IAsyncEnumerable<TTuple> EnumerateAsync(TTemplate template = default);

    /// <summary>
    /// Reloads all <see cref="TTuple"/>'s from the space.
    /// </summary>
    Task ReloadAsync();

    /// <summary>
    /// Removes all <typeparamref name="TTuple"/>'s in the space.
    /// </summary>
    Task ClearAsync();
}