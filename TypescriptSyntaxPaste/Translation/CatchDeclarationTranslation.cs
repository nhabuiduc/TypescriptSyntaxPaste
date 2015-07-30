using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class CatchDeclarationTranslation : CSharpSyntaxTranslation
    {
        public new CatchDeclarationSyntax Syntax
        {
            get { return (CatchDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public CatchDeclarationTranslation() { }
        public CatchDeclarationTranslation(CatchDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }


        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
