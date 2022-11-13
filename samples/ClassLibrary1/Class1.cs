using OrleanSpaces.Tuples;

namespace ClassLibrary1;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly SpaceTemplate tuple_2 = new(SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}