using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Translation;

namespace RoslynTypeScript.VirtualTranslation.Delegates
{
    public class ComparisonGenericNameTranslation : BaseFunctionGenericNameTranslation
    {
        public ComparisonGenericNameTranslation(GenericNameTranslation genericNameTranslation) : base(genericNameTranslation)
        {
        }

        protected override string InnerTranslate()
        {
            var firstParam = genericNameTranslation.TypeArgumentList.Arguments.GetEnumerable().First();
            string firstParamStr = firstParam.Translate();
            return $"(_:{firstParamStr}, __:{firstParamStr})=> number";
        }
    }
}
