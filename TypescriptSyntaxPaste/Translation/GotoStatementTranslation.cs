using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynTypeScript.Translation
{
    public class GotoStatementTranslation : StatementTranslation
    {
        public new GotoStatementSyntax Syntax
        {
            get { return (GotoStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public GotoStatementTranslation() { }
        public GotoStatementTranslation(GotoStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            
        }

        public ExpressionTranslation Expression { get; set; }

        protected override string InnerTranslate()
        {
 
            return Syntax.ToString();
        }
    }
}
