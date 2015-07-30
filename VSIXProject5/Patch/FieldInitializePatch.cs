using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoslynTypeScript.Translation;

namespace RoslynTypeScript.Patch
{
    /// <summary>
    /// Not using at this moment!!!!
    /// try to find constructor and set default value for field which is number, boolean. Because
    /// in javascript all fields are undefined at the first place.
    /// </summary>
    public class FieldInitializePatch : Patch
    {
        public void Apply(ClassDeclarationTranslation typeTranslation)
        {
            var fields = typeTranslation.Members.GetEnumerable<FieldDeclarationTranslation>();
            if (!fields.Any())
            {
                return;
            }

            ConstructorDeclarationTranslation constructor = FindConstructor(typeTranslation);
            if (constructor == null)
            {
                return;
            }

            if (!typeTranslation.HasExplicitBase())
            {
                return;
            }

            foreach (FieldDeclarationTranslation field in fields)
            {
                if (field.Modifiers.IsStatic)
                {
                    continue;
                }

                AssignmentExpressionTranslation assignment = BuildAssignment(field);
                constructor.Body.Statements.Insert(0, assignment);
            }
        }

        private ConstructorDeclarationTranslation FindConstructor(TypeDeclarationTranslation typeTranslation)
        {
            var constructor = typeTranslation.Members.GetEnumerable<ConstructorDeclarationTranslation>().FirstOrDefault(f => !f.IsDeclarationOverload);
            return constructor;
        }

        private AssignmentExpressionTranslation BuildAssignment(FieldDeclarationTranslation field)
        {
            var declarator = field.Declaration.Variables.GetEnumerable().First();
            var initializeStr = declarator.GetInitializerStr();
            string statement = $"this.{declarator.Identifier.Translate()}{initializeStr};";
            return new AssignmentExpressionTranslation() { SyntaxString = statement };
        }
    }
}
