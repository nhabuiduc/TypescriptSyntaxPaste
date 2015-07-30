using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Translation;

namespace RoslynTypeScript.Contract
{
    public interface IBaseExtended
    {
        BaseListTranslation BaseList { get; set; }
    }
}
