using OrleanSpaces.Tuples;

namespace AnalyzersOffenders.OSA002;

//public class OSA002
//{
//    SpaceTemplate template1 = new();
//    SpaceTemplate template2 = new(SpaceUnit.Null);
//    SpaceTemplate template3 = new(SpaceUnit.Null, SpaceUnit.Null);
//    SpaceTemplate template4 = new(SpaceUnit.Null, new SpaceUnit(), SpaceUnit.Null);
//}

public class OSA002
{
    SpaceTemplate template1 = new(SpaceUnit.Null);
    SpaceTemplate template3 = SpaceTemplateCache.Tuple_2;
}