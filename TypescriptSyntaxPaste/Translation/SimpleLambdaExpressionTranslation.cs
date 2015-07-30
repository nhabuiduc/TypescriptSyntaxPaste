using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class SimpleLambdaExpressionTranslation : ExpressionTranslation
    {
        public new SimpleLambdaExpressionSyntax Syntax
        {
            get { return (SimpleLambdaExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public SimpleLambdaExpressionTranslation() { }
        public SimpleLambdaExpressionTranslation(SimpleLambdaExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Body = syntax.Body.Get<CSharpSyntaxTranslation>(this);
            Parameter = syntax.Parameter.Get<ParameterTranslation>(this);
        }

        public CSharpSyntaxTranslation Body { get; set; }
        public ParameterTranslation Parameter { get; set; }

        protected override string InnerTranslate()
        {
            //return Syntax.ToString();
            return $"{Parameter.Translate()} => {Body.Translate()}";
        }
    }
}
