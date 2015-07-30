using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using RoslynTypeScript.Constants;

namespace RoslynTypeScript.Translation
{
    public class YieldStatementTranslation : StatementTranslation
    {
        public new YieldStatementSyntax Syntax
        {
            get { return (YieldStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public YieldStatementTranslation() { }
        public YieldStatementTranslation(YieldStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
        }

        public override void ApplyPatch()
        {
            if (Syntax.IsKind(SyntaxKind.YieldReturnStatement))
            {
                var method = this.GetAncestor<MethodDeclarationTranslation>();
                if (method != null)
                {
                    method.IsYieldReturn = true;
                }
            }

            base.ApplyPatch();
        }

        public ExpressionTranslation Expression { get; set; }

        protected override string InnerTranslate()
        {
            if (Syntax.IsKind(SyntaxKind.YieldReturnStatement))
            {
                var expr = Expression.Translate();

                return $@"{TC.YieldResultName}.push({expr});
                    /*{Syntax.ToString()}*/";
            }

            return Syntax.ToString();
        }
    }
}
