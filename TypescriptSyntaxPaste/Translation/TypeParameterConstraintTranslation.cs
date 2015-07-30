using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class TypeParameterConstraintTranslation : CSharpSyntaxTranslation
    {
        public new TypeParameterConstraintSyntax Syntax
        {
            get { return (TypeParameterConstraintSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public TypeParameterConstraintTranslation() { }
        public TypeParameterConstraintTranslation(TypeParameterConstraintSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }


        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
