using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTypeScript.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class OperatorDeclarationTranslation : BaseMethodDeclarationTranslation
    {
        public new OperatorDeclarationSyntax Syntax
        {
            get { return (OperatorDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public OperatorDeclarationTranslation() { }
        public OperatorDeclarationTranslation(OperatorDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            ReturnType = syntax.ReturnType.Get<TypeTranslation>(this);
            Identifier = new TokenTranslation { SyntaxString = Helper.OperatorToMethod(syntax.OperatorToken.ToString()) };
        }

        //public ArrowExpressionClauseSyntax ExpressionBody { get; set; }

        public TypeTranslation ReturnType { get; set; }


        protected override string InnerTranslate()
        {

            // currently, I only support == != > < >= <=
            string originalOpeartor = Syntax.OperatorToken.ToString();
            if(!Helper.IsSupportOperator(originalOpeartor))
            {
                throw new NotSupportedException();
            }

            // this is static method -> to normal method
            var firstParam = ParameterList.Parameters.GetEnumerable().First();
            string firstParamStr = firstParam.Identifier.Translate();
            ParameterList.Parameters.Remove(firstParam);
            return $@" public {Identifier} {ParameterList}: {ReturnType}
                    {{
                    var {firstParamStr} = this;
                    {Body.Statements.Translate()}
                    }}
                ";
        }
    }
}
