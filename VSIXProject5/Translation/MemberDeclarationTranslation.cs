using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public abstract class MemberDeclarationTranslation : CSharpSyntaxTranslation
    {
        public MemberDeclarationTranslation() { }
        public MemberDeclarationTranslation(MemberDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            
        }
    }
}
