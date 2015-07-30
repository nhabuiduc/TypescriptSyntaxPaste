using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynTypeScript.Translation
{
    public class ArgumentTranslation : SyntaxTranslation
    {
        public new ArgumentSyntax Syntax
        {
            get { return (ArgumentSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public ArgumentTranslation() { }

        public ArgumentTranslation(ArgumentSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
        }

        public ExpressionTranslation Expression { get; set; }
        

        public bool IsExistingRefOrOutKeyword
        {
            get { return !Syntax.RefOrOutKeyword.Span.IsEmpty; }
        }

        public override void ApplyPatch()
        {
            base.ApplyPatch();
            Helper.ApplyFunctionBindToCorrectContext(this.Expression);
        }

        protected override string InnerTranslate()
        {
          
            string nameColon = string.Empty;
            if (Syntax.NameColon != null)
            {
                nameColon = $"/*{Syntax.NameColon.ToString()}*/";
            }
            return $"{nameColon}{Expression.Translate()}";
        }
    }
}
