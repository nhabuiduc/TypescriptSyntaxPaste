using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class InterpolatedStringExpressionTranslation : ExpressionTranslation
    {
        public new InterpolatedStringExpressionSyntax Syntax
        {
            get { return (InterpolatedStringExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public InterpolatedStringExpressionTranslation() { }
        public InterpolatedStringExpressionTranslation(InterpolatedStringExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
