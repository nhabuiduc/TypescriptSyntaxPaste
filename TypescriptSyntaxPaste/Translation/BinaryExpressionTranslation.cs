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
    public class BinaryExpressionTranslation : ExpressionTranslation
    {
        public new BinaryExpressionSyntax Syntax
        {
            get { return (BinaryExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public BinaryExpressionTranslation() { }
        public BinaryExpressionTranslation(BinaryExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Left = syntax.Left.Get<ExpressionTranslation>(this);
            Right = syntax.Right.Get<ExpressionTranslation>(this);
        }

        public ExpressionTranslation Left { get; set; }
        public ExpressionTranslation Right { get; set; }

        protected override string InnerTranslate()
        {
            string tokenStr = Syntax.OperatorToken.ToString();           

            if (Syntax.IsKind(SyntaxKind.AsExpression))
            {
                return $"__as__<{Right.Translate()}> ({Left.Translate()},{Right.Translate()})";
            }

            if (Syntax.IsKind(SyntaxKind.IsExpression))
            {
                tokenStr = "instanceof";
                var right = (TypeTranslation)Right;
                var rightStr = right.GetTypeIgnoreGeneric();
                switch(rightStr)
                {
                    case "boolean":
                    case "number":
                    case "string":
                        return $"typeof {Left.Translate()} === \"{rightStr}\"";
                }

                return $"{Left.Translate()} instanceof {rightStr}";
            }

            if (Syntax.IsKind(SyntaxKind.CoalesceExpression))
            {
                // only process with case left is identifier name
                if(Left is IdentifierNameTranslation || Left is MemberAccessExpressionTranslation)
                {
                    // ?? -> != null ? :
                    string leftStr = Left.Translate();
                    string rightStr = Right.Translate();
                    
                    return $"{leftStr} != null ? {leftStr} : {rightStr}";
                }                
            }

            if (Syntax.IsKind(SyntaxKind.DivideAssignmentExpression) )
            {
                return $"{Left.Translate()} = {Left.Translate()} \\ {Right.Translate()}";
            }

            if (Syntax.IsKind(SyntaxKind.MultiplyAssignmentExpression))
            {
                return $"{Left.Translate()} = {Left.Translate()} * {Right.Translate()}";
            }

            return $"{Left.Translate()} {tokenStr} {Right.Translate()}";
        }
    }
}
