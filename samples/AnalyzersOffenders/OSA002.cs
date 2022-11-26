using OrleanSpaces.Tuples;

namespace AnalyzersOffenders;

public class OSA002
{
    SpaceTemplate template0 = new();
    SpaceTemplate template1 = new(new SpaceUnit());
    SpaceTemplate template2 = new(new SpaceUnit(), new SpaceUnit(), 1);
    SpaceTemplate template3 = new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit());
}