using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTypeScript.Contract;
using RoslynTypeScript.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public class MethodDeclarationTranslation : BaseMethodDeclarationTranslation, ITypeParameterConstraint
    {
        public new MethodDeclarationSyntax Syntax
        {
            get { return (MethodDeclarationSyntax)base.Syntax; }
            set { base.Syntax = value; }
        }


        public MethodDeclarationTranslation()
        { }

        public MethodDeclarationTranslation(MethodDeclarationSyntax syntax, SyntaxTranslation parent) : base(syntax, parent)
        {
            ReturnType = syntax.ReturnType.Get<TypeTranslation>(this);
            SemicolonToken = syntax.SemicolonToken.Get(this);
            Identifier = syntax.Identifier.Get(this);
            TypeParameterList = syntax.TypeParameterList.Get<TypeParameterListTranslation>(this);
            ConstraintClauses = syntax.ConstraintClauses.Get<TypeParameterConstraintClauseSyntax, TypeParameterConstraintClauseTranslation>(this);
            
        }

        //public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; }
        public TypeTranslation ReturnType { get; set; }

        public TypeParameterListTranslation TypeParameterList { get; set; }

        public SyntaxListTranslation<TypeParameterConstraintClauseSyntax, TypeParameterConstraintClauseTranslation> ConstraintClauses { get; set; }

        public bool IsOverloadedDeclaration { get; set; }

        public bool IsYieldReturn { get; set; }

        public override void ApplyPatch()
        {
            base.ApplyPatch();

            var arrayInitReturnForYieldPatch = new ArrayInitReturnForYieldPatch();
            arrayInitReturnForYieldPatch.Apply(this);
            // what if this methid is static, and this class/struct is generic?
            // we must move this generic to static method

            if (!Modifiers.IsStatic)
            {
                return;
            }

            var clssStr = GetAncestor<ClassStructDeclarationTranslation>();
            if(clssStr == null || clssStr.TypeParameterList == null)
            {
                return;
            }

            if(this.TypeParameterList != null)
            {
                return;
            }

            this.TypeParameterList = new TypeParameterListTranslation() {
                Parent = this,
                SyntaxString = clssStr.TypeParameterList.Translate()
            };

        }

        protected override string InnerTranslate()
        {

            if (this.Syntax?.Identifier.ToString() == "GetDebuggerDisplay")
            {
                return "";
            }

            if (SemicolonToken == null || SemicolonToken.IsEmpty)
            {
                return string.Format(@"{0} {1} {2}: {3}
{4}",
            Modifiers.Translate(),
              Identifier.Translate() + TypeParameterList?.Translate() ?? "",
             ParameterList.Translate(),
              ReturnType.Translate(),
              Body?.Translate() ?? ";");
            }

            string appendStr = ";";
            var found = (TypeDeclarationTranslation)TravelUpNotMe(f => f is TypeDeclarationTranslation);
            if(found is ClassDeclarationTranslation && found.Modifiers.IsAbstract && !this.IsOverloadedDeclaration)
            {
                appendStr = "{ throw new Error('not implemented'); }";
            }

            return $@"{GetAttributeList()}{Modifiers.Translate()} {Identifier.Translate()}{TypeParameterList?.Translate() ?? ""} {ParameterList.Translate()} : {ReturnType.Translate()}{appendStr} ";
        }


    }
}
