using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace RoslynTypeScript.Translation
{
    public abstract class CSharpSyntaxTranslation :SyntaxTranslation
    {
        public CSharpSyntaxTranslation() { }
        public CSharpSyntaxTranslation(SyntaxNode syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
        }
    }
}
