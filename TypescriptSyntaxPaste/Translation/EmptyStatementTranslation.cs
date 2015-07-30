using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class EmptyStatementTranslation : StatementTranslation
    {
        public new EmptyStatementSyntax Syntax
        {
            get { return (EmptyStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public EmptyStatementTranslation() { }
        public EmptyStatementTranslation(EmptyStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
