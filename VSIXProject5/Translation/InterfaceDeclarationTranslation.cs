using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class InterfaceDeclarationTranslation : TypeDeclarationTranslation
    {
        public new InterfaceDeclarationSyntax Syntax
        {
            get { return (InterfaceDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public InterfaceDeclarationTranslation(InterfaceDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

            if (syntax.BaseList != null)
            {
                BaseList = syntax.BaseList.Get<BaseListTranslation>(this);
                BaseList.IsForInterface = true;
            }
        }

        public BaseListTranslation BaseList { get; set; }

        protected override string InnerTranslate()
        {
            string baseTranslation = BaseList?.Translate();
            return $@"{GetAttributeList()}export interface {Syntax.Identifier} {TypeParameterList?.Translate()} {baseTranslation}
                {{
                {Members.Translate()} 
                }}";
        }
    }
}
