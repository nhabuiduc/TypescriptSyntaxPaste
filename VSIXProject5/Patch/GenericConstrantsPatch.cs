using RoslynTypeScript.Contract;
using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Patch
{
    public class GenericConstrantsPatch : Patch
    {
        /// <summary>
        /// Buid generic constrant
        /// </summary>
        /// <param name="typeDeclarationTranslation"></param>
        public void Apply(ITypeParameterConstraint typeDeclarationTranslation)
        {

            if(typeDeclarationTranslation.TypeParameterList == null)
            {
                return;
            }

            var parameters = typeDeclarationTranslation.TypeParameterList.Parameters.GetEnumerable();
            foreach (TypeParameterTranslation item in parameters)
            {
                var foundClause = typeDeclarationTranslation.ConstraintClauses
                    .GetEnumerable().FirstOrDefault(f => f.Name.Translate() == item.Syntax.Identifier.ToString());
                if (foundClause == null)
                {
                    continue;
                }

                var constraints = foundClause.Constraints.GetEnumerable().OfType<TypeConstraintTranslation>().ToArray();
                if (!constraints.Any())
                {
                    continue;
                }

                if (constraints.Length > 1)
                {
                    throw new NotSupportedException("not support multiple constrants");
                }

                item.TypeConstraint = constraints[0].Type;

            }
        }

    }
}
