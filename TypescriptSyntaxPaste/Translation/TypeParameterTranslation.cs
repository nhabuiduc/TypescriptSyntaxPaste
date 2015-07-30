using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class TypeParameterTranslation : CSharpSyntaxTranslation
    {
        public new TypeParameterSyntax Syntax
        {
            get { return (TypeParameterSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public TypeParameterTranslation(TypeParameterSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }

        public TypeTranslation TypeConstraint { get; set; }

        public bool IsExcludeConstraint { get; set; }

        protected override string InnerTranslate()
        {
            if(TypeConstraint!=null && !IsExcludeConstraint)
            {
                return $"{Syntax.Identifier} extends {TypeConstraint.Translate()}";
            }

            return Syntax.Identifier.ToString();
        }
    }
}
