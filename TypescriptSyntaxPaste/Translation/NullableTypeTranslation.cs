using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class NullableTypeTranslation : TypeTranslation
    {
        public new NullableTypeSyntax Syntax
        {
            get { return (NullableTypeSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public NullableTypeTranslation() { }
        public NullableTypeTranslation(NullableTypeSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            ElementType = syntax.ElementType.Get<TypeTranslation>(this);
        }

        public TypeTranslation ElementType { get; set; }

        protected override string InnerTranslate()
        {
            return ElementType.Translate();
        }
    }
}
