using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ForEachStatementTranslation : StatementTranslation
    {
        public new ForEachStatementSyntax Syntax
        {
            get { return (ForEachStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public ForEachStatementTranslation() { }
        public ForEachStatementTranslation(ForEachStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Expression = syntax.Expression.Get<ExpressionTranslation>(this);
            Statement = syntax.Statement.Get<StatementTranslation>(this);
            Type = syntax.Type.Get<TypeTranslation>(this);
        }

        public ExpressionTranslation Expression { get; set; }
        //public SyntaxToken Identifier { get; }
        public StatementTranslation Statement { get; set; }
        public TypeTranslation Type { get; set; }

        protected override string InnerTranslate()
        {

            var expression = Statement is BlockTranslation ? Statement.Translate() : "{" + Statement.Translate() + "}";
            var statement = @"{0}.forEach(function({1}){2});";

            return string.Format(statement, Expression.Translate(), Syntax.Identifier, expression);

        }
    }
}
