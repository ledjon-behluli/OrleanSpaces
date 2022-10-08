using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers;

internal static class DiagnosticDescriptors
{
    public const string CategoryUsage = "Usage";
    public const string PerformanceCategory = "Performance";

    public static readonly DiagnosticDescriptor SpaceTuple_DefaultCtorDiagnostic = new(
        id: "OSA001",
        category: PerformanceCategory,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "Avoid using default constructor.",
        messageFormat: "Do not use default constructor in order to avoid unneccessary memory allocations.");
}
