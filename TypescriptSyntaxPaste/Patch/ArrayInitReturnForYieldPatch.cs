using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Constants;
using RoslynTypeScript.Translation;

namespace RoslynTypeScript.Patch
{
    /// <summary>
    /// try to replace yield statement with array.push, then return array at the end of method.
    /// </summary>
    public class ArrayInitReturnForYieldPatch : Patch
    {
        public void Apply(MethodDeclarationTranslation method)
        {
            if (!method.IsYieldReturn)
            {
                return;
            }

            var arrayCreation = new ArrayCreationExpressionTranslation();
            string typeParemter = string.Empty;
            var genericType = method.ReturnType as GenericNameTranslation;
            if (genericType != null)
            {
                typeParemter = genericType.TypeArgumentList.Translate();
            }
            arrayCreation.SyntaxString = $"var {TC.YieldResultName} = new Array{typeParemter}();";

            var returnStatement = new ReturnStatementTranslation();
            returnStatement.SyntaxString = $"return {TC.YieldResultName};";

            method.Body.Statements.Insert(0, arrayCreation);
            method.Body.Statements.Add(returnStatement);
        }
    }
}
