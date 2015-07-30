using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ParenthesizedExpressionTranslation : ExpressionTranslation
    {
        public new ParenthesizedExpressionSyntax Syntax
        {
            get { return (ParenthesizedExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public ParenthesizedExpressionTranslation() { }
        public ParenthesizedExpressionTranslation(ParenthesizedExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
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
