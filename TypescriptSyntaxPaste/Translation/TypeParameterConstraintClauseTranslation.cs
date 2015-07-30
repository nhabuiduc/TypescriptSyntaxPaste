using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class TypeParameterConstraintClauseTranslation : SyntaxTranslation
    {
        public new TypeParameterConstraintClauseSyntax Syntax
        {
            get { return (TypeParameterConstraintClauseSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public TypeParameterConstraintClauseTranslation() { }
        public TypeParameterConstraintClauseTranslation(TypeParameterConstraintClauseSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Constraints = syntax.Constraints.Get<TypeParameterConstraintSyntax, TypeParameterConstraintTranslation>(this);
            Name = syntax.Name.Get<IdentifierNameTranslation>(this);
        }

        public SeparatedSyntaxListTranslation<TypeParameterConstraintSyntax, TypeParameterConstraintTranslation> Constraints { get; set; }
        public IdentifierNameTranslation Name { get; set; }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
