//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RoslynTypeScript.Translation
//{
//    public class NameOfExpressionTranslation : ExpressionTranslation
//    {
//        public new NameOfExpressionSyntax Syntax
//        {
//            get { return (NameOfExpressionSyntax)base.Syntax; }
//            set { base.Syntax = value; }
//        }

//        public NameOfExpressionTranslation() { }
//        public NameOfExpressionTranslation(NameOfExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
//        {
//            Argument = syntax.Argument.Get<ExpressionTranslation>(this);
//        }

//        public ExpressionTranslation Argument { get; set; }

//        protected override string InnerTranslate()
//        {
            
//            //return Syntax.ToString();
//            return $"'{Argument.Translate()}'";
//        }
//    }
//}
