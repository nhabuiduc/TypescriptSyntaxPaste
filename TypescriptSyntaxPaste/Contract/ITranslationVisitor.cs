using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Contract
{
    public interface ITranslationVisitor
    {
        void Visit(SyntaxTranslation translation);
    }
}
