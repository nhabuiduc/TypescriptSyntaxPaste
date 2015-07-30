using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Constants;

namespace RoslynTypeScript.Translation
{
    public class BaseListTranslation : CSharpSyntaxTranslation
    {
        public new BaseListSyntax Syntax
        {
            get { return (BaseListSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public BaseListTranslation() { }

        public BaseListTranslation(BaseListSyntax syntax,  SyntaxTranslation parent) : base(syntax, parent)
        {
            Types = syntax.Types.Get<BaseTypeSyntax, BaseTypeTranslation>(this);
        }

        public SeparatedSyntaxListTranslation<BaseTypeSyntax, BaseTypeTranslation> Types { get; set; }

        public bool IsForInterface { get; set; }

        public BaseTypeTranslation GetBaseClass()
        {
            // because there is no semantic model, indicate interface by prefix I
            return Types.GetEnumerable().FirstOrDefault(f => !IsInterface(f.ToString()));
        }

        private IEnumerable<BaseTypeTranslation> GetInterfaces(BaseTypeTranslation baseClass)
        {
            return Types.GetEnumerable().Where(f => IsInterface(f.ToString())).ToArray();
        }

        private bool  IsInterface(string name)
        {
            return name.StartsWith("I") && name.Length > 1 && char.IsUpper(name[1]);
        }

        protected override string InnerTranslate()
        {
            if (IsForInterface)
            {
                if (!Types.GetEnumerable().Any())
                {
                    return string.Empty;
                }
                string join = string.Join(",", Types.GetEnumerable().Select(f => f.Translate()));
                return $"extends {join}";

            }

            var baseType = GetBaseClass();
            string baseExtends = string.Empty;
            if (baseType != null)
            {
                baseExtends = $"extends {baseType.Translate()}";
            }

            string interfaceImplements = string.Empty;
            var interfaces = GetInterfaces(baseType);
            if (interfaces.Any())
            {
                string join = string.Join(",", interfaces.Select(f => f.Translate()));
                interfaceImplements = $"implements {join}";
            }

            return $"{baseExtends} {interfaceImplements}";
        }
    }
}
