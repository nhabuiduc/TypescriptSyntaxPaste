using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class CheckedExpressionTranslation : ExpressionTranslation
    {
        public new CheckedExpressionSyntax Syntax
        {
            get { return (CheckedExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public CheckedExpressionTranslation() { }
        public CheckedExpressionTranslation(CheckedExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
        }

        public ExpressionTranslation Expression { get; set; }

        protected override string InnerTranslate()
        {
            return $"({Expression.Translate()})";
        }
    }
}
