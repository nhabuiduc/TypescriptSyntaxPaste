using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class PrefixUnaryExpressionTranslation : ExpressionTranslation
    {
        public new PrefixUnaryExpressionSyntax Syntax
        {
            get { return (PrefixUnaryExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public PrefixUnaryExpressionTranslation() { }
        public PrefixUnaryExpressionTranslation(PrefixUnaryExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Operand = syntax.Operand.Get<ExpressionTranslation>(this);
        }

        public ExpressionTranslation Operand { get; set; }

        protected override string InnerTranslate()
        {
            return $"{Syntax.OperatorToken.ToString()}{Operand.Translate()}";
        }
    }
}
