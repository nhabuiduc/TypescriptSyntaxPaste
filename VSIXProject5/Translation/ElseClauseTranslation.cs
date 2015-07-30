using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ElseClauseTranslation : CSharpSyntaxTranslation
    {
        public new ElseClauseSyntax Syntax
        {
            get { return (ElseClauseSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public ElseClauseTranslation() { }
        public ElseClauseTranslation(ElseClauseSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Statement = syntax.Statement.Get<StatementTranslation>(this);
        }

        public StatementTranslation Statement { get; set; }

        protected override string InnerTranslate()
        {
            return $"else {Statement.Translate()}";
        }
    }
}
