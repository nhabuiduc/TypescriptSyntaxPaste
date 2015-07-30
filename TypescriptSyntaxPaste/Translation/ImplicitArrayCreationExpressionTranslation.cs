using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ImplicitArrayCreationExpressionTranslation : ExpressionTranslation
    {
        public new ImplicitArrayCreationExpressionSyntax Syntax
        {
            get { return (ImplicitArrayCreationExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public ImplicitArrayCreationExpressionTranslation() { }
        public ImplicitArrayCreationExpressionTranslation(ImplicitArrayCreationExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Initializer = syntax.Initializer.Get<InitializerExpressionTranslation>(this);
        }

        public InitializerExpressionTranslation Initializer { get; set; }

        protected override string InnerTranslate()
        {
            return $"new Array({Initializer.Expressions.Translate()})";
        }
    }
}
