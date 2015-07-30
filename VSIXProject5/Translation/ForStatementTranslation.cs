using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ForStatementTranslation : StatementTranslation
    {
        public new ForStatementSyntax Syntax
        {
            get { return (ForStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public ForStatementTranslation() { }
        public ForStatementTranslation(ForStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            //KeyValuePair<int, string>? a = null;            
            //var b = ((bool?)null)?.Equals(false) ?? true ? false ? true : false :true;

            //string str = "\{ ("\{( "(\{ "{}" })" )}") }";

            Condition = syntax.Condition.Get<ExpressionTranslation>(this);
            Declaration = syntax.Declaration.Get<VariableDeclarationTranslation>(this);
            Incrementors = syntax.Incrementors.Get<ExpressionSyntax, ExpressionTranslation>(this);
            Initializers = syntax.Initializers.Get<ExpressionSyntax, ExpressionTranslation>(this);
            Statement = syntax.Statement.Get<StatementTranslation>(this);
        }

        public ExpressionTranslation Condition { get; set; }
        public VariableDeclarationTranslation Declaration { get; set; }
        public SeparatedSyntaxListTranslation<ExpressionSyntax, ExpressionTranslation> Incrementors { get; set; }
        public SeparatedSyntaxListTranslation<ExpressionSyntax, ExpressionTranslation> Initializers { get; set; }
        public StatementTranslation Statement { get; }

        protected override string InnerTranslate()
        {
            return $@"for({Declaration?.Translate()};{Condition?.Translate()};{Incrementors?.Translate()})
                {Statement.Translate()}";
        }
    }
}
