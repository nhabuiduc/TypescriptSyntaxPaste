using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public abstract class BaseArgumentListTranslation : CSharpSyntaxTranslation
    {
        public new BaseArgumentListSyntax Syntax
        {
            get { return (BaseArgumentListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public BaseArgumentListTranslation() { }
        public BaseArgumentListTranslation(BaseArgumentListSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Arguments = syntax.Arguments.Get<ArgumentSyntax, ArgumentTranslation>(this);
        }

        public SeparatedSyntaxListTranslation<ArgumentSyntax, ArgumentTranslation> Arguments { get; set; }
    }
}
