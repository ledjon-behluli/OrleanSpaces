using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers;

internal static class DiagnosticDescriptors
{
    public const string CategoryUsage = "Usage";
    public const string PerformanceCategory = "Performance";

    public static readonly DiagnosticDescriptor DefaultSpaceTupleCtorDiagnostic = new(
        id: "TS001",
        category: PerformanceCategory,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "Prefer using 'SpaceTuple.Null' over default constructor to avoid unneccessary memory allocations.",
        messageFormat: "Prefer using 'SpaceTuple.Null' over default constructor to avoid unneccessary memory allocations.");
}
