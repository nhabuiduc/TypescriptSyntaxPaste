using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public abstract class SwitchLabelTranslation : CSharpSyntaxTranslation
    {
        public new SwitchLabelSyntax Syntax
        {
            get { return (SwitchLabelSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public SwitchLabelTranslation() { }
        public SwitchLabelTranslation(SwitchLabelSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
