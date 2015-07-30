using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class SimpleBaseTypeTranslation : BaseTypeTranslation
    {
        public new SimpleBaseTypeSyntax Syntax
        {
            get { return (SimpleBaseTypeSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public SimpleBaseTypeTranslation() { }
        public SimpleBaseTypeTranslation(SimpleBaseTypeSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            this.Syntax = syntax;
        }
    }
}
