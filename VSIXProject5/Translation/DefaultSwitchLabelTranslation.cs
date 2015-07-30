using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class DefaultSwitchLabelTranslation : SwitchLabelTranslation
    {
        public new DefaultSwitchLabelSyntax Syntax
        {
            get { return (DefaultSwitchLabelSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public DefaultSwitchLabelTranslation() { }
        public DefaultSwitchLabelTranslation(DefaultSwitchLabelSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
