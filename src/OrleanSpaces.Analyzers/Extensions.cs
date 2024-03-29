﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using System.Text;

namespace OrleanSpaces.Analyzers;

internal static class Extensions
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
    public static bool IsOfAnyType(this ITypeSymbol? symbol, List<ITypeSymbol?> candidateSymbols)
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
    public static bool IsOfAnyClrType(this ITypeSymbol? symbol, List<Type> candidateTypes, Compilation compilation)
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

    public static string GetFullName(this ISymbol symbol)
    {
        if (symbol == null || IsRootNamespace(symbol))
        {
            return string.Empty;
        }

        StringBuilder builder = new(symbol.MetadataName);
        ISymbol lastSymbol = symbol;

        symbol = symbol.ContainingSymbol;

        while (!IsRootNamespace(symbol))
        {
            if (symbol is ITypeSymbol && lastSymbol is ITypeSymbol)
            {
                builder.Insert(0, '+');
            }
            else
            {
                builder.Insert(0, '.');
            }

            builder.Insert(0, symbol.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
            symbol = symbol.ContainingSymbol;
        }

        return builder.ToString();

        static bool IsRootNamespace(ISymbol symbol)
        {
            INamespaceSymbol? namespaceSymbol;
            return ((namespaceSymbol = symbol as INamespaceSymbol) != null) && namespaceSymbol.IsGlobalNamespace;
        }
    }

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

    public static T? TryGetParentNode<T>(this SyntaxNode? node) where T : class
    {
        if (node is GlobalStatementSyntax globalStatement)
        {
            var targetNode = node.ChildNodes().FirstOrDefault(x => x is T);
            return targetNode is null ? null : targetNode as T;
        }
        else
        {
            while (node is not null && node is not T)
            {
                node = node.Parent;
            }

            return node is null ? null : node as T;
        }
    }

    public static T? TryGetChildNode<T>(this SyntaxNode? node) where T : class
    {
        var targetNode = node?.ChildNodes().FirstOrDefault(x => x is T);
        return targetNode is null ? null : targetNode as T;
    }

    public static SyntaxNode? TryGetNamespaceNode(this SyntaxNode? node)
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
                TrySetNode(potentialNamespace, ref namespaceNode);
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

            TrySetNode(potentialNamespace, ref namespaceNode);
        }

        return namespaceNode;

        static void TrySetNode(SyntaxNode? potentialNamespaceNode, ref SyntaxNode? namespaceNode)
        {
            if (potentialNamespaceNode is GlobalStatementSyntax globalStatement)
            {
                namespaceNode = globalStatement;
                return;
            }

            if (potentialNamespaceNode is BaseNamespaceDeclarationSyntax namespaceParent)
            {
                namespaceNode = namespaceParent;

                while (true)
                {
                    if (namespaceParent.Parent is not NamespaceDeclarationSyntax parent)
                    {
                        break;
                    }

                    namespaceParent = parent;
                    namespaceNode = parent;
                }
            }
        }
    }
}
