using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class FieldDeclarationTranslation : MemberDeclarationTranslation
    {
        public new FieldDeclarationSyntax Syntax
        {
            get { return (FieldDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public VariableDeclarationTranslation Declaration { get; set; }
        public SyntaxTokenListTranslation Modifiers { get; set; }

        public FieldDeclarationTranslation() { }
        public FieldDeclarationTranslation(FieldDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Declaration = syntax.Declaration.Get<VariableDeclarationTranslation>(this);
            Declaration.ExcludeVar = true;
            Modifiers = syntax.Modifiers.Get(this);
            Modifiers.ConstantToStatic = true;                     
        }

        protected override string InnerTranslate()
        {
            return string.Format("{0} {1};", Modifiers.Translate(), Declaration.Translate());
        }
    }
}
