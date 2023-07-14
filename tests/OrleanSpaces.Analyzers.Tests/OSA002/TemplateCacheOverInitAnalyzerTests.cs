﻿using OrleanSpaces.Analyzers.OSA002;

namespace OrleanSpaces.Analyzers.Tests.OSA002;

public class TemplateCacheOverInitAnalyzerTests : AnalyzerFixture
{
    public TemplateCacheOverInitAnalyzerTests() : base(
        new TemplateCacheOverInitAnalyzer(),
        TemplateCacheOverInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        var diagnostic = TemplateCacheOverInitAnalyzer.Diagnostic;

        Assert.Equal("OSA002", diagnostic.Id);
        Assert.Equal(Categories.Performance, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, diagnostic.DefaultSeverity);
        Assert.Equal("Avoid constructor instantiation of template having only 'null' type arguments.", diagnostic.Title);
        Assert.Equal("Avoid constructor instantiation of template having only 'null' type arguments.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("SpaceTemplate template = new([|null|]);")]
    [InlineData("SpaceTemplate template = new([|null, null|]);")]
    [InlineData("SpaceTemplate template = new([|null, null, null|]);")]
    public void Should_Diagnose_Space_Template(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("IntTemplate template = new([|null|]);")]
    [InlineData("IntTemplate template = new([|null, null|]);")]
    [InlineData("IntTemplate template = new([|null, null, null|]);")]
    public void Should_Diagnose_Specialized_Template(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples_Specialized);

    [Theory]
    [InlineData("SpaceTemplate template = new([|1|]);")]
    [InlineData("SpaceTemplate template = new([|1, null|]);")]
    [InlineData("SpaceTemplate template = new([|1, null, null|]);")]
    [InlineData("SpaceTemplate template = new([|null, 1|]);")]
    [InlineData("SpaceTemplate template = new([|null, 1, null|]);")]
    [InlineData("SpaceTemplate template = new([|null, 1, null, null|]);")]
    [InlineData("SpaceTemplate template = new([|null, 1, null, 1, null|]);")]
    public void Should_Not_Diagnose_Space_Template(string code) =>
        NoDiagnostic(code, Namespace.OrleanSpaces_Tuples);

    [Theory]
    [InlineData("IntTemplate template = new([|1|]);")]
    [InlineData("IntTemplate template = new([|1, null|]);")]
    [InlineData("IntTemplate template = new([|1, null, null|]);")]
    [InlineData("IntTemplate template = new([|null, 1|]);")]
    [InlineData("IntTemplate template = new([|null, 1, null|]);")]
    [InlineData("IntTemplate template = new([|null, 1, null, null|]);")]
    [InlineData("IntTemplate template = new([|null, 1, null, 1, null|]);")]
    public void Should_Not_Diagnose_Specialized_Template(string code) =>
        NoDiagnostic(code, Namespace.OrleanSpaces_Tuples_Specialized);
}
