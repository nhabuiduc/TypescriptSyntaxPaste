using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTypeScript.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynTypeScript.Translation
{
    public class AssignmentExpressionTranslation : ExpressionTranslation
    {
        public new AssignmentExpressionSyntax Syntax
        {
            get { return (AssignmentExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public ExpressionTranslation Left { get; set; }
        public ExpressionTranslation Right { get; set; }

        public string OverrideOpeartor { get; set; }

        public AssignmentExpressionTranslation() { }
        public AssignmentExpressionTranslation(AssignmentExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Left = syntax.Left.Get<ExpressionTranslation>(this);
            Right = syntax.Right.Get<ExpressionTranslation>(this);
        }

        public override void ApplyPatch()
        {
            base.ApplyPatch();
            Helper.ApplyFunctionBindToCorrectContext(this.Right);
        }

        protected override string InnerTranslate()
        {
            if(OverrideOpeartor != null)
            {
                return $"{Left.Translate()} {OverrideOpeartor}  {Right.Translate()}";
            }
        
            var operatorToken = Syntax.OperatorToken.ToString();
            if (Helper.IsInKinds(this.Syntax,
                SyntaxKind.OrAssignmentExpression,
                SyntaxKind.AndAssignmentExpression,
                SyntaxKind.BitwiseOrExpression,
                SyntaxKind.BitwiseAndExpression))
            {
                switch (this.Syntax.Kind())
                {
                    case SyntaxKind.OrAssignmentExpression:
                        return $"{Left.Translate()} = {Left.Translate()} || {Right.Translate()} ";
                    case SyntaxKind.AndAssignmentExpression:
                        return $"{Left.Translate()} = {Left.Translate()} && {Right.Translate()} ";
                    case SyntaxKind.BitwiseOrExpression:
                        operatorToken = "||";
                        break;
                    case SyntaxKind.BitwiseAndExpression:
                        operatorToken = "&&";
                        break;
                }
            }

            var rightStr = Right.Translate();
           
            return string.Format("{0} {1} {2}", Left.Translate(), operatorToken, rightStr);
        }
    }
}
