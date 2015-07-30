using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public static class SyntaxNodeExtensions
    {
        public static CompilationUnitTranslation GetCompilationUnit(this CompilationUnitSyntax syntax)
        {
            return new CompilationUnitTranslation(syntax, null);
        }

        public static T Get<T>(this SyntaxNode syntaxNode, SyntaxTranslation parent) where T : SyntaxTranslation
        {
            if (syntaxNode == null)
            {
                return null;
            }

            var node = TF.Get<T>(syntaxNode, parent);
            return node;
        }

        public static SyntaxTokenListTranslation Get(this SyntaxTokenList list, SyntaxTranslation parent)
        {
            var newList = new SyntaxTokenListTranslation(list, parent);
            return newList;
        }

        public static SyntaxListTranslation<T, TS> Get<T, TS>(this SyntaxList<T> syntaxList, SyntaxTranslation parent) where T : SyntaxNode where TS : SyntaxTranslation
        {
            var newList = new SyntaxListTranslation<T, TS>(syntaxList, parent);
            return newList;
        }

        public static SeparatedSyntaxListTranslation<T, TS> Get<T, TS>(this SeparatedSyntaxList<T> syntaxList, SyntaxTranslation parent) where T : SyntaxNode where TS : SyntaxTranslation
        {
            var newList = new SeparatedSyntaxListTranslation<T, TS>(syntaxList, parent);
            return newList;
        }

        public static TokenTranslation Get(this SyntaxToken token, SyntaxTranslation parent)
        {
            var newToken = new TokenTranslation(token, parent);
            return newToken;
        }
    }
}
