using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA004;

/// <summary>
/// Warns when a method which is supported only in full configuration, is called when partial configuration is used.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class InvalidMethodCallOnCurrentConfigAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA004",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "Method is not supported with the current configuration. An instance of 'Microsoft.Extensions.Hosting.IHost' must be configured to run.",
        messageFormat: "Method '{0}' is not supported with the current configuration. An instance of 'Microsoft.Extensions.Hosting.IHost' must be configured to run.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA004.md");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyseInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyseInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocationExpression = (InvocationExpressionSyntax)context.Node;
        var memberAccessExpression = invocationExpression.Expression as MemberAccessExpressionSyntax;

        if (memberAccessExpression == null || !IsNonBlockingSpaceAgentMethod(invocationExpression, memberAccessExpression))
        {
            return;
        }

        ITypeSymbol? typeSymbol = null;

        // The target is a simple identifier, the code being analysed is of the form: "agent.PeekAsync(...)", and identifierName = "agent".
        if (memberAccessExpression.Expression is IdentifierNameSyntax identifierName)
        {
            typeSymbol = context.SemanticModel.GetTypeInfo(identifierName, context.CancellationToken).Type;
        }

        // The target is another invocation syntax, the code being analysed is of the form: "GetAgent().PeekAsync(...)", and _invocationExpression = "GetAgent()". 
        if (memberAccessExpression.Expression is InvocationExpressionSyntax _invocationExpression)
        {
            var symbol = context.SemanticModel.GetSymbolInfo(_invocationExpression, context.CancellationToken).Symbol;

            if (symbol is IMethodSymbol methodSymbol) typeSymbol = methodSymbol.ReturnType;
            if (symbol is IFunctionPointerTypeSymbol funcSymbol) typeSymbol = funcSymbol.Signature.ReturnType;
        }

        // The target is a member access syntaxt, the code being analysed is of the form: "x.Agent.PeekAsync(...)", _memberAccessExpression = "x.Agent"
        if (memberAccessExpression.Expression is MemberAccessExpressionSyntax _memberAccessExpression)
        {
            var symbol = context.SemanticModel.GetSymbolInfo(_memberAccessExpression, context.CancellationToken).Symbol;

            if (symbol is IFieldSymbol fieldSymbol) typeSymbol = fieldSymbol.Type;
            if (symbol is IPropertySymbol propertySymbol) typeSymbol = propertySymbol.Type;
        }

        if (typeSymbol?.Name == "ISpaceAgent" && !IsHostPresent(ref context))
        {
            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
                descriptor: Diagnostic,
                location: invocationExpression.GetLocation(),
                messageArgs: new[] { memberAccessExpression.Name.Identifier.ValueText }));
        }

        
    }

    private static bool IsNonBlockingSpaceAgentMethod(InvocationExpressionSyntax invocationExpression, MemberAccessExpressionSyntax memberAccessExpression)
    {
        string methodName = memberAccessExpression.Name.Identifier.ValueText;

        if (methodName == "EvaluateAsync")
        {
            return true;
        }

        if (methodName == "PeekAsync" || methodName == "PopAsync")
        {
            // Arguments.Count = 2, because only the overloads with a callback delegate can not work without full configuration in-place.
            if (invocationExpression.ChildNodes().OfType<ArgumentListSyntax>().Single().Arguments.Count == 2)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsHostPresent(ref SyntaxNodeAnalysisContext context)
    {
        SyntaxNode? parentNode = context.Node.Parent;

        while (parentNode is not null && parentNode is not CompilationUnitSyntax)
        {
            parentNode = parentNode.Parent;
        }

        if (parentNode is CompilationUnitSyntax compilationSyntax)
        {
            foreach (var memberAccessExpression in compilationSyntax.DescendantNodes().OfType<MemberAccessExpressionSyntax>())
            {
                var identifierNames = memberAccessExpression.ChildNodes().OfType<IdentifierNameSyntax>().ToArray();
                if (identifierNames.Length == 2)
                {
                    if (IsConsoleApp(identifierNames) || IsWebApp(identifierNames))
                    {
                        if (memberAccessExpression.Parent is InvocationExpressionSyntax invocationExpression)
                        {
                            var argumentSyntax = invocationExpression.ArgumentList.Arguments.FirstOrDefault();
                            if (argumentSyntax != null)
                            {
                                var typeSymbol = context.SemanticModel.GetTypeInfo(argumentSyntax.Expression, context.CancellationToken).Type;
                                if (typeSymbol is IArrayTypeSymbol arrayTypeSymbol && arrayTypeSymbol.ElementType.SpecialType == SpecialType.System_String)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;

        static bool IsConsoleApp(IdentifierNameSyntax[] identifierNames) =>
            identifierNames[0].Identifier.ValueText == "Host" && identifierNames[1].Identifier.ValueText == "CreateDefaultBuilder";

        static bool IsWebApp(IdentifierNameSyntax[] identifierNames) =>
            (identifierNames[0].Identifier.ValueText == "WebHost" && identifierNames[1].Identifier.ValueText == "CreateDefaultBuilder") ||
            (identifierNames[0].Identifier.ValueText == "WebApplication" && identifierNames[1].Identifier.ValueText == "CreateBuilder");
    }
}