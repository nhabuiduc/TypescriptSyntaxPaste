using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class BaseExpressionTranslation : ExpressionTranslation
    {
        public new BaseExpressionSyntax Syntax
        {
            get { return (BaseExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public BaseExpressionTranslation() { }
        public BaseExpressionTranslation(BaseExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }

        protected override string InnerTranslate()
        {
            return "super";
        }
    }
}
