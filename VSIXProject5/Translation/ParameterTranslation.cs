using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Patch;

namespace RoslynTypeScript.Translation
{
    public class ParameterTranslation : CSharpSyntaxTranslation
    {
        public new ParameterSyntax Syntax
        {
            get { return (ParameterSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }

        public ParameterTranslation()
        {

        }

        public ParameterTranslation(ParameterSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            Type = syntax.Type.Get<TypeTranslation>(this);
            Identifier = syntax.Identifier.Get(this);
            Modifiers = syntax.Modifiers.Get(this);
            Default = syntax.Default.Get<EqualsValueClauseTranslation>(this);
           
        }

        public TypeTranslation Type { get; set; }
        public TokenTranslation Identifier { get; set; }
        public SyntaxTokenListTranslation Modifiers { get; set; }

        public EqualsValueClauseTranslation Default { get; set; }

        public bool ExcludeDefaultValue { get; set; }

        public bool IsOptional { get; set; }

        public bool IsRef()
        {
            if(Syntax == null)
            {
                return false;
            }

            string modifiers = Syntax.Modifiers.ToString();
            return modifiers.Contains("ref") || modifiers.Contains("out");
        }

        public bool IsParam()
        {
            if (Syntax == null)
            {
                return false;
            }

            string modifiers = Syntax.Modifiers.ToString();
            return modifiers.Contains("params");
        }

        public bool HasDefault
        {
            get { return Default != null; }
        }

        public override void ApplyPatch()
        {
            base.ApplyPatch();
        }

        protected override string InnerTranslate()
        {

            string paramStr = IsParam() ? "..." : "";
            string defaultStr = Default?.Translate() ?? string.Empty;
            string optionalStr = IsOptional ? "?" : "";
            if(ExcludeDefaultValue && !string.IsNullOrEmpty(defaultStr))
            {
                defaultStr = $"/*{defaultStr}*/ ";
            }
            string typeStr = string.Empty;
            if (Type != null)
            {
                typeStr = $":{Type.Translate()}";

            }
            return $"{paramStr}{Identifier.Translate()}{optionalStr}{typeStr}{defaultStr}";
        }
    }
}
