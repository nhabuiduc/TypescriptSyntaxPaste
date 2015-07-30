using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoslynTypeScript.Translation;

namespace RoslynTypeScript.Patch
{
    public class LabeledStatementBeforeGotoPatch : Patch
    {
        private static int increment = 0;
        public void Apply(LabeledStatementTranslation labeledStatement)
        {
            // we can only replace in method scope
            BaseMethodDeclarationTranslation method = labeledStatement.GetAncestor<BaseMethodDeclarationTranslation>();
            if (method == null)
            {
                return;
            }

            var currentLabelName = labeledStatement.Syntax.Identifier.ToString();
            var root = labeledStatement.GetRootTranslation();
            var founds = root.GotoLabeledStatements
                .Where(f => f.Expression.ToString() == currentLabelName)
                .Where(f => method.Syntax.Span.Contains(f.Syntax.Span))
                .ToList();
            bool isLabelBeforeAllGoto = founds.All(f => f.Syntax.Span.Start > labeledStatement.Syntax.Span.Start);
            if (!isLabelBeforeAllGoto)
            {
                return;
            }


            var list = labeledStatement.Parent as SyntaxListBaseTranslation;
            var idx = list.SyntaxCollection.IndexOf(labeledStatement);
            int max = idx;

            foreach (var item in founds)
            {
                var found = list.SyntaxCollection.FirstOrDefault(f => f.Syntax.Span.Contains(item.Syntax.Span));
                var foundIdx = list.SyntaxCollection.IndexOf(found);
                max = Math.Max(idx, foundIdx);
            }

            string prefix = $@"{labeledStatement.Syntax.Identifier.ToString()}:
                while(true)
                {{";
            string posfix = @"break; 
                        }";

            var foundStatement = list.SyntaxCollection[max];
            labeledStatement.IgnoreLabel = true;
            if (labeledStatement.Syntax.Span.Contains(foundStatement.Syntax.Span))
            {
                //labeledStatement.SyntaxString = $"{prefix} \r\n {labeledStatement.Statement.Translate()} {posfix}";
                labeledStatement.Prefix = $"{prefix} \r\n " + labeledStatement.Prefix;
                labeledStatement.Suffix += $" {posfix}"; ;
            }
            else
            {
                labeledStatement.Prefix = $"{prefix} \r\n " + labeledStatement.Prefix;
                foundStatement.Suffix += $" {posfix}";
                //labeledStatement.SyntaxString = $"{prefix} \r\n {labeledStatement.Statement.Translate()}";
                //foundStatement.SyntaxString = $"{foundStatement.Translate()} {posfix}";
            }

            HandleContinueInOuterLoop(labeledStatement, idx, max, list);
        }

        private void HandleContinueInOuterLoop(LabeledStatementTranslation labeledStatement, int from, int to, SyntaxListBaseTranslation syntaxList)
        {
            var found = labeledStatement.TravelUpNotMe(f =>
          f is WhileStatementTranslation
          || f is ForStatementTranslation
          || f is DoStatementTranslation);

            if (found == null)
            {
                return;
            }

            var root = labeledStatement.GetRootTranslation();

            var continueStatements = new List<ContinueStatementTranslation>();
            for (int i = from; i <= to; i++)
            {
                var syntax = syntaxList.SyntaxCollection[i];
                continueStatements.AddRange(root.ContinueStatements.Where(f => syntax.Syntax.Span.Contains(f.Syntax.Span)));
            }

            Random rd = new Random(DateTime.Now.Millisecond + increment++);
            var label = $"__Outer{rd.Next(100)}";
            if (found.Prefix != null && found.Prefix.EndsWith(":"))
            {
                label = found.Prefix.Substring(0, found.Prefix.Length - 1);
            }
            else
            {
                found.Prefix = $"{label}:" + found.Prefix;
            }


            foreach (var item in continueStatements)
            {
                item.SyntaxString = $"continue {label};";
            }
        }
    }
}
