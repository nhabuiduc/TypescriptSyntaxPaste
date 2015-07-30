using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class QualifiedNameTranslation : NameTranslation
    {
        public new QualifiedNameSyntax Syntax
        {
            get { return (QualifiedNameSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public QualifiedNameTranslation(QualifiedNameSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Left = syntax.Left.Get<NameTranslation>(this);
            Right = syntax.Right.Get<SimpleNameTranslation>(this);

            var genericTranslation = Left as GenericNameTranslation;
            if (genericTranslation != null)
            {
                ((GenericNameTranslation)Left).ExcludeTypeParameter = true;

            }

            var simpleName = Right as SimpleNameTranslation;
            if (simpleName != null)
            {
                simpleName.DetectApplyThis = false;
                var identifierName = simpleName as IdentifierNameTranslation;
                if (genericTranslation != null && identifierName!=null)
                {
                    identifierName.TypeArgumentList = genericTranslation.TypeArgumentList;
                }
                
            }
        }

        public override string GetTypeIgnoreGeneric()
        {
            return $"{Left.GetTypeIgnoreGeneric()}.{Right.Translate()}";
        }

        public NameTranslation Left { get; set; }
        public SimpleNameTranslation Right { get; set; }

        protected override string InnerTranslate()
        {
            return $"{Left.Translate()}.{Right.Translate()}";
        }
    }
}
