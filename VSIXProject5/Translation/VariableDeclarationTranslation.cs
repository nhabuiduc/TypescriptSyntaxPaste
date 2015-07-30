using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class VariableDeclarationTranslation : CSharpSyntaxTranslation
    {
        public new VariableDeclarationSyntax Syntax
        {
            get { return (VariableDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }
        public TypeTranslation Type { get; set; }
        public SeparatedSyntaxListTranslation<VariableDeclaratorSyntax, VariableDeclaratorTranslation> Variables { get; set; }


        public VariableDeclarationTranslation(VariableDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Type = syntax.Type.Get<TypeTranslation>(this);
            Variables = syntax.Variables.Get<VariableDeclaratorSyntax, VariableDeclaratorTranslation>(this);
            if (!syntax.Type.IsVar)
            {
                Variables.GetEnumerable().First().FirstType = Type;
            }

            foreach (var item in Variables.GetEnumerable())
            {
                item.KnownType = Type;
            }
        }

        public bool ExcludeVar { get; set; }

        //private bool isIgnoreInitialize;
        //public bool IsIgnoreInitialize
        //{
        //    get { return isIgnoreInitialize; }
        //    set {
        //        isIgnoreInitialize = value;
        //        foreach (var item in Variables.GetEnumerable())
        //        {                    
        //            item.IsIgnoreInitialize = value;
        //        }
        //    }
        //}

        protected override string InnerTranslate()
        {
            if (ExcludeVar)
                return string.Format("{0}", Variables.Translate());
            return string.Format("var {0}", Variables.Translate());
        }
    }
}
