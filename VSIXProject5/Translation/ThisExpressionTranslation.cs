using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ThisExpressionTranslation : InstanceExpressionTranslation
    {
        public new ThisExpressionSyntax Syntax
        {
            get { return (ThisExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public ThisExpressionTranslation(ThisExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
