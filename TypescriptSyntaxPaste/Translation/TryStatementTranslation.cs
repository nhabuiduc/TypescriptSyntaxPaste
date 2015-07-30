using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class TryStatementTranslation : CSharpSyntaxTranslation
    {
        public new TryStatementSyntax Syntax
        {
            get { return (TryStatementSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public TryStatementTranslation() { }
        public TryStatementTranslation(TryStatementSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Block = syntax.Block.Get<BlockTranslation>(this);
            Catches = syntax.Catches.Get<CatchClauseSyntax, CatchClauseTranslation>(this);
            Finally = syntax.Finally.Get<FinallyClauseTranslation>(this);
        }

        public BlockTranslation Block { get; set; }
        public SyntaxListTranslation<CatchClauseSyntax, CatchClauseTranslation> Catches { get; set; }
        public FinallyClauseTranslation Finally { get; set; }

        protected override string InnerTranslate()
        {
            string finallyStr = "";
            if (Finally != null)
            {
                finallyStr = Finally.Translate();
            }

            

            if (Catches.GetEnumerable().Count() > 1)
            {
                return $@"try
              {Block.Translate()}
              catch(__ex__){{
              {BuildCatchBlock()}
              }}
              {finallyStr}";
            }

            return $@"try
                {Block.Translate()}
                {Catches.Translate()}
                {finallyStr}";
        }

        private string BuildCatchBlock()
        {
            StringBuilder bd = new StringBuilder();
            foreach (var item in Catches.GetEnumerable())
            {
                string str = "";
                string name = "__ex__";
                if (item.Syntax.Declaration.Identifier.ToString() != string.Empty)
                {
                    str = $"var {item.Syntax.Declaration.Identifier } = __ex__;";
                    name = item.Syntax.Declaration.Identifier.ToString();
                }

                str += $"if( {name } instanceof {item.Syntax.Declaration.Type})"
                    + $"\n {item.Block} \n";
                bd.Append(str);
            }

            return bd.ToString();
        }
    }
}
