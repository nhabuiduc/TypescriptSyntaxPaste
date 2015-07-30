using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public abstract class TypeTranslation : ExpressionTranslation
    {
        public TypeTranslation() { }
        public TypeTranslation(TypeSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }

        public bool ActAsTypeParameter { get; set; }

        public virtual bool IsPrimitive
        {
            get { return false; }
        }

        public virtual string GetTypeIgnoreGeneric()
        {
            return this.Translate();
        }
    }
}
