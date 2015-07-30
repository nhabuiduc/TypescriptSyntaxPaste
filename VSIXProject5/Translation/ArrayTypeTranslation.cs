using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ArrayTypeTranslation : TypeTranslation
    {
        public new ArrayTypeSyntax Syntax
        {
            get { return (ArrayTypeSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public ArrayTypeTranslation() { }
        public ArrayTypeTranslation(ArrayTypeSyntax syntax,  SyntaxTranslation parent) : base(syntax, parent)
        {

            RankSpecifiers = syntax.RankSpecifiers.Get<ArrayRankSpecifierSyntax, ArrayRankSpecifierTranslation>(this);
            ElementType = syntax.ElementType.Get<TypeTranslation>(this);
        }

        public SyntaxListTranslation<ArrayRankSpecifierSyntax,ArrayRankSpecifierTranslation> RankSpecifiers { get; set; }
        public TypeTranslation ElementType { get; set; }

        protected override string InnerTranslate()
        {
            string elementTypeStr = ElementType.Translate();
            
            // in javascript for byte array, we will use 
            if(elementTypeStr == "byte")
            {
                return "Int8Array";
            }

            return $"{elementTypeStr}{RankSpecifiers.Translate()}";
        }
    }
}
