using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public abstract class ExpressionTranslation : CSharpSyntaxTranslation
    {
        public ExpressionTranslation() { }
        public ExpressionTranslation(ExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }
    }
}
