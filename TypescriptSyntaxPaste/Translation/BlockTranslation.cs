using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class BlockTranslation : StatementTranslation
    {
        public new BlockSyntax Syntax
        {
            get { return (BlockSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public SyntaxListTranslation<StatementSyntax, StatementTranslation> Statements { get; set; }

        public BlockTranslation()
        {
            Statements = new SyntaxListTranslation<StatementSyntax, StatementTranslation>();
        }

        public BlockTranslation(BlockSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Statements = syntax.Statements.Get<StatementSyntax, StatementTranslation>(this);
        }

        public bool IsIgnoreBracket { get; set; }

        protected override string InnerTranslate()
        {
            if(IsIgnoreBracket)
            {
                return Statements.Translate();
            }

            return string.Format(@"{{
{0}
}}", Statements.Translate());
        }
    }
}
