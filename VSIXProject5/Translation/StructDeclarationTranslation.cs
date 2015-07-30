using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Patch;
using RoslynTypeScript.Contract;
using RoslynTypeScript.Constants;

namespace RoslynTypeScript.Translation
{
    public class StructDeclarationTranslation : ClassStructDeclarationTranslation, IBaseExtended
    {
        public new StructDeclarationSyntax Syntax
        {
            get { return (StructDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        //public StructDeclarationTranslation() { }
        public StructDeclarationTranslation(StructDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {              
            if(BaseList == null)
            {
                BaseList = new BaseListTranslation();
                BaseList.Parent = this;
                BaseList.Types = new SeparatedSyntaxListTranslation<BaseTypeSyntax, BaseTypeTranslation>();
                BaseList.Types.Parent = BaseList;
            }

            //BaseList.Types.Add(new BaseTypeTranslation() { SyntaxString = TC.IStruct });
        }


        public override void ApplyPatch()
        {
            base.ApplyPatch();
            //StructPatch structPatch = new StructPatch();
            //structPatch.Apply(this);
        }

        protected override string InnerTranslate()
        {
            string baseTranslation = BaseList?.Translate();

            return $@"{GetAttributeList()}export class {Syntax.Identifier}{TypeParameterList?.Translate()} {baseTranslation}
                {{
                {Members.Translate()} 
                }}";
        }
    }
}
