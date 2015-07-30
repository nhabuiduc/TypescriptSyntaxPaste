using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class QueryExpressionTranslation : ExpressionTranslation
    {
        public new QueryExpressionSyntax Syntax
        {
            get { return (QueryExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public QueryExpressionTranslation() { }
        public QueryExpressionTranslation(QueryExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {

        }

        protected override string InnerTranslate()
        {
            return Syntax.ToString();
        }
    }
}
