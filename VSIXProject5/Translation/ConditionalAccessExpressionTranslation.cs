using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ConditionalAccessExpressionTranslation : ExpressionTranslation
    {
        public new ConditionalAccessExpressionSyntax Syntax
        {
            get { return (ConditionalAccessExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public ConditionalAccessExpressionTranslation() { }
        public ConditionalAccessExpressionTranslation(ConditionalAccessExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
            WhenNotNull = syntax.WhenNotNull.Get<ExpressionTranslation>(this);
                 
        }

        public ExpressionTranslation Expression { get; set; }
        public ExpressionTranslation WhenNotNull { get; set; }

        protected override string InnerTranslate()
        {
            return $"{Expression.Translate()}?{WhenNotNull.Translate()}";
        }
    }
}
