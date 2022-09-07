using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

internal interface ISpaceElement : ITuple
{
    ReadOnlySpan<object> AsReadOnlySpan();
}