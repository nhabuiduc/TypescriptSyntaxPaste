using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class TypeConstraintTranslation : TypeParameterConstraintTranslation
    {
        public new TypeConstraintSyntax Syntax
        {
            get { return (TypeConstraintSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public TypeConstraintTranslation() { }
        public TypeConstraintTranslation(TypeConstraintSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Type = syntax.Type.Get<TypeTranslation>(this);
        }

        public TypeTranslation Type { get; set; }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
