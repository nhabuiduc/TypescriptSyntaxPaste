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
    public class AccessorDeclarationTranslation : CSharpSyntaxTranslation
    {
        public new AccessorDeclarationSyntax Syntax
        {
            get { return (AccessorDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public AccessorDeclarationTranslation(AccessorDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Body = syntax.Body.Get<BlockTranslation>(this);
            Modifiers = syntax.Modifiers.Get(this);
        }


        public BlockTranslation Body { get; set; }
        public SyntaxTokenListTranslation ParentModifiers
        {
            get { return Modifiers; }
            set { Modifiers = value; }
        }

        public SyntaxTokenListTranslation Modifiers { get; set; }

        protected override string InnerTranslate()
        {
            var ancestor = GetAncestor<BasePropertyDeclarationTranslation>();
            if (ancestor is IndexerDeclarationTranslation)
            {
                return BuildIndexerAccessor();
            }

            return BuildPropertyAccessor();
        }

        private string BuildIndexerAccessor()
        {
            var ancestor = GetAncestor<IndexerDeclarationTranslation>();

            string keyword = Syntax.Keyword.ToString();

            string bodyStr = Body?.Translate() ?? "{ throw new System.NotImplementedException();}";

            if (keyword == "get")
            {
                return $@"{Modifiers.Translate()} {TC.IndexerGetName}({ancestor.ParameterList.Parameters.Translate()}) :{ancestor.Type.Translate()}
                    {bodyStr}";
            }
            else
            {
                return $@"{Modifiers.Translate()} {TC.IndexerSetName}({ancestor.ParameterList.Parameters.Translate()}, value :{ancestor.Type.Translate()}):void
                {bodyStr}";
            }
        }

        private string BuildPropertyAccessor()
        {
            var ancestor = GetAncestor<PropertyDeclarationTranslation>();

            string keyword = Syntax.Keyword.ToString();

            if (keyword == "get")
            {
                return string.Format(@"{0} get {1}(): {2}
{3}", Modifiers.Translate(), ancestor.Identifier.Translate(), ancestor.Type.Translate(), Body.Translate());

            }

            return string.Format(@"{0} set {1}(value: {2})
{3}", Modifiers.Translate(), ancestor.Identifier.Translate(), ancestor.Type.Translate(), Body.Translate());
        }

        public bool IsShorten()
        {
            return Syntax.SemicolonToken.ToString() == ";";
        }
    }
}
