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
    public class LabeledStatementTranslation : SyntaxTranslation
    {
        public new LabeledStatementSyntax Syntax
        {
            get { return (LabeledStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public LabeledStatementTranslation() { }
        public LabeledStatementTranslation(LabeledStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Statement = syntax.Statement.Get<StatementTranslation>(this);
        }

        public StatementTranslation Statement { get; set; }

        public override void ApplyPatch()
        {
            base.ApplyPatch();
            
        }

        public bool TakeCare { get; set; }

        public bool IgnoreLabel { get; set; }

        protected override string InnerTranslate()
        {

            //string add = TakeCare ? "(^_^)" :"";
            string label = IgnoreLabel ? string.Empty : Syntax.Identifier.ToString() + ":"; 
            return $@"{label}
                 {Statement.Translate()}";
        }
    }
}
