using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA004;

/// <summary>
/// Warns when a method which is supported only on full configuration, is called when partial configuration is used.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class InvalidMethodCallOnCurrentConfigAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA004",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "Method is not supported with the current configuration.",
        messageFormat: "Method '{0}' is not supported with the current configuration.",
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

        if (memberAccessExpression == null || !IsNonBlockingSpaceAgentMethod())
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

        if (typeSymbol?.Name == "ISpaceAgent" && !IsGenericHostBuilt())
        {
            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
                descriptor: Diagnostic,
                location: invocationExpression.GetLocation(),
                messageArgs: new[] { memberAccessExpression.Name.Identifier.ValueText }));
        }

        bool IsNonBlockingSpaceAgentMethod()
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

        bool IsGenericHostBuilt()
        {
            SyntaxNode? parentNode = context.Node.Parent;
            while (parentNode is not null && parentNode is not CompilationUnitSyntax)
            {
                parentNode = parentNode.Parent;
            }

            ITypeSymbol? typeSymbol = null;
            if (parentNode is CompilationUnitSyntax compilationSyntax)
            {
                //_ = compilationSyntax.ChildNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault(x => x.Identifier.ToString() == "Program")?
                //    .DescendantNodes().OfType<IdentifierNameSyntax>().FirstOrDefault(identifierName =>
                //    {
                //        if (identifierName.ToString() == "CreateDefaultBuilder")
                //        {
                //            var b11 = context.SemanticModel.GetDeclaredSymbol(identifierName);
                //            var b112 = context.SemanticModel.GetSymbolInfo(identifierName);
                //            var b222 = context.SemanticModel.GetTypeInfo(identifierName);

                //            if (identifierName.Parent is MemberAccessExpressionSyntax memberAccessExpression)
                //            {
                //                var a = context.SemanticModel.GetDeclaredSymbol(memberAccessExpression.Expression);
                //                var b = context.SemanticModel.GetDeclaredSymbol(memberAccessExpression);

                //                var a1 = context.SemanticModel.GetSymbolInfo(memberAccessExpression.Expression);
                //                var b1 = context.SemanticModel.GetSymbolInfo(memberAccessExpression);

                //                var a2 = context.SemanticModel.GetTypeInfo(memberAccessExpression.Expression);
                //                var b2 = context.SemanticModel.GetTypeInfo(memberAccessExpression);

                //                //var a = context.SemanticModel.GetSymbolInfo(memberAccessExpression.Expression, context.CancellationToken).Symbol;

                //                if (a is IMethodSymbol methodSymbol)
                //                {
                //                    typeSymbol = methodSymbol.ReturnType;
                //                }
                //            }
                //        }

                //        return typeSymbol != null;
                //    });

                _ = compilationSyntax.ChildNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault(x => x.Identifier.ToString() == "Program")?
                    .DescendantNodes().OfType<MemberAccessExpressionSyntax>().FirstOrDefault(memberAccessExpression =>
                    {
                        if (memberAccessExpression.Name.Identifier.ToString() == "CreateDefaultBuilder")
                        {
                            var d = context.SemanticModel.GetSymbolInfo(memberAccessExpression);
                            var a = context.SemanticModel.GetSymbolInfo(memberAccessExpression).Symbol;
                            var b = context.SemanticModel.GetSymbolInfo(memberAccessExpression.Expression).Symbol;

                            if (a is IMethodSymbol methodSymbol)
                            {
                                typeSymbol = methodSymbol.ReturnType;
                            }
                        }

                        return typeSymbol != null;
                    });
            }

            return typeSymbol?.Name == "IHostBuilder" || typeSymbol?.Name == "IWebHostBuilder";
        }
    }
}