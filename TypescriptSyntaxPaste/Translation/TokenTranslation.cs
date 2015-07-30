using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class TokenTranslation : SyntaxTranslation
    {
        private SyntaxToken token;
        public TokenTranslation()
        { }

        public TokenTranslation(SyntaxToken token, SyntaxTranslation parent) : base(null, parent)
        {
            this.token = token;

        }

        protected override string InnerTranslate()
        {
            return Helper.NormalizeVariabeleName(token.ToString());
        }

        public virtual bool IsEmpty
        {
            get { return token.ToString() == string.Empty; }
        }

        public SyntaxToken Token
        {
            get
            {
                return token;
            }
        }

        public bool TokenEquals(TokenTranslation another)
        {
            return this.ToString() == another.ToString();
        }
    }
}
