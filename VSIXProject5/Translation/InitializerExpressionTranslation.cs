using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynTypeScript.Translation
{
    public class InitializerExpressionTranslation : ExpressionTranslation
    {
        public new InitializerExpressionSyntax Syntax
        {
            get { return (InitializerExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public InitializerExpressionTranslation() { }
        public InitializerExpressionTranslation(InitializerExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expressions = syntax.Expressions.Get<ExpressionSyntax, ExpressionTranslation>(this);
        }

        public SeparatedSyntaxListTranslation<ExpressionSyntax,ExpressionTranslation> Expressions { get; set; }
        

        public override void ApplyPatch()
        {
            base.ApplyPatch();
            if (Syntax.IsKind(SyntaxKind.ArrayInitializerExpression))
            {
                return;
            }

            foreach (var item in Expressions.GetEnumerable())
            {
                var exp = item as AssignmentExpressionTranslation;
                if(exp == null)
                {
                    continue;
                }

                var identifierName = exp.Left as IdentifierNameTranslation;
                if(identifierName == null)
                {
                    continue;
                }

                identifierName.DetectApplyThis = false;

                exp.OverrideOpeartor = ":";
            }
        }

        protected override string InnerTranslate()
        {
            if (Syntax.IsKind(SyntaxKind.ArrayInitializerExpression))
            {
                return $"[ {Expressions.Translate()} ]"; ;
            }
            return $"{{ {Expressions.Translate()} }}";
        }
    }
}
