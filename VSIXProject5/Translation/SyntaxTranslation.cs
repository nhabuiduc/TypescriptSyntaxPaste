using Microsoft.CodeAnalysis;
using RoslynTypeScript.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RoslynTypeScript.Translation
{
    [DebuggerDisplay("Syntax = {Syntax}")]
    public abstract class SyntaxTranslation
    {
        private const string ParentName = "Parent";

        private SyntaxTranslation root;

        public SyntaxTranslation() { }

        public SyntaxTranslation(SyntaxNode syntax, SyntaxTranslation parent)
        {
            this.Syntax = syntax;
            this.Parent = parent;
        }

        public SyntaxNode Syntax { get; set; }

        public SyntaxTranslation Parent { get; set; }
        public string SyntaxString { get; set; }

        public string Prefix { get; set; }
        public string Suffix { get; set; }

        public SemanticModel AssignedSemanticModel { get; set; }

        public SemanticModel CachedSemanticModel { get; set; }

        public SemanticModel SemanticModel { get; set; }

        public Compilation Compilation { get; set; }

        public virtual string Translate()
        {           

            var result = string.Empty;

            if (SyntaxString != null)
            {
                result = $"{SyntaxString}";
            }
            else
            {
                result = $"{Prefix}{InnerTranslate()}{Suffix}";
            }

            return $"{ result.Trim('\r', '\n')}";
        }

        public override string ToString()
        {
            return Translate();
        }

        public Compilation GetCompilation()
        {

            if (Compilation != null)
            {
                return Compilation;
            }

            if (Parent != null)
            {
                Compilation = Parent.GetCompilation();
            }

            return Compilation;
        }

        public SemanticModel GetSemanticModel()
        {
            if (AssignedSemanticModel != null)
            {
                return AssignedSemanticModel;
            }

            if (SemanticModel != null)
            {
                return SemanticModel;
            }

            if (Parent != null)
            {
                SemanticModel = Parent.GetSemanticModel();
            }

            return SemanticModel;
        }

        public bool IsInExpression()
        {
            return !(Parent is ExpressionStatementTranslation);
        }

        public virtual void VisitBy(ITranslationVisitor visitor)
        {
            visitor.Visit(this);
            foreach (var prop in GetChildrenTranslation())
            {
                prop.VisitBy(visitor);
            }
        }

        public IEnumerable<T> Decendants<T>() where T : SyntaxTranslation
        {
            var visitor = new GetDecendantsVistor<T>();
            this.VisitBy(visitor);
            return visitor.Founds;
        }

        public void Attach()
        {
            foreach (var prop in GetChildrenTranslation())
            {
                prop.Parent = this;
            }

        }

        public SyntaxTranslation TravelUp(Func<SyntaxTranslation, bool> func)
        {
            SyntaxTranslation translation = this;
            while (translation != null)
            {
                if (func(translation))
                {
                    return translation;
                }
                translation = translation.Parent;
            }

            return null;
        }

        public SyntaxTranslation TravelUpNotMe(Func<SyntaxTranslation, bool> func)
        {
            SyntaxTranslation translation = this.Parent;
            while (translation != null)
            {
                if (func(translation))
                {
                    return translation;
                }
                translation = translation.Parent;
            }

            return null;
        }

        public T GetAncestor<T>() where T : SyntaxTranslation
        {
            var found = TravelUp(f => f is T);
            return (T)found;
        }

        public bool IsInScope<T>()
        {
            var found = TravelUp(f => f != this && f is T);
            return found != null;
        }

        public virtual void ApplyPatch()
        {
            foreach (var item in GetChildrenTranslation())
            {
                item.ApplyPatch();
            }
        }

        public virtual void ReplaceTranslation(SyntaxTranslation original, SyntaxTranslation newOne)
        {
            Type type = this.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in properties)
            {
                if (item.Name != ParentName && item.PropertyType.IsAssignableFrom(original.GetType()))
                {
                    SyntaxTranslation prop = (SyntaxTranslation)item.GetValue(this);
                    if (prop == original)
                    {
                        item.SetValue(this, newOne);
                        newOne.Parent = this;
                    }

                }
            }
        }

        protected abstract string InnerTranslate();

        private IEnumerable<SyntaxTranslation> GetChildrenTranslation()
        {
            Type type = this.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in properties)
            {
                if (item.Name != ParentName && typeof(SyntaxTranslation).IsAssignableFrom(item.PropertyType))
                {
                    SyntaxTranslation prop = (SyntaxTranslation)item.GetValue(this);
                    if (prop != null)
                    {
                        yield return prop;
                    }

                }
            }
        }
        public SyntaxTranslation GetRootTranslation()
        {
            if (root != null)
            {
                return root;
            }

            SyntaxTranslation translation = this;
            while (translation.Parent != null)
            {
                translation = translation.Parent;

            }

            root = translation;
            return root;
        }

    }

    public class GetDecendantsVistor<T> : ITranslationVisitor where T : SyntaxTranslation
    {
        public List<T> Founds { get; set; } = new List<T>();
        public void Visit(SyntaxTranslation translation)
        {
            if (translation is T)
            {
                Founds.Add((T)translation);
            }
        }
    }
}
