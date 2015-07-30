using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class ObjectCreationExpressionTranslation : ExpressionTranslation
    {
        public new ObjectCreationExpressionSyntax Syntax
        {
            get { return (ObjectCreationExpressionSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public ObjectCreationExpressionTranslation() { }
        public ObjectCreationExpressionTranslation(ObjectCreationExpressionSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            ArgumentList = syntax.ArgumentList.Get<ArgumentListTranslation>(this);
            Initializer = syntax.Initializer.Get<InitializerExpressionTranslation>(this);
            Type = syntax.Type.Get<TypeTranslation>(this);
        }

        public ArgumentListTranslation ArgumentList { get; set; }
        public InitializerExpressionTranslation Initializer { get; set; }
        public TypeTranslation Type { get; set; }

        protected override string InnerTranslate()
        {
            var name = Type.Translate();

            // the case object creation only by Initializer
            if(ArgumentList == null)
            {
                ArgumentList = new ArgumentListTranslation()
                {
                    Parent = this,
                    SyntaxString = "()"
                };
            }


            if (Initializer == null )
            {

                return $"new {Type.Translate()} {ArgumentList.Translate()}";
            }

            return $" __init(new {Type.Translate()} {ArgumentList.Translate()},{Initializer.Translate()})";
        }
    }
}
