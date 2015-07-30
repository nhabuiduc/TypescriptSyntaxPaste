using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class EnumMemberDeclarationTranslation : MemberDeclarationTranslation
    {
        public new EnumMemberDeclarationSyntax Syntax
        {
            get { return (EnumMemberDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public EnumMemberDeclarationTranslation() { }
        public EnumMemberDeclarationTranslation(EnumMemberDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }


        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
