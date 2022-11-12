using OrleanSpaces;
using OrleanSpaces.Tuples;
using System.Threading.Tasks;

namespace WebApplication_3_1
{
    public class MyClass
    {
        SpaceTemplate template = new(1);

        public async Task Test1(ISpaceAgent agent)
        {
            await agent.PeekAsync(template);
            await agent.PeekAsync(template, _ => Task.CompletedTask);
        }
    }
}
