using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class LiteralExpressionTranslation : ExpressionTranslation
    {
        public new LiteralExpressionSyntax Syntax
        {
            get { return (LiteralExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public LiteralExpressionTranslation(LiteralExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
