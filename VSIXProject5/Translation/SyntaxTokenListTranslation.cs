using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class SyntaxTokenListTranslation : CSharpSyntaxTranslation
    {

        private IEnumerable<TokenTranslation> tokens;
        private SyntaxTokenList tokenListSyntax;
        public SyntaxTokenListTranslation() { }
        public SyntaxTokenListTranslation(SyntaxTokenList syntax, SyntaxTranslation parent) : base(null, parent)
        {
            tokenListSyntax = syntax;
            tokens = syntax.Select(f => f.Get(this)).ToArray();
        }

        public bool ConstantToStatic { get; set; }

        protected override string InnerTranslate()
        {
            var result =  string.Join(" ", tokens.Select(f => Filter(f.Translate())));
            result = result.Replace("static public", "public static");
            return result;
        }

        public bool IsAbstract
        {
            get { return tokens.Any(f => f.Token.ToString() == "abstract"); }
        }

        public bool IsOverride
        {
            get { return tokens.Any(f => f.Token.ToString() == "override"); }
        }

        public bool IsStatic
        {
            get { return tokens.Any(f => f.Token.ToString() == "static" || f.Token.ToString() == "const"); }
        }

        public bool IsProtected
        {
            get { return tokens.Any(f => f.Token.ToString() == "protected"); }
        }

        public bool IsPrivate
        {
            get { return tokens.Any(f => f.Token.ToString() == "private"); }
        }

        public bool IsInternal
        {
            get { return tokens.Any(f => f.Token.ToString() == "internal"); }
        }

        public SyntaxTokenList TokenListSyntax
        {
            get { return tokenListSyntax; }
        }

        private string Filter(string str)
        {

            var result = str.Replace("internal", "public")
                        .Replace("volatile","")
                        .Replace("override", "")
                        .Replace("readonly", "")
                        .Replace("abstract", "")
                        .Replace("new", "")
                        .Replace("virtual", "")
                        .Replace("unsafe", "")
                        .Replace("sealed", "")
                        .Replace("const", ConstantToStatic ? "static" : "");

            return result;
        }


    }
}
