using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class AttributeListTranslation : CSharpSyntaxTranslation
    {
        public new AttributeListSyntax Syntax
        {
            get { return (AttributeListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public AttributeListTranslation() { }
        public AttributeListTranslation(AttributeListSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }

        protected override string InnerTranslate()
        {
            return $" /*{Syntax.ToString()}*/";
        }
    }
}
