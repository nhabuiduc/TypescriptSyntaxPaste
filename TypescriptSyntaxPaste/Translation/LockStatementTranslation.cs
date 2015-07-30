using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class LockStatementTranslation : StatementTranslation
    {
        public new LockStatementSyntax Syntax
        {
            get { return (LockStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public LockStatementTranslation() { }
        public LockStatementTranslation(LockStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
            Statement = syntax.Statement.Get<StatementTranslation>(this);
        }

        public ExpressionTranslation Expression { get; set; }
        public StatementTranslation Statement { get; set; }

        protected override string InnerTranslate()
        {
            return $@"lock({Expression.Translate()})
                {Statement.Translate()}";
        }
    }
}
