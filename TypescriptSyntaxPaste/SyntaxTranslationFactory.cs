using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace RoslynTypeScript.Translation
{
    public static class TF
    {
        private static Dictionary<Type, Type> _mapType = new Dictionary<Type, Type>();

        static TF()
        {
        }

        public static T Get<T>(SyntaxNode syntaxNode, SyntaxTranslation parent) where T : SyntaxTranslation
        {
            return (T)Get(syntaxNode, parent);
        }

        public static T VirtualGet<T>(string code) where T : SyntaxTranslation, new()
        {
            return new T() { SyntaxString = code };
        }

        public static SyntaxTranslation Get(SyntaxNode node, SyntaxTranslation parent)
        {
            Type type = node.GetType();
            Type newType = FindMatchedType(type);

            SyntaxTranslation translation = (SyntaxTranslation)Activator.CreateInstance(newType, node, parent);
            return translation;
        }

        private static Type FindMatchedType(Type type)
        {
            if(_mapType.ContainsKey(type))
            {
                return _mapType[type];
            }
            Assembly assembly = typeof(TF).Assembly;
            var newType = assembly.GetType(GetTranslationName(type.Name));
            if (newType == null)
            {
                return typeof(GenericTranslation);
            }

            return newType;
        }


        private static string GetTranslationName(string syntaxNodeName)
        {
            string name = syntaxNodeName.Remove(syntaxNodeName.Length - 6);
            return "RoslynTypeScript.Translation." + name + "Translation";
        }

        public static void RegisterType<TSyntax,TTransaltion>()
            where TSyntax :SyntaxNode
            where TTransaltion :SyntaxTranslation
        {
            _mapType[typeof(TSyntax)] = typeof(TTransaltion);
        }
    }
}
