using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class EnumDeclarationTranslation : BaseTypeDeclarationTranslation
    {
        public new EnumDeclarationSyntax Syntax
        {
            get { return (EnumDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public EnumDeclarationTranslation() { }
        public EnumDeclarationTranslation(EnumDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Members = syntax.Members.Get<EnumMemberDeclarationSyntax, EnumMemberDeclarationTranslation>(this);
            Members.IsNewLine = true;
        }

        public SeparatedSyntaxListTranslation<EnumMemberDeclarationSyntax, EnumMemberDeclarationTranslation> Members { get; set; }


        protected override string InnerTranslate()
        {
            return $@"export enum {Syntax.Identifier} {{
                 {Members.Translate()}
                 }}";
        }
    }
}
