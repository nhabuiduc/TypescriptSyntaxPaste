using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypescriptSyntaxPaste
{
    public class OptionalInterfaceProperties
    {
        public static CSharpSyntaxNode AddOptional(CSharpSyntaxNode syntaxNode)
        {
            var interfaces = syntaxNode.DescendantNodesAndSelf().Where(f => f is InterfaceDeclarationSyntax);

            var properties = interfaces.SelectMany(f => f.DescendantNodes().Where(c => c is PropertyDeclarationSyntax));
            var methods = interfaces.SelectMany(f => f.DescendantNodes().Where(c => c is MethodDeclarationSyntax));

            return syntaxNode.ReplaceNodes(properties.Concat(methods), (node, node2) =>
            {
                var property = node as PropertyDeclarationSyntax;
                var method = node as MethodDeclarationSyntax;
                if (property != null)
                {
                    return property.WithIdentifier(SyntaxFactory.Identifier(property.Identifier.ValueText + "?"));
                }

                return method.WithIdentifier(SyntaxFactory.Identifier(method.Identifier.ValueText + "?"));
            });          

        }
    }
}
