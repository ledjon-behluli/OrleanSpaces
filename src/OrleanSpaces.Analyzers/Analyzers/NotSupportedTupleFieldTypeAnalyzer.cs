using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.Analyzers;

//[DiagnosticAnalyzer(LanguageNames.CSharp)]
//internal sealed class NotSupportedTupleFieldTypeAnalyzer : DiagnosticAnalyzer
//{
//    public static readonly DiagnosticDescriptor Diagnostic = new(
//        id: "OSA002",
//        category: Categories.Usage,
//        defaultSeverity: DiagnosticSeverity.Warning,
//        isEnabledByDefault: true,
//        title: "The supplied constructor argument is not of a supported type.",
//        messageFormat: "The supplied constructor argument '{0}' is not of a supported type..");

//    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

//    public override void Initialize(AnalysisContext context)
//    {
//        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
//        context.EnableConcurrentExecution();

//        context.RegisterOperationAction(AnalyzeArrayCreation, OperationKind.ArrayCreation);
//        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
//    }

//    private void AnalyzeObjectCreation(OperationAnalysisContext context)
//    {
//        var operation = (IObjectCreationOperation)context.Operation;
//        ReportDiagnostic(operation.Syntax, operation.Type, context.ReportDiagnostic);
//    }

//    private void AnalyzeArrayCreation(OperationAnalysisContext context)
//    {
//        var operation = (IArrayCreationOperation)context.Operation;
//        ReportDiagnostic(operation.Syntax, operation.Type, context.ReportDiagnostic);
//    }

//    private static void ReportDiagnostic(SyntaxNode node, ITypeSymbol? type, Action<Diagnostic> action)
//    {
//        action(Microsoft.CodeAnalysis.Diagnostic.Create(
//                descriptor: Diagnostic,
//                location: node.GetLocation(),
//                messageArgs: type?.Name));
//    }
//}