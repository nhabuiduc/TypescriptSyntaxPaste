using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TypescriptSyntaxPaste.VSIX;

namespace TypescriptSyntaxPaste
{

    public class TypeNameReplacement
    {
        public static CSharpSyntaxNode Replace(TypeNameReplacementData[] replacedTypeNameArray, CSharpSyntaxNode syntaxNode)
        {
            var typeNodes = syntaxNode.DescendantNodes()
                .OfType<TypeSyntax>()
                .Where(f => replacedTypeNameArray.Any(r => r.OldTypeName == f.ToString()));
            

            return syntaxNode.ReplaceNodes(typeNodes, (n1, n2) => {
                var name = n1.ToString();
                var newName = replacedTypeNameArray.First(f => f.OldTypeName == name).NewTypeName;
                var newType = SyntaxFactory.ParseTypeName(newName);

                return newType;
            });

        }

    }
}
