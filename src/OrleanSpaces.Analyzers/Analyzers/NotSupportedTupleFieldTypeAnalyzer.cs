using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Xml.Linq;

namespace OrleanSpaces.Analyzers.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class NotSupportedTupleFieldTypeAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA002",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "The supplied argument is not of a supported type.",
        messageFormat: "The supplied argument '{0}' is not a supported type.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var operation = (IObjectCreationOperation)context.Operation;

        if (IsOfType(operation.Type, context.Compilation.GetTypeByMetadataName(Constants.SpaceTupleFullName)))
        {
            var argumentOperation = operation.Arguments.SingleOrDefault();
            if (argumentOperation != null)
            {
                var arguments = argumentOperation.Syntax.DescendantNodes().OfType<ArgumentSyntax>();
                foreach (var argument in arguments)
                {
                    var type = operation.SemanticModel?.GetTypeInfo(argument, context.CancellationToken).Type;

                    var symbol = operation.SemanticModel?.GetSymbolInfo(argument, context.CancellationToken).Symbol;

                    //if (!IsSimpleExpression(argument.Expression))
                    //{
                    //    context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
                    //        descriptor: Diagnostic,
                    //        location: argument.GetLocation(),
                    //        messageArgs: argument.ToString()));
                    //}
                }
            }
        }
    }

    private static bool IsOfType(ITypeSymbol? currentSymbol, ITypeSymbol? targetSymbol) =>
        SymbolEqualityComparer.Default.Equals(currentSymbol, targetSymbol);

    private static bool IsSimpleType(ITypeSymbol? type)
    {
        //if (type == null)
        //{
        //    return false;
        //}


        return type?.SpecialType switch
        {
            // Primitives
            SpecialType.System_Boolean
            or SpecialType.System_Byte
            or SpecialType.System_SByte
            or SpecialType.System_Char
            or SpecialType.System_Double
            or SpecialType.System_Single
            or SpecialType.System_Int16 
            or SpecialType.System_UInt16
            or SpecialType.System_Int32 
            or SpecialType.System_UInt32
            or SpecialType.System_Int64
            or SpecialType.System_UInt64 
            or SpecialType.System_Enum
            or SpecialType.System_String
            or SpecialType.System_DateTime
            // TODO: Find DateTimeOffset, TimeSpan, Guid
                => true,
            _ => false,
        };
    }

    private static bool IsSimpleExpression<T>(T? syntax) where T : ExpressionSyntax
    {
        if (syntax is LiteralExpressionSyntax)
        {
            return true;
        }

        if (syntax is IdentifierNameSyntax identifierNameSyntax)
        {
            var a = identifierNameSyntax.SyntaxTree;
            //TODO: Find (somehow) the VariableDeclarator and see if ...
            if (syntax is PredefinedTypeSyntax predefinedTypeSyntax)
            {
                predefinedTypeSyntax.Keyword.IsKind(SyntaxKind.IntKeyword);
                return false;
            }
        }

        return false;
    }
}