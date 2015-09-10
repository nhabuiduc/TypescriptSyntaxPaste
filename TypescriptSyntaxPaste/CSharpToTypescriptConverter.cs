using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypescriptSyntaxPaste
{
    public class CSharpToTypescriptConverter
    {
        private MetadataReference mscorlib;
        private MetadataReference Mscorlib
        {
            get
            {
                if (mscorlib == null)
                {
                    mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                }

                return mscorlib;
            }
        }

        public string ConvertToTypescript(string text, bool convertClssToInterface)
        {
            try
            {
                var tree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(text);

                // detect to see if it's actually C# sourcode by checking whether it has any error
                if (tree.GetDiagnostics().Any(f => f.Severity == DiagnosticSeverity.Error))
                {
                    return null;
                }

                var root = tree.GetRoot();

                if (convertClssToInterface)
                {
                    root = ClassToInterfaceReplacement.ReplaceClass(root);
                }

                tree = (CSharpSyntaxTree)root.SyntaxTree;

                var translationNode = TF.Get(root, null);

                var compilation = CSharpCompilation.Create("TemporaryCompilation",
                     syntaxTrees: new[] { tree }, references: new[] { Mscorlib });
                var model = compilation.GetSemanticModel(tree);

                translationNode.Compilation = compilation;
                translationNode.SemanticModel = model;

                translationNode.ApplyPatch();
                return translationNode.Translate();

            }
            catch (Exception ex)
            {
                // TODO
                // swallow exception .!!!!!!!!!!!!!!!!!!!!!!!
            }

            return null;
        }
    }
}
