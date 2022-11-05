using OrleanSpaces.Analyzers.OSA004;

namespace OrleanSpaces.Analyzers.Tests.OSA004;

public class InvalidMethodCallOnCurrentConfigAnalyzerTests : AnalyzerFixture
{
	public InvalidMethodCallOnCurrentConfigAnalyzerTests() : base(
		new InvalidMethodCallOnCurrentConfigAnalyzer(),
		InvalidMethodCallOnCurrentConfigAnalyzer.Diagnostic.Id)
	{

	}

	[Fact]
	public void Should_Equal()
    {
        var diagnostic = InvalidMethodCallOnCurrentConfigAnalyzer.Diagnostic;

        Assert.Equal("OSA004", diagnostic.Id);
        Assert.Equal(Categories.Usage, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Warning, diagnostic.DefaultSeverity);
        Assert.Equal("Method is not supported on the configured environment.", diagnostic.Title);
        Assert.Equal("Method '{0}' is not supported on the configured environment.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Fact]
    public void A()
    {
        string code = @"
SpaceTemplate template = new(1);

void M(ISpaceAgent agent) => [|agent.PeekAsync(template)|];";

        HasDiagnostic(code, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void B()
    {
        string code = @"
SpaceTemplate template = new(1);
	
void M() => [|GetAgent().PeekAsync(template)|];

static ISpaceAgent GetAgent() => (ISpaceAgent)new object();";

        HasDiagnostic(code, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void C()
    {
        string code = @"
SpaceTemplate template = new(1);
	
void M(ISpaceAgent agent)
{
    X x = new();
    [|x.Agent.PeekAsync(template)|];
}

class X
{
    public ISpaceAgent Agent => (ISpaceAgent)new object();
}";

        HasDiagnostic(code, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void D()
    {
        string code = @"
SpaceTemplate template = new(1);
	
void M(ISpaceAgent agent)
{
    X x = new();
    [|x.Agent.PeekAsync(template)|];
}

class X
{
    public ISpaceAgent Agent = (ISpaceAgent)new object();
}";

        HasDiagnostic(code, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }
}
