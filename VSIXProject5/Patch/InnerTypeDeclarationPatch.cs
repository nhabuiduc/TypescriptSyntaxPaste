using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Patch
{
    /// <summary>
    /// put the nested class into the correct structure of Typescript, which we must create 
    /// another module
    /// </summary>
    public class InnerTypeDeclarationPatch : Patch
    {
        public void Apply(BaseTypeDeclarationTranslation typeDeclarationTranslation)
        {
            // TODO: only support one level, why do you need nested > 1 level ?
            TypeDeclarationTranslation outerMemberDeclaration = 
                (TypeDeclarationTranslation)typeDeclarationTranslation.TravelUpNotMe(f => f is TypeDeclarationTranslation);
            if(outerMemberDeclaration==null)
            {
                return;
            }

            SyntaxListBaseTranslation syntaxListBaseTranslation = (SyntaxListBaseTranslation)typeDeclarationTranslation.Parent;

            syntaxListBaseTranslation.Remove(typeDeclarationTranslation);

            SyntaxListBaseTranslation outerSyntaxListBaseTranslation = (SyntaxListBaseTranslation)outerMemberDeclaration.Parent;
            var newNamespace = CreateNewNamespace(outerMemberDeclaration.Syntax.Identifier.ToString(), typeDeclarationTranslation);
            outerSyntaxListBaseTranslation.Add(newNamespace);
            
        }

        private NamespaceDeclarationTranslation CreateNewNamespace(string identifier, BaseTypeDeclarationTranslation typeDeclarationTranslation)
        {
            NamespaceDeclarationTranslation newNamespaceTranslation = new NamespaceDeclarationTranslation();
            newNamespaceTranslation.Name = new IdentifierNameTranslation() { SyntaxString = identifier, Parent = newNamespaceTranslation };
            newNamespaceTranslation.Members = new SyntaxListTranslation<MemberDeclarationSyntax, MemberDeclarationTranslation>() { Parent = newNamespaceTranslation };
            newNamespaceTranslation.Members.Add(typeDeclarationTranslation);
            newNamespaceTranslation.IsExport = true;

            return newNamespaceTranslation;
        }
    }
}
