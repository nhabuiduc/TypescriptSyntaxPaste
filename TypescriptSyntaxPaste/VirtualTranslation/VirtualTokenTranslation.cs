using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.VirtualTranslation
{
    public class VirtualTokenTranslation :TokenTranslation
    {
        public string TokenStr { get; set; }

         protected override string InnerTranslate()
        {
            return TokenStr;
        }

        public override bool IsEmpty
        {
            get
            {
                return TokenStr == string.Empty;
            }
        }

        public static VirtualTokenTranslation SemicolonToken
        {
            get { return new VirtualTokenTranslation { TokenStr = ";" }; }
        }
    }
}
