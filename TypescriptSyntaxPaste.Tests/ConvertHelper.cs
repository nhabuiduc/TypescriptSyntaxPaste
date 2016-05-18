using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypescriptSyntaxPaste.Tests
{
    public static class ConvertHelper
    {
        public static string ConvertToTypescript(string csharpCode, IEnumerable< Func<CSharpSyntaxNode,CSharpSyntaxNode>> funcs = null)
        {
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var tree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(csharpCode);

            if (tree.GetDiagnostics().Any(f => f.Severity == DiagnosticSeverity.Error))
            {
                return null;
            }

            var root = tree.GetRoot();

            if (funcs != null)
            {
                foreach (var func in funcs)
                {
                    root = func(root);
                }
                
            }

            var translationNode = TF.Get(root, null);

            var compilation = CSharpCompilation.Create("TemporaryCompilation",
                 syntaxTrees: new[] { tree }, references: new[] { mscorlib });
            var model = compilation.GetSemanticModel(tree);

            translationNode.Compilation = compilation;
            translationNode.SemanticModel = model;

            translationNode.ApplyPatch();
            return translationNode.Translate();
        }

        public static void AssertConvertingIgnoreSpaces(string csharpCode, string typescriptCode,IEnumerable< Func<CSharpSyntaxNode, CSharpSyntaxNode>> funcs = null)
        {
            var stripTypescriptCode = StripAllSpaces(typescriptCode);
            var converted = ConvertToTypescript(csharpCode, funcs);

            Assert.AreEqual(stripTypescriptCode, StripAllSpaces(converted));
        }

        private static string StripAllSpaces(string code)
        {
            if (code == null)
            {
                return string.Empty;
            }

            return code.Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }
    }
}
