using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class TypeArgumentListTranslation : CSharpSyntaxTranslation
    {
        public new TypeArgumentListSyntax Syntax
        {
            get { return (TypeArgumentListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public TypeArgumentListTranslation() { }
        public TypeArgumentListTranslation(TypeArgumentListSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Arguments = syntax.Arguments.Get<TypeSyntax, TypeTranslation>(this);
        }

        public SeparatedSyntaxListTranslation<TypeSyntax,TypeTranslation> Arguments { get; set; }

        protected override string InnerTranslate()
        {
            return $"<{Arguments.Translate()}> ";
        }
    }
}
