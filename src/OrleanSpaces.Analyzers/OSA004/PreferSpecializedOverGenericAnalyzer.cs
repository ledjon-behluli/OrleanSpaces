using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA004;

/// <summary>
/// Reports to prefer specialized tuples & templates over the generic verisons.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class PreferSpecializedOverGenericAnalyzer : DiagnosticAnalyzer
{
    private const string spaceTupleName = "SpaceTuple";
    private const string spaceTemplateName = "SpaceTemplate";

    public static readonly DiagnosticDescriptor Diagnostic = new(
      id: "OSA004",
      category: Categories.Performance,
      defaultSeverity: DiagnosticSeverity.Warning,
      isEnabledByDefault: true,
      title: "Prefer using specialized over generic type.",
      messageFormat: "Prefer using specialized '{0}' over generic '{1}' type.",
      helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA004.md");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var creationOperation = (IObjectCreationOperation)context.Operation;

        if (creationOperation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTuple)))
        {
            TryReportDiagnosticFor(in context, creationOperation, spaceTupleName);
            return;
        }

        if (creationOperation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTemplate)))
        {
            TryReportDiagnosticFor(in context, creationOperation, spaceTemplateName);
            return;
        }
    }

    private static void TryReportDiagnosticFor(
        in OperationAnalysisContext context, IObjectCreationOperation creationOperation, string genericTypeName)
    {
        var arguments = creationOperation.GetArguments();
        if (arguments.Count() == 0)
        {
            return;
        }

        var argumentTypeSymbols = arguments.Select(arg => creationOperation.SemanticModel?.GetTypeInfo(arg.Expression).Type);
        if (argumentTypeSymbols == null)
        {
            return;
        }


        int sameTypeArgumentsCount = argumentTypeSymbols.Distinct(SymbolEqualityComparer.Default).Count();
        if (sameTypeArgumentsCount > 1)
        {
            return;
        }

        string? argumentTypeName = argumentTypeSymbols.First()?.GetFullName();
        if (argumentTypeName is null)
        {
            return;
        }

        string? specializedTypeName = genericTypeName == spaceTupleName ?
            argumentTypeName switch
            {
                FullyQualifiedNames.System_Boolean => FullyQualifiedNames.BoolTuple,
                FullyQualifiedNames.System_Byte => FullyQualifiedNames.ByteTuple,
                FullyQualifiedNames.System_SByte => FullyQualifiedNames.SByteTuple,
                FullyQualifiedNames.System_Char => FullyQualifiedNames.CharTuple,
                FullyQualifiedNames.System_Double => FullyQualifiedNames.DoubleTuple,
                FullyQualifiedNames.System_Single => FullyQualifiedNames.FloatTuple,
                FullyQualifiedNames.System_Int16 => FullyQualifiedNames.ShortTuple,
                FullyQualifiedNames.System_UInt16 => FullyQualifiedNames.UShortTuple,
                FullyQualifiedNames.System_Int32 => FullyQualifiedNames.IntTuple,
                FullyQualifiedNames.System_UInt32 => FullyQualifiedNames.UIntTuple,
                FullyQualifiedNames.System_Int64 => FullyQualifiedNames.LongTuple,
                FullyQualifiedNames.System_UInt64 => FullyQualifiedNames.ULongTuple,
                FullyQualifiedNames.System_Int128 => FullyQualifiedNames.HugeTuple,
                FullyQualifiedNames.System_UInt128 => FullyQualifiedNames.UHugeTuple,
                FullyQualifiedNames.System_Decimal => FullyQualifiedNames.DecimalTuple,
                FullyQualifiedNames.System_DateTime => FullyQualifiedNames.DateTimeTuple,
                FullyQualifiedNames.System_DateTimeOffset => FullyQualifiedNames.DateTimeOffsetTuple,
                FullyQualifiedNames.System_TimeSpan => FullyQualifiedNames.TimeSpanTuple,
                FullyQualifiedNames.System_Guid => FullyQualifiedNames.GuidTuple,
                _ => null
            } :
            argumentTypeName switch
            {
                FullyQualifiedNames.System_Boolean => FullyQualifiedNames.BoolTemplate,
                FullyQualifiedNames.System_Byte => FullyQualifiedNames.ByteTemplate,
                FullyQualifiedNames.System_SByte => FullyQualifiedNames.SByteTemplate,
                FullyQualifiedNames.System_Char => FullyQualifiedNames.CharTemplate,
                FullyQualifiedNames.System_Double => FullyQualifiedNames.DoubleTemplate,
                FullyQualifiedNames.System_Single => FullyQualifiedNames.FloatTemplate,
                FullyQualifiedNames.System_Int16 => FullyQualifiedNames.ShortTemplate,
                FullyQualifiedNames.System_UInt16 => FullyQualifiedNames.UShortTemplate,
                FullyQualifiedNames.System_Int32 => FullyQualifiedNames.IntTemplate,
                FullyQualifiedNames.System_UInt32 => FullyQualifiedNames.UIntTemplate,
                FullyQualifiedNames.System_Int64 => FullyQualifiedNames.LongTemplate,
                FullyQualifiedNames.System_UInt64 => FullyQualifiedNames.ULongTemplate,
                FullyQualifiedNames.System_Int128 => FullyQualifiedNames.HugeTemplate,
                FullyQualifiedNames.System_UInt128 => FullyQualifiedNames.UHugeTemplate,
                FullyQualifiedNames.System_Decimal => FullyQualifiedNames.DecimalTemplate,
                FullyQualifiedNames.System_DateTime => FullyQualifiedNames.DateTimeTemplate,
                FullyQualifiedNames.System_DateTimeOffset => FullyQualifiedNames.DateTimeOffsetTemplate,
                FullyQualifiedNames.System_TimeSpan => FullyQualifiedNames.TimeSpanTemplate,
                FullyQualifiedNames.System_Guid => FullyQualifiedNames.GuidTemplate,
                _ => null
            };

        specializedTypeName = GetTupleShortName(specializedTypeName.AsSpan());
        if (specializedTypeName is null)
        {
            return;
        }

        var localDeclaration = creationOperation.Syntax.TryGetParentNode<LocalDeclarationStatementSyntax>();
        if (localDeclaration is null)
        {
            return;
        }

        var builder = ImmutableDictionary.CreateBuilder<string, string?>();
        builder.Add("SpecializedTypeName", specializedTypeName);

        context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
            descriptor: Diagnostic,
            location: localDeclaration.GetLocation(),
            messageArgs: new[] { specializedTypeName, genericTypeName },
            properties: builder.ToImmutable()));
    }

    private static string? GetTupleShortName(ReadOnlySpan<char> fullName)
    {
        if (fullName == null)
        {
            return null;
        }

        int lastDotIndex = fullName.LastIndexOf('.');
        if (lastDotIndex >= 0 && lastDotIndex < fullName.Length - 1)
        {
            return fullName.Slice(lastDotIndex + 1).ToString();
        }

        return null;
    }
}