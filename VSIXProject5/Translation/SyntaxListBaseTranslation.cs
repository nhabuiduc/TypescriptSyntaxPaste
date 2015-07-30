using Microsoft.CodeAnalysis;
using RoslynTypeScript.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTypeScript.Translation
{
    public abstract class SyntaxListBaseTranslation : CSharpSyntaxTranslation
    {
        public SyntaxListBaseTranslation()
        { }

        public SyntaxListBaseTranslation(SyntaxTranslation parent) : base(null, parent)
        {

        }

        public List<SyntaxTranslation> SyntaxCollection { get; set; }

        public void Remove(IEnumerable<SyntaxTranslation> collection)
        {
            foreach (var item in collection)
            {
                SyntaxCollection.Remove(item);
            }
        }

        public void Add(IEnumerable<SyntaxTranslation> collection)
        {
            foreach (var item in collection)
            {
                item.Parent = this;
                SyntaxCollection.Add(item);
            }
        }

        public void Add(SyntaxTranslation translation)
        {
            Add(new[] { translation });
        }

        public void Remove(SyntaxTranslation translation)
        {
            Remove(new[] { translation });
        }

        public void Insert(int position,SyntaxTranslation translation)
        {
            SyntaxCollection.Insert(position, translation);
            translation.Parent = this;
        }

        public override void ReplaceTranslation(SyntaxTranslation original, SyntaxTranslation newOne)
        {
            var index = SyntaxCollection.IndexOf(original);
            SyntaxCollection[index] = newOne;
            newOne.Parent = this;
        }

        public override void ApplyPatch()
        {
            foreach (var item in SyntaxCollection.ToArray())
            {
                item.ApplyPatch();
            }
        }

        public override void VisitBy(ITranslationVisitor visitor)
        {
            base.VisitBy(visitor);
            foreach (var item in SyntaxCollection)
            {
                item.VisitBy(visitor);
            }
        }

        public void Clear()
        {
            SyntaxCollection.Clear();
        }
    }
    public abstract class SyntaxListBaseTranslation<T, ST> : SyntaxListBaseTranslation where T : SyntaxNode where ST : SyntaxTranslation
    {

        public SyntaxListBaseTranslation()
        { }

        public SyntaxListBaseTranslation(SyntaxTranslation parent) : base( parent)
        {

        }

       

        public IEnumerable<ST> GetEnumerable()
        {
            return SyntaxCollection.Select(f => (ST)f).ToArray();
        }

        public IEnumerable<TT> GetEnumerable<TT>() where TT : SyntaxTranslation
        {
            return SyntaxCollection.Where(f => f is TT).Select(f => (TT)f).ToArray();
        }

  
    }
}
