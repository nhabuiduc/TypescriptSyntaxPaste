using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace TypescriptSyntaxPaste
{

    class MemberToCamelCaseRewriter:CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax propertySyntax)
        {
            var leadingTrivia = propertySyntax.Identifier.LeadingTrivia;
            var trailingTriva = propertySyntax.Identifier.TrailingTrivia;
            return propertySyntax.ReplaceToken(propertySyntax.Identifier,
                SyntaxFactory.Identifier(leadingTrivia,
                ToCamelCase(propertySyntax.Identifier.ValueText), trailingTriva) );
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax methodSyntax)
        {
            var leadingTrivia = methodSyntax.Identifier.LeadingTrivia;
            var trailingTriva = methodSyntax.Identifier.TrailingTrivia;
            return methodSyntax.ReplaceToken(methodSyntax.Identifier,
                SyntaxFactory.Identifier(leadingTrivia, ToCamelCase(methodSyntax.Identifier.ValueText), trailingTriva));
        }

        public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax fieldSyntax)
        {
            return fieldSyntax.ReplaceTokens(fieldSyntax.Declaration.Variables.Select(f => f.Identifier),
                (t1, t2) => SyntaxFactory.Identifier(t1.LeadingTrivia, ToCamelCase(t1.ValueText), t1.TrailingTrivia));
            
        }

        private static string ToCamelCase(string name)
        {
            return name.Substring(0, 1).ToLower() + name.Substring(1);
        }
    }

    public class MakeMemberCamelCase
    {
        public static CSharpSyntaxNode Make(CSharpSyntaxNode syntaxNode)
        {

            var rewriter = new MemberToCamelCaseRewriter();
            return (CSharpSyntaxNode)rewriter.Visit(syntaxNode);
        }

        
    }
}
