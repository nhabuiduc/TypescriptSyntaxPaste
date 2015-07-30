//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RoslynTypeScript.Translation;

//namespace RoslynTypeScript.Patch
//{
//    public class LabeledStatementAfterGotoPatch :Patch
//    {
//        public void Apply(LabeledStatementTranslation labeledStatement)
//        {
//            // we can only replace in method scope
//            BaseMethodDeclarationTranslation method = labeledStatement.GetAncestor<BaseMethodDeclarationTranslation>();
//            if (method == null)
//            {
//                return;
//            }

//            var currentLabelName = labeledStatement.Syntax.Identifier.ToString();
//            var root = labeledStatement.GetRootTranslation();
//            var founds = root.GotoLabeledStatements
//                .Where(f=>f.Expression.ToString() == currentLabelName)
//                  .Where(f => method.Syntax.Span.Contains(f.Syntax.Span))
//                .ToList();
//            bool isLabelBeforeAllGoto = founds.All(f => f.Syntax.Span.Start < labeledStatement.Syntax.Span.Start);
//            if(!isLabelBeforeAllGoto)
//            {
//                return;
//            }

//            if(labeledStatement.TravelUpNotMe(f =>
//            f is WhileStatementTranslation
//            || f is ForStatementTranslation
//            || f is DoStatementTranslation) != null)
//            {
//                throw new Exception();
//            }

//            var list = labeledStatement.Parent as SyntaxListBaseTranslation;
//            var idx = list.SyntaxCollection.IndexOf(labeledStatement);
//            var endStatement = list.SyntaxCollection[idx - 1];
//            labeledStatement.IgnoreLabel = true;
//            // if previous statement is while, and all goto statements are in while, don't need to add wrapped while
//            if(endStatement is WhileStatementTranslation
//                && founds.All(f => endStatement.Syntax.Span.Contains(f.Syntax.Span)))
//            {
//                //endStatement.SyntaxString = $"{labeledStatement.Syntax.Identifier.ToString()}: \r\n {endStatement.Translate()}";
//                endStatement.Prefix = $"{labeledStatement.Syntax.Identifier.ToString()}:\r\n" + endStatement.Prefix;
//                return;
//            }

//            int min = idx;

//            foreach (var item in founds)
//            {
//                var found = list.SyntaxCollection.FirstOrDefault(f => f.Syntax.Span.Contains(item.Syntax.Span));
//                var foundIdx = list.SyntaxCollection.IndexOf(found);
//                min = Math.Min(idx, foundIdx);
//            }

//            string prefix = $@"{labeledStatement.Syntax.Identifier.ToString()}:
//                while(true)
//                {{";
//            string posfix = @"break; 
//                        }";
            
            

//            var foundStatement = list.SyntaxCollection[min];
//            if(endStatement.Syntax.Span.Contains(foundStatement.Syntax.Span))
//            {

//                //endStatement.SyntaxString = $"{prefix} \r\n {endStatement.Translate()} {posfix}";
//                endStatement.Prefix = $"{prefix} \r\n ";
//                endStatement.Suffix = $" {posfix}"; ;
//            }
//            else
//            {
//                foundStatement.Prefix = $"{prefix} \r\n ";
//                endStatement.Suffix = $" {posfix}"; ;
//                //foundStatement.SyntaxString = $"{prefix} \r\n {foundStatement.Translate()}";
//                //endStatement.SyntaxString = $"{endStatement.Translate()} {posfix}";
//            }
                        
//        }
//    }
//}
