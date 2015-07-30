using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Contract;

namespace RoslynTypeScript.Translation
{
    public class SyntaxListTranslation<T,ST> : SyntaxListBaseTranslation<T, ST> where T : SyntaxNode where ST :SyntaxTranslation
    {
        private SyntaxList<T> syntaxList;

        public SyntaxListTranslation()
        {
            SyntaxCollection = new List<SyntaxTranslation>();
        }

        public SyntaxListTranslation(SyntaxList<T> syntaxList, SyntaxTranslation parent) :base(parent)
        {
            this.syntaxList = syntaxList;
            this.Parent = parent;
            SyntaxCollection = syntaxList.Select(f => f.Get<SyntaxTranslation>(this)).ToList();
        }

        

        protected override string InnerTranslate()
        {
            return string.Join("\r\n", SyntaxCollection.Select(f => f.Translate()));
        }
        
    }
}

