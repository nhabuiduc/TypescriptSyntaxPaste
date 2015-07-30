using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class PropertyDeclarationTranslation : BasePropertyDeclarationTranslation
    {
        public new PropertyDeclarationSyntax Syntax
        {
            get { return (PropertyDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public TokenTranslation Identifier { get; set; }


        public PropertyDeclarationTranslation(PropertyDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Identifier = syntax.Identifier.Get(this);
        }


        protected override string InnerTranslate()
        {
            var found = TravelUp(f => f is ClassDeclarationTranslation || f is InterfaceDeclarationTranslation);

            // following TypeScript with interface, we just need set as Field
            if (found is InterfaceDeclarationTranslation)
            {
                //return string.Format("{0}: {1}", syntax.Identifier,type.Translate());
                return $"{Helper.GetAttributeList(Syntax.AttributeLists)}{Syntax.Identifier}: {Type.Translate()} ;";
            }

            // hmm, if it's in class, much thing to do

            if (AccessorList.IsShorten())
            {
             
                var defaultStr = Helper.GetDefaultValue(Type);
                if (defaultStr == "null")
                {
                    defaultStr = string.Empty;
                }
                else
                {
                    defaultStr = " = " + defaultStr;
                }
                return $"{Helper.GetAttributeList(Syntax.AttributeLists)}public {Syntax.Identifier}: {Type.Translate()}{defaultStr} ;";
            }

            return AccessorList.Translate();
        }
    }
}
