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
        Assert.Equal("Method is not supported with the current configuration.", diagnostic.Title);
        Assert.Equal("Method '{0}' is not supported with the current configuration.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("void M(ISpaceAgent agent) => [|agent.PeekAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("void M(ISpaceAgent agent) => [|agent.PopAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("void M(ISpaceAgent agent) => [|agent.EvaluateAsync(template, () => Task.FromResult(tuple))|];")]
    public void Should_Diagnose_On_IdentifierExpression(string code)
    {
        string newCode = @$"
SpaceTemplate template = new(1);
SpaceTuple tuple = new SpaceTuple(1);

{code}";

        HasDiagnostic(newCode, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }

    [Theory]
    [InlineData("void M() => [|GetAgent().PeekAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("void M() => [|GetAgent().PopAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("void M() => [|GetAgent().EvaluateAsync(template, () => Task.FromResult(tuple))|];")]
    public void Should_Diagnose_On_InvocationExpression(string code)
    {
        string newCode = @$"
SpaceTemplate template = new(1);
SpaceTuple tuple = new SpaceTuple(1);

{code}

static ISpaceAgent GetAgent() => (ISpaceAgent)new object();";

        HasDiagnostic(newCode, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }

    [Theory]
    // Field
    [InlineData("[|c.agent.PeekAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("[|c.agent.PopAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("[|c.agent.EvaluateAsync(template, () => Task.FromResult(tuple))|];")]
    // Property
    [InlineData("[|c.Agent.PeekAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("[|c.Agent.PopAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("[|c.Agent.EvaluateAsync(template, () => Task.FromResult(tuple))|];")]
    // Method
    [InlineData("[|c.GetAgent().PeekAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("[|c.GetAgent().PopAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("[|c.GetAgent().EvaluateAsync(template, () => Task.FromResult(tuple))|];")]
    public void Should_Diagnose_On_MemberAccessExpression(string code)
    {
        string newCode = @$"
SpaceTemplate template = new(1);
SpaceTuple tuple = new SpaceTuple(1);
	
void M()
{{
    C c = new();
    {code}
}}

class C
{{
    public ISpaceAgent agent = (ISpaceAgent)new object();
    public ISpaceAgent Agent => (ISpaceAgent)new object();
    public ISpaceAgent GetAgent() => (ISpaceAgent)new object();
}}";

        HasDiagnostic(newCode, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void A()
    {
        string code = @$"
SpaceTemplate template = new(1);
SpaceTuple tuple = new SpaceTuple(1);
	
void M()
{{
    C c = new();
    Func<ISpaceAgent> func = () => c.GetAgent();

    [|func().PeekAsync(template, tuple => Task.CompletedTask)|];
}}

class C
{{
    public ISpaceAgent agent = (ISpaceAgent)new object();
    public ISpaceAgent Agent => (ISpaceAgent)new object();
    public ISpaceAgent GetAgent() => (ISpaceAgent)new object();
}}";

        HasDiagnostic(code, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }
}
