using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public abstract class BaseParameterListTranslation : CSharpSyntaxTranslation
    {
        public new BaseParameterListSyntax Syntax
        {
            get { return (BaseParameterListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public BaseParameterListTranslation() { }
        public BaseParameterListTranslation(BaseParameterListSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Parameters = syntax.Parameters.Get<ParameterSyntax, ParameterTranslation>(this);
        }

        public SeparatedSyntaxListTranslation<ParameterSyntax,ParameterTranslation> Parameters { get; set; }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
