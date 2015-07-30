using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class PostfixUnaryExpressionTranslation : ExpressionTranslation
    {
        public new PostfixUnaryExpressionSyntax Syntax
        {
            get { return (PostfixUnaryExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public PostfixUnaryExpressionTranslation() { }
        public PostfixUnaryExpressionTranslation(PostfixUnaryExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Operand = syntax.Operand.Get<ExpressionTranslation>(this);
        }

        public ExpressionTranslation Operand { get; set; }

        protected override string InnerTranslate()
        {
            return $"{Operand.Translate()}{Syntax.OperatorToken.ToString()}";
        }
    }
}
