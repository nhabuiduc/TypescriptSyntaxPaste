using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.VirtualTranslation
{
    public class FunctionTypeGenericNameTranslation : BaseFunctionGenericNameTranslation
    {
        public FunctionTypeGenericNameTranslation(GenericNameTranslation genericNameTranslation) :base(genericNameTranslation)
        {
            ReturnType = genericNameTranslation.TypeArgumentList.Arguments.GetEnumerable().Last();
            Arguments = new SeparatedSyntaxListTranslation<TypeSyntax, TypeTranslation>();
            Arguments.Add(genericNameTranslation.TypeArgumentList.Arguments.GetEnumerable().Where(f => f != ReturnType));
            this.Attach();
        }

        protected override string InnerTranslate()
        {
            List<string> list = new List<string>();
            string name = "";
            list = Arguments.GetEnumerable().Select(f => $"{name = GetFakeParamName(name)}:{f.Translate()}").ToList();

            return $"({string.Join(",", list)}) => {ReturnType.Translate()}";
        }
    }
}
