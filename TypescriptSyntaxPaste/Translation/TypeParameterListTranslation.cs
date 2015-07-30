using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class TypeParameterListTranslation : SyntaxTranslation
    {
        public new TypeParameterListSyntax Syntax
        {
            get { return (TypeParameterListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public SeparatedSyntaxListTranslation<TypeParameterSyntax, TypeParameterTranslation> Parameters { get; set; }

        public TypeParameterListTranslation() { }

        public TypeParameterListTranslation(TypeParameterListSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Parameters = syntax.Parameters.Get<TypeParameterSyntax, TypeParameterTranslation>(this);
        }

        private bool isExcludeConstraint;
        public bool IsExcludeConstraint
        {
            get { return this.isExcludeConstraint; }
            set
            {
                this.isExcludeConstraint = value;
                foreach (var item in Parameters.GetEnumerable())
                {
                    item.IsExcludeConstraint = value;
                }
            }
        }

        protected override string InnerTranslate()
        {
            return string.Format("<{0}>", Parameters.Translate());
        }
    }
}
