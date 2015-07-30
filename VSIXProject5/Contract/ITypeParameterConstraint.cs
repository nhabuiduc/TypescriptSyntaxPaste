using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Contract
{
    public interface ITypeParameterConstraint
    {
        TypeParameterListTranslation TypeParameterList { get; set; }

        SyntaxListTranslation<TypeParameterConstraintClauseSyntax, TypeParameterConstraintClauseTranslation> ConstraintClauses { get; set; }
    }
}
