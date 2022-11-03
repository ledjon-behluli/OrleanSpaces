using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace OrleanSpaces.Analyzers;

internal static class TypeSymbolExtensions
{
    /// <summary>
    /// Determines if <paramref name="symbol"/> is of type <paramref name="candidateSymbol"/>.
    /// </summary>
    /// <param name="symbol">Symbol type being compared.</param>
    /// <param name="candidateSymbol">Symbol type to compare against.</param>
    public static bool IsOfType(this ITypeSymbol? symbol, ITypeSymbol? candidateSymbol) =>
        symbol.IsOfAnyType(new List<ITypeSymbol?>() { candidateSymbol });

    /// <summary>
    /// Determines if <paramref name="symbol"/> is any of the types in <paramref name="candidateSymbols"/>.
    /// </summary>
    /// <param name="symbol">Symbol type being compared.</param>
    /// <param name="candidateSymbols">Collection of symbol types to compare against.</param>
    public static bool IsOfAnyType(this ITypeSymbol? symbol, IEnumerable<ITypeSymbol?> candidateSymbols)
    {
        if (symbol == null || candidateSymbols == null || candidateSymbols.Count() == 0)
        {
            return false;
        }

        return candidateSymbols.Any(candidate => candidate != null && symbol.Equals(candidate, SymbolEqualityComparer.Default));
    }

    /// <summary>
    /// Determines if <paramref name="symbol"/> is of CLR type <paramref name="candidateType"/>.
    /// </summary>
    /// <param name="symbol">Symbol type being compared.</param>
    /// <param name="candidateType">CLR types to compare against.</param>
    /// <param name="compilation">Compilation unit.</param>
    public static bool IsOfClrType(this ITypeSymbol? symbol, Type candidateType, Compilation compilation) =>
        symbol.IsOfAnyClrType(new List<Type>() { candidateType }, compilation);

    /// <summary>
    /// Determines if <paramref name="symbol"/> is any of the CLR types in <paramref name="candidateTypes"/>.
    /// </summary>
    /// <param name="symbol">Symbol type being compared.</param>
    /// <param name="candidateTypes">Collection of CLR types to compare against.</param>
    /// <param name="compilation">Compilation unit.</param>
    public static bool IsOfAnyClrType(this ITypeSymbol? symbol, IEnumerable<Type> candidateTypes, Compilation compilation)
    {
        if (symbol == null)
        {
            return false;
        }

        return candidateTypes.Any(type =>
        {
            var clrTypeSymbol = compilation.GetTypeByMetadataName(type.FullName);
            if (type == typeof(Enum))
            {
                var baseType = symbol.OriginalDefinition.BaseType;
                if (baseType != null)
                {
                    return baseType.Equals(clrTypeSymbol, SymbolEqualityComparer.Default);
                }
            }

            return symbol.OriginalDefinition.Equals(clrTypeSymbol, SymbolEqualityComparer.Default);
        });
    }
}

internal static class OperationExtensions
{
    /// <summary>
    /// Returns all <see cref="ArgumentSyntax"/> found in <paramref name="operation"/>
    /// </summary>
    public static IEnumerable<ArgumentSyntax> GetArguments(this IObjectCreationOperation operation)
    {
        var argumentOperation = operation.Arguments.SingleOrDefault();
        if (argumentOperation != null)
        {
            var arguments = argumentOperation.Syntax.DescendantNodes().OfType<ArgumentSyntax>();
            foreach (var argument in arguments)
            {
                yield return argument;
            }
        }
    }
}

internal static class SyntaxExtensions
{
    public static (SyntaxNode?, string) GetNamespaceParts(this SyntaxNode? node)
    {
        string @namespace = string.Empty;
        SyntaxNode? namespaceNode = null;
      
        if (node is CompilationUnitSyntax compilation)
        {
            var potentialNamespace = node.ChildNodes().FirstOrDefault(x => 
                x is NamespaceDeclarationSyntax || 
                x is FileScopedNamespaceDeclarationSyntax ||
                x is GlobalStatementSyntax);

            if (potentialNamespace != null)
            {
                TrySetResult(potentialNamespace, ref @namespace, ref namespaceNode);
            }
        }
        else
        {
            var potentialNamespace = node?.Parent;

            while (potentialNamespace is not null &&
                   potentialNamespace is not NamespaceDeclarationSyntax &&
                   potentialNamespace is not FileScopedNamespaceDeclarationSyntax)
            {
                potentialNamespace = potentialNamespace.Parent;
            }

            TrySetResult(potentialNamespace, ref @namespace, ref namespaceNode);
        }

        return (namespaceNode, @namespace);
    }

    private static void TrySetResult(SyntaxNode? potentialNamespace, ref string @namespace, ref SyntaxNode? namespaceNode)
    {
        if (potentialNamespace is BaseNamespaceDeclarationSyntax namespaceParent)
        {
            @namespace = namespaceParent.Name.ToString();
            namespaceNode = namespaceParent;

            while (true)
            {
                if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                {
                    break;
                }

                @namespace = $"{namespaceParent.Name}.{@namespace}";  // Add the outer namespace as a prefix to the final namespace

                namespaceParent = parent;
                namespaceNode = parent;
            }

            return;
        }
       
        if (potentialNamespace is GlobalStatementSyntax globalStatement)
        {
            @namespace = string.Empty;
            namespaceNode = globalStatement;
        }
    }
}
