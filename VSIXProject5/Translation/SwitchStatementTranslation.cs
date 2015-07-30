using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Patch;

namespace RoslynTypeScript.Translation
{
    public class SwitchStatementTranslation : StatementTranslation
    {
        public new SwitchStatementSyntax Syntax
        {
            get { return (SwitchStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public SwitchStatementTranslation() { }
        public SwitchStatementTranslation(SwitchStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
            Sections = syntax.Sections.Get<SwitchSectionSyntax, SwitchSectionTranslation>(this);
        }

        public ExpressionTranslation Expression { get; set; }
        public SyntaxListTranslation<SwitchSectionSyntax, SwitchSectionTranslation> Sections { get; set; }

        public bool GotoExists { get; set; }
        public bool GotoDefaultExists { get; set; }

        public override void ApplyPatch()
        {
            base.ApplyPatch();
        }

        protected override string InnerTranslate()
        {
            return $@"switch({Expression.Translate()})
                {{
                {Sections.Translate()}
                }}";
        }
    }
}
