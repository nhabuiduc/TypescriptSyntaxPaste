using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypescriptSyntaxPaste
{
    public class ClassToInterfaceReplacement
    {
        public static CSharpSyntaxNode ReplaceClass(CSharpSyntaxNode syntaxNode)
        {
            IEnumerable<TypeDeclarationSyntax> allClassSyntaxes = syntaxNode.DescendantNodesAndSelf().OfType<ClassDeclarationSyntax>();
            IEnumerable<TypeDeclarationSyntax> allStructSyntaxes = syntaxNode.DescendantNodesAndSelf().OfType<StructDeclarationSyntax>();

            var newSyntaxNode = syntaxNode;
            TypeDeclarationSyntax currentSyntax;
            while((currentSyntax = FindTypeDeclrationToReplace(newSyntaxNode))!= null)
            {
                newSyntaxNode = newSyntaxNode.ReplaceNode(currentSyntax, MakeInterface(currentSyntax));
            }

            return newSyntaxNode;
        }

        private static TypeDeclarationSyntax FindTypeDeclrationToReplace(CSharpSyntaxNode syntaxNode)
        {
            TypeDeclarationSyntax typeSyntax = syntaxNode.DescendantNodesAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (typeSyntax != null) return typeSyntax;

            typeSyntax = syntaxNode.DescendantNodesAndSelf().OfType<StructDeclarationSyntax>().FirstOrDefault();
            if (typeSyntax != null) return typeSyntax;

            return null;
        }

        private static InterfaceDeclarationSyntax MakeInterface(TypeDeclarationSyntax typeSyntax)
        {
            return SyntaxFactory.InterfaceDeclaration(
                attributeLists: typeSyntax.AttributeLists,
                modifiers: typeSyntax.Modifiers,
                identifier: typeSyntax.Identifier,
                typeParameterList: typeSyntax.TypeParameterList,
                baseList: typeSyntax.BaseList,
                constraintClauses: typeSyntax.ConstraintClauses,
                members: MakeInterfaceSyntaxList(typeSyntax.Members)).NormalizeWhitespace();
        }

        private static SyntaxList<MemberDeclarationSyntax> MakeInterfaceSyntaxList(IEnumerable<MemberDeclarationSyntax> members)
        {
            var newMembers = ExtractInterfaceMembers(members).ToArray();
            var syntaxList = new SyntaxList<MemberDeclarationSyntax>();
            syntaxList = syntaxList.AddRange(newMembers);
            return syntaxList;
        }

        private static IEnumerable<MemberDeclarationSyntax> ExtractInterfaceMembers(IEnumerable<MemberDeclarationSyntax> members)
        {
            foreach (var member in members)
            {
                if (member is MethodDeclarationSyntax)
                {
                    yield return MakeInterfaceMethod((MethodDeclarationSyntax)member);
                }

                if (member is PropertyDeclarationSyntax)
                {
                    yield return MakeInterfaceProperty((PropertyDeclarationSyntax)member);
                }
            }
        }

        private static MethodDeclarationSyntax MakeInterfaceMethod(MethodDeclarationSyntax methodSyntax)
        {
            return methodSyntax.WithBody(null)
                .WithModifiers(new SyntaxTokenList())
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }

        private static PropertyDeclarationSyntax MakeInterfaceProperty(PropertyDeclarationSyntax propertySyntax)
        {
            var accessors = propertySyntax.AccessorList.Accessors.Select(f => MakeInterfaceAccessor(f));
            var syntaxList = new SyntaxList<AccessorDeclarationSyntax>();
            syntaxList = syntaxList.AddRange(accessors);

            var accessorList = propertySyntax.AccessorList.WithAccessors(syntaxList);

            return propertySyntax.WithModifiers(new SyntaxTokenList()).WithAccessorList(accessorList);
        }

        private static AccessorDeclarationSyntax MakeInterfaceAccessor(AccessorDeclarationSyntax accessorSyntax)
        {
            return accessorSyntax.WithModifiers(new SyntaxTokenList())
                .WithBody(null)
                .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }

    }
}
