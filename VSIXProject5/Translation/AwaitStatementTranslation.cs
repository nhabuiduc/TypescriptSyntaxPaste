using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class AwaitExpressionTranslation : ExpressionTranslation
    {
        public new AwaitExpressionSyntax Syntax
        {
            get { return (AwaitExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        
        public AwaitExpressionTranslation() { }
        public AwaitExpressionTranslation(AwaitExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
        }

        public ExpressionTranslation Expression { get; set; }

        protected override string InnerTranslate()
        {
            return $"await {Expression.Translate()}";
        }
    }
}
