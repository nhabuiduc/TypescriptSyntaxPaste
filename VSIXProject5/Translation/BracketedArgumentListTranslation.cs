using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class BracketedArgumentListTranslation : BaseArgumentListTranslation
    {
        public new BracketedArgumentListSyntax Syntax
        {
            get { return (BracketedArgumentListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public BracketedArgumentListTranslation(BracketedArgumentListSyntax syntax,  SyntaxTranslation parent) : base(syntax, parent)
        {
            
        }     

        protected override string InnerTranslate()
        {
            return $"[{Arguments.Translate()}]";
        }
    }
}
