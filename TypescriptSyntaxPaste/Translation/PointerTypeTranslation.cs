using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class PointerTypeTranslation : TypeTranslation
    {
        public new PointerTypeSyntax Syntax
        {
            get { return (PointerTypeSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public PointerTypeTranslation() { }
        public PointerTypeTranslation(PointerTypeSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
