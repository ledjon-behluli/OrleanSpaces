using OrleanSpaces.Tuples;

namespace AnalyzersOffenders;

public class OSA003
{
    SpaceTemplate template1 = new();
    SpaceTemplate template2 = new(SpaceUnit.Null);
    SpaceTemplate template3 = SpaceTemplateCache.Tuple_2;
}

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
    private static readonly SpaceTemplate tuple_2 = new(SpaceUnit.Null, SpaceUnit.Null);
    private static readonly SpaceTemplate tuple_3 = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
    public static ref readonly SpaceTemplate Tuple_3 => ref tuple_3;
}