using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.VirtualTranslation
{
    public class PredicateGenericNameTranslation : BaseFunctionGenericNameTranslation
    {
        public PredicateGenericNameTranslation(GenericNameTranslation genericNameTranslation) :base(genericNameTranslation)
        {            
        }

        protected override string InnerTranslate()
        {
            var firstParam = genericNameTranslation.TypeArgumentList.Arguments.GetEnumerable().First();
            return $"(_:{firstParam.Translate()})=>boolean";
        }
    }
}
