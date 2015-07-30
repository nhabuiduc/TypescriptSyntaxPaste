using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ArgumentListTranslation : CSharpSyntaxTranslation
    {
        public new ArgumentListSyntax Syntax
        {
            get { return (ArgumentListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public SeparatedSyntaxListTranslation<ArgumentSyntax,ArgumentTranslation> Arguments { get; set; }
        public ArgumentListTranslation() { }
        public ArgumentListTranslation(ArgumentListSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Arguments = syntax.Arguments.Get<ArgumentSyntax, ArgumentTranslation>(this);
        }


         protected override string InnerTranslate()
        {
            return string.Format("({0})", Arguments.Translate());
        }
    }
}
