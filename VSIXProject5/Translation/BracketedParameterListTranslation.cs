using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class BracketedParameterListTranslation : BaseParameterListTranslation
    {
        public new BracketedParameterListSyntax Syntax
        {
            get { return (BracketedParameterListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public BracketedParameterListTranslation() { }
        public BracketedParameterListTranslation(BracketedParameterListSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }


        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
