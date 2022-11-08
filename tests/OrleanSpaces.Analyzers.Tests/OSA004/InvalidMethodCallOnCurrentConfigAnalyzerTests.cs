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

    #region Identifier Expression

    [Theory]
    [InlineData("void M(ISpaceAgent agent) => [|agent.PeekAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("void M(ISpaceAgent agent) => [|agent.PopAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("void M(ISpaceAgent agent) => [|agent.EvaluateAsync(template, () => Task.FromResult(tuple))|];")]
    public void Should_Diagnose_On_IdentifierExpression(string code) =>
        HasDiagnostic(ComposeCodeForIdentifierExpression(code), Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("void M(ISpaceAgent agent) => [|agent.PeekAsync(template)|];")]
    [InlineData("void M(ISpaceAgent agent) => [|agent.PopAsync(template)|];")]
    public void Should_Not_Diagnose_NonCallbackOverload_On_IdentifierExpression(string code) =>
        NoDiagnostic(ComposeCodeForIdentifierExpression(code), Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);

    private static string ComposeCodeForIdentifierExpression(string code) => @$"
SpaceTemplate template = new(1);
SpaceTuple tuple = new SpaceTuple(1);

{code}";

    #endregion

    #region Invocation Expression

    [Theory]
    [InlineData("void M() => [|GetAgent().PeekAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("void M() => [|GetAgent().PopAsync(template, tuple => Task.CompletedTask)|];")]
    [InlineData("void M() => [|GetAgent().EvaluateAsync(template, () => Task.FromResult(tuple))|];")]
    public void Should_Diagnose_On_InvocationExpression(string code) =>
        HasDiagnostic(ComposeCodeForInvocationExpression(code), Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("void M() => [|GetAgent().PeekAsync(template)|];")]
    [InlineData("void M() => [|GetAgent().PopAsync(template)|];")]
    public void Should_Not_Diagnose_NonCallbackOverload_On_InvocationExpression(string code) =>
        NoDiagnostic(ComposeCodeForInvocationExpression(code), Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);

    private static string ComposeCodeForInvocationExpression(string code) => @$"
SpaceTemplate template = new(1);
SpaceTuple tuple = new SpaceTuple(1);

{code}

static ISpaceAgent GetAgent() => (ISpaceAgent)new object();";

    #endregion

    #region Member Access Expression

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
    public void Should_Diagnose_On_MemberAccessExpression(string code) =>
        HasDiagnostic(ComposeCodeForMemberAccessExpression(code), Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);

    [Theory]
    // Field
    [InlineData("[|c.agent.PeekAsync(template)|];")]
    [InlineData("[|c.agent.PopAsync(template)|];")]
    // Property
    [InlineData("[|c.Agent.PeekAsync(template)|];")]
    [InlineData("[|c.Agent.PopAsync(template)|];")]
    // Method
    [InlineData("[|c.GetAgent().PeekAsync(template)|];")]
    [InlineData("[|c.GetAgent().PopAsync(template)|];")]
    public void Should_Not_Diagnose_NonCallbackOverload_On_MemberAccessExpression(string code) =>
        NoDiagnostic(ComposeCodeForMemberAccessExpression(code), Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);

    private static string ComposeCodeForMemberAccessExpression(string code) => @$"
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


    #endregion

    [Fact]
    public void A()
    {
        string code = @"
public class Program
{
    public static async Task Main(string[] args)
    {
        C c = new();
        [|c.Agent.PeekAsync(new(1), tuple => Task.CompletedTask)|];

        await Host.CreateDefaultBuilder(args).Build().RunAsync();
    }
}

class C
{
    public ISpaceAgent Agent => (ISpaceAgent)new object();
}";

        HasDiagnostic(code, Namespace.OrleansSpaces, Namespace.OrleanSpaces_Tuples);
    }
}
