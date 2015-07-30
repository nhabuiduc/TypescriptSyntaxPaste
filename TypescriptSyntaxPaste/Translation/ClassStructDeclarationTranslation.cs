using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTypeScript.Patch;

namespace RoslynTypeScript.Translation
{
    public abstract class ClassStructDeclarationTranslation :TypeDeclarationTranslation
    {
        public ClassStructDeclarationTranslation(TypeDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            if (syntax.BaseList != null)
            {
                BaseList = syntax.BaseList.Get<BaseListTranslation>(this);                
            }           
        }

        public override void ApplyPatch()
        {
            base.ApplyPatch();
           // ConstructorPatch constructorPatch = new ConstructorPatch();
           // constructorPatch.Apply(this);
        }

        public BaseListTranslation BaseList { get; set; }

        public bool HasExplicitBase()
        {
            if (Syntax.BaseList == null)
            {
                return false;
            }

            return BaseList.GetBaseClass() != null;
        }
    }
}
