using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ReturnStatementTranslation : StatementTranslation
    {
        public new ReturnStatementSyntax Syntax
        {
            get { return (ReturnStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public ExpressionTranslation Expression { get; set; }
        public ReturnStatementTranslation() { }
        public ReturnStatementTranslation(ReturnStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
        }

        public override void ApplyPatch()
        {
            base.ApplyPatch();
            Helper.ApplyFunctionBindToCorrectContext(this.Expression);
        }

        protected override string InnerTranslate()
        {
            if (Expression == null)
            {
                return "return";
            }

            var expr = Expression.Translate();

            return $"return {expr};";
        }
    }
}
