using OrleanSpaces.Tuples;

namespace AnalyzersOffenders;

public class OSA003
{
    SpaceTemplate template1 = SpaceTemplateCache.Tuple_1;
    SpaceTemplate template2 = SpaceTemplateCache.Tuple_2;
}
public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
    private static readonly SpaceTemplate tuple_2 = new(SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}