using OrleanSpaces.Analyzers.OSA003;

namespace OrleanSpaces.Analyzers.Tests.OSA003;

public class AllUnitArgsTemplateInitAnalyzerTests : AnalyzerFixture
{
	public AllUnitArgsTemplateInitAnalyzerTests() : base(
		new AllUnitArgsTemplateInitAnalyzer(),
		AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id)
	{

	}

	[Fact]
    public void Should_Equal()
    {
        Assert.Equal("OSA003", AllUnitArgsTemplateInitAnalyzer.Diagnostic.Id);
        Assert.Equal(Categories.Performance, AllUnitArgsTemplateInitAnalyzer.Diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, AllUnitArgsTemplateInitAnalyzer.Diagnostic.DefaultSeverity);
        Assert.Equal("Avoid constructor instantiation of 'SpaceTemplate' having only 'SpaceUnit' type arguments.", AllUnitArgsTemplateInitAnalyzer.Diagnostic.Title);
        Assert.Equal("Avoid constructor instantiation of 'SpaceTemplate' having only 'SpaceUnit' type arguments.", AllUnitArgsTemplateInitAnalyzer.Diagnostic.MessageFormat);
        Assert.True(AllUnitArgsTemplateInitAnalyzer.Diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("SpaceTemplate template = new([||]);")]
    [InlineData("SpaceTemplate template = new([|new SpaceUnit()|]);")]
    [InlineData("SpaceTemplate template = new([|new SpaceUnit(), new SpaceUnit()|]);")]
    [InlineData("SpaceTemplate template = new([|SpaceUnit.Null|]);")]
    [InlineData("SpaceTemplate template = new([|SpaceUnit.Null, SpaceUnit.Null|]);")]
    [InlineData("SpaceTemplate template = new([|SpaceUnit.Null, new SpaceUnit(), SpaceUnit.Null|]);")]
    public void Should_Diagnose_SpaceTemplate(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);
}
