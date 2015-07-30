using Microsoft.CodeAnalysis;
using RoslynTypeScript.Constants;
using RoslynTypeScript.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace RoslynTypeScript
{
    public static class Helper
    {    
        public static string GetAttributeList(SyntaxList<AttributeListSyntax> attributeList)
        {
            var attr = attributeList.ToString();
            return string.IsNullOrWhiteSpace(attr) ? string.Empty : "/*" + attr + "*/" + Environment.NewLine;
        }
        public static string NormalizeVariabeleName(string name)
        {
            if (name.StartsWith("@"))
            {
                name = "$" + name.Substring(1);
            }

            return name;
        }
        public static bool IsSupportOperator(string operatorStr)
        {
            switch (operatorStr)
            {
                case OperatorConstants.OperatorEquals:
                case OperatorConstants.OperatorNotEqual:
                case OperatorConstants.OperatorGreaterThan:
                case OperatorConstants.OperatorLessThan:
                case OperatorConstants.OperatorGreaterThanOrEqual:
                case OperatorConstants.OperatorLessThanOrEqual:
                    return true;
            }
            return false;
        }

        public static string GetNewLineIfExist(string trivia)
        {
            return trivia.Contains(Environment.NewLine) ? Environment.NewLine : "";
        }

        public static string GetNewLineIfExist(SyntaxTriviaList triviaList)
        {
            return triviaList.Any(f=>f.IsKind(SyntaxKind.EndOfLineTrivia)) ? Environment.NewLine : "";
        }

        public static bool IsInKinds(SyntaxNode node, params SyntaxKind[] kinds)
        {
            return kinds.Any(f => node.IsKind(f));
        }

        public static string OperatorToMethod(string operatorStr)
        {
            switch (operatorStr)
            {
                case OperatorConstants.OperatorEquals: return OperatorConstants.MethodEquals;
                case OperatorConstants.OperatorNotEqual: return OperatorConstants.MethodNotEqual;
                case OperatorConstants.OperatorGreaterThan: return OperatorConstants.MethodGreaterThan;
                case OperatorConstants.OperatorLessThan: return OperatorConstants.MethodLessThan;
                case OperatorConstants.OperatorGreaterThanOrEqual: return OperatorConstants.MethodGreaterThanOrEqual;
                case OperatorConstants.OperatorLessThanOrEqual: return OperatorConstants.MethodLessThanOrEqual;
            }
            throw new NotSupportedException();
        }

        public static bool IsInteger(SpecialType specialType)
        {
            return specialType == SpecialType.System_Int32
                    || specialType == SpecialType.System_Int64
                    || specialType == SpecialType.System_Int16
                    || specialType == SpecialType.System_Byte
                    || specialType == SpecialType.System_UInt16
                    || specialType == SpecialType.System_UInt32
                    || specialType == SpecialType.System_UInt64;                    
        }

        public static bool IsUnsignedInterger(SpecialType specialType)
        {
            return                     
                    specialType == SpecialType.System_UInt16
                    || specialType == SpecialType.System_UInt32
                    || specialType == SpecialType.System_UInt64;
        }

        public static bool IsNumber(SpecialType specialType)
        {
            if (IsInteger(specialType))
            {
                return true;
            }

            switch (specialType)
            {
                case SpecialType.System_Double:
                case SpecialType.System_Decimal:
                case SpecialType.System_Single:
                    return true;
            }
            return false;
        }

        public static string GetTSValueTypeToCheck(TypeTranslation type, ITypeSymbol typeSymbol)
        {

            if (Helper.IsNumber(typeSymbol.SpecialType))
            {
                return "Number";
            }

            if (typeSymbol.SpecialType == SpecialType.System_Char)
            {
                return "TSChar";
            }

            if (typeSymbol.SpecialType == SpecialType.System_Boolean)
            {
                return "Boolean";
            }

            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                return "Number";
            }

            if (typeSymbol.IsValueType)
            {
                return type.Translate();

            }

            return null;
        }

        public static bool IsDefinedStruct(ITypeSymbol typeSymbol)
        {
            if (typeSymbol == null)
            {
                return false;
            }

            if (typeSymbol.IsReferenceType)
            {
                return false;
            }

            if (Helper.IsNumber(typeSymbol.SpecialType))
            {
                return false;
            }

            if (typeSymbol.SpecialType == SpecialType.System_Char)
            {
                return false;
            }

            if (typeSymbol.SpecialType == SpecialType.System_Boolean)
            {
                return false;
            }

            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                return false;
            }

            if (typeSymbol.IsValueType)
            {
                return true;

            }

            return false;

        }
        public static string GetDefaultValue(TypeTranslation type)
        {
            if (type.Syntax.IsKind(SyntaxKind.BoolKeyword))
            {
                return "false";
            }

            if (type.Syntax.IsKind(SyntaxKind.ObjectKeyword))
            {
                return "null";
            }

            if (type.Syntax.IsKind(SyntaxKind.IntKeyword)
                || type.Syntax.IsKind(SyntaxKind.UIntKeyword)
                || type.Syntax.IsKind(SyntaxKind.FloatKeyword)
                || type.Syntax.IsKind(SyntaxKind.DecimalKeyword)
                || type.Syntax.IsKind(SyntaxKind.DoubleKeyword)
                || type.Syntax.IsKind(SyntaxKind.ByteKeyword)
                 || type.Syntax.IsKind(SyntaxKind.LongKeyword)
                 || type.Syntax.IsKind(SyntaxKind.ULongKeyword)
                )
            {
                return "0";
            }

            return "null";
        }

        public static string GetDefaultValue(TypeTranslation type, ITypeSymbol typeSymbol)
        {

            if (Helper.IsNumber(typeSymbol.SpecialType))
            {
                return "0";
            }

            if (typeSymbol.SpecialType == SpecialType.System_Char)
            {
                return "''";
            }

            if (typeSymbol.SpecialType == SpecialType.System_Boolean)
            {
                return "false";
            }

            if (typeSymbol.SpecialType == SpecialType.System_String)
            {
                return "null";
            }

            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                return "0";
            }

            if (typeSymbol.IsValueType)
            {
                if (type is GenericNameTranslation)
                {
                    return $"<{type.Translate()}> structDefault({type.GetTypeIgnoreGeneric()})";
                }
                else
                {
                    return $"structDefault({type.GetTypeIgnoreGeneric()})";
                }


            }

            return "null";
        }

        public static string TranslateTypeFromSymbols(IEnumerable<ITypeSymbol> types, string ns)
        {
            var join = string.Join(",", types.Select(f => TranslateTypeFromSymbol(f, ns)));
            return $"<{join}>";
        }

        public static string TranslateConvernsion(Conversion conversion, NamespaceDeclarationTranslation ns, string innerTranslation)
        {
            var name = Helper.GetUniqueHashName(conversion.MethodSymbol.OriginalDefinition);
            var clss = Helper.GetClassOfMethod(conversion.MethodSymbol.OriginalDefinition);
            var generic = string.Empty;
            INamedTypeSymbol returnType = conversion.MethodSymbol.ReturnType as INamedTypeSymbol;

            if (returnType.IsGenericType)
            {
                generic = Helper.TranslateTypeFromSymbols(returnType.TypeArguments, ns?.Name.Translate());
            }


            var fullInvoke = $"{clss}.{name}";
            if (ns != null)
            {
                fullInvoke = Helper.GetReduceNameWithNamespace(fullInvoke, ns.Name.Translate());
            }
            fullInvoke = $"{fullInvoke}{generic}";

            return $"{fullInvoke}({innerTranslation})";
        }

        public static string TranslateTypeFromSymbol(ITypeSymbol typeSymbol, string ns)
        {

            if (Helper.IsNumber(typeSymbol.SpecialType))
            {
                return "number";
            }
            if (typeSymbol.SpecialType == SpecialType.System_Boolean)
            {
                return "boolean";
            }
            if (typeSymbol.SpecialType == SpecialType.System_String)
            {
                return "string";
            }
            if (typeSymbol.SpecialType == SpecialType.System_Array)
            {
                return "Array";
            }

            if (typeSymbol.SpecialType == SpecialType.System_DateTime)
            {
                return "Date";
            }
            //var result =  typeSymbol.ToFullNameWithoutGeneric();
            var nameType = typeSymbol as INamedTypeSymbol;
            if (nameType == null || !nameType.IsGenericType)
            {
                return typeSymbol.ToFullNameGeneric();
            }

            var fullName = typeSymbol.ToFullNameWithoutGeneric();
            fullName = GetReduceNameWithNamespace(fullName, ns);

            return $"{fullName}<{TranslateTypeFromSymbols(nameType.TypeArguments, ns)}> ";

        }

        public static string GetReduceNameWithNamespace(string fullName, string ns)
        {
            if (string.IsNullOrEmpty(ns))
            {
                return fullName;
            }
            var split = fullName.Split('.');
            string namePrefix = string.Join(".", split.Take(split.Length - 1));
            var common = FindCommonPrefix(namePrefix, ns);
            if (string.IsNullOrWhiteSpace(common))
            {
                return split.Last();
            }
            return $"{common}.{split.Last()}";
        }
        public static string GetPrefix(SyntaxTranslation syntax, SemanticModel semanticModel = null)
        {
            if (semanticModel == null)
            {
                semanticModel = syntax.GetSemanticModel();
            }
            var symbolInfo = semanticModel.GetSymbolInfo(syntax.Syntax);
            string foundNsName = FindScopeName(syntax, semanticModel);


            if (foundNsName != null)
            {
                string namespaceName = Helper.GetPrefixName(symbolInfo.Symbol);
                string common = Helper.FindCommonPrefix(namespaceName, foundNsName);

                string dot = string.IsNullOrEmpty(common) ? "" : ".";
                return $"{common}{dot}";
            }
            return null;
        }

        public static string FindScopeName(SyntaxTranslation syntax, SemanticModel semanticModel = null)
        {
            if (semanticModel == null)
            {
                semanticModel = syntax.GetSemanticModel();
            }

            var symbolInfo = semanticModel.GetSymbolInfo(syntax.Syntax);
            string foundNsName = null;
            var foundClssOrStr = (TypeDeclarationTranslation)syntax.TravelUpNotMe(f => f is ClassDeclarationTranslation || f is StructDeclarationTranslation);
            if (foundClssOrStr != null)
            {
                var clssSemanticModel = foundClssOrStr.GetSemanticModel();
                var clssOrStrSymbol = clssSemanticModel.GetDeclaredSymbol(foundClssOrStr.Syntax);
                if (clssOrStrSymbol != null)
                {
                    foundNsName = Helper.GetPrefixName(clssOrStrSymbol);
                }

            }

            if (foundNsName == null)
            {
                if (symbolInfo.Symbol.Kind == SymbolKind.Namespace)
                {
                    return null;
                }
                var foundNs = (NamespaceDeclarationTranslation)syntax.TravelUpNotMe(f => f is NamespaceDeclarationTranslation);
                foundNsName = foundNs.Name.Translate();
            }

            return foundNsName;
        }

        public static string GetPrefixName(ISymbol symbol)
        {
            SymbolDisplayFormat format = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.None);
            if (symbol.ContainingType != null)
            {
                return symbol.ContainingType.ToDisplayString(format);
            }
            if (symbol.ContainingNamespace != null)
            {
                return symbol.ContainingNamespace.ToDisplayString(format);
            }

            return null;
        }

        public static string FindCommonPrefix(string typePrefix, string scope)
        {
            var typePrefixSplit = typePrefix.Split('.');
            var containingPrefixSplit = scope.Split('.');
            int length = Math.Min(typePrefixSplit.Length, containingPrefixSplit.Length);
            List<string> result = new List<string>();
            int i = 0;
            for (i = 0; i < length; i++)
            {
                if (typePrefixSplit[i] != containingPrefixSplit[i])
                {
                    break;
                }
            }

            // what if the last part of contating namespace match with first part of prefix?
            if(i<typePrefixSplit.Length && containingPrefixSplit.Last() == typePrefixSplit[i])
            {
                i--;
            }

            for (; i < typePrefixSplit.Length; i++)
            {
                result.Add(typePrefixSplit[i]);
            }

            return string.Join(".", result);
        }

        public static string ApplyThis(SemanticModel semanticModel, SyntaxTranslation syntaxTranslation, string property)
        {
            SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(syntaxTranslation.Syntax);
            var found = (TypeDeclarationTranslation)syntaxTranslation.TravelUpNotMe(f => f is TypeDeclarationTranslation);
            if (found != null)
            {
                var classSymbol = semanticModel.GetDeclaredSymbol(found.Syntax) as INamedTypeSymbol;
                if (classSymbol != null && IsEqualOrBaseOf(symbolInfo.Symbol.ContainingType, classSymbol))
                {
                    string prefixName = symbolInfo.Symbol.IsStatic ? classSymbol.Name : "this";
                    return $"{prefixName}.{property}";
                }
            }
            return null;
        }

        public static SymbolDisplayFormat ConvertHashDisplayFormat =
             new SymbolDisplayFormat(
                    genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                    typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly,
                    memberOptions: SymbolDisplayMemberOptions.IncludeParameters | SymbolDisplayMemberOptions.IncludeType,
                    parameterOptions: SymbolDisplayParameterOptions.IncludeType);
        public static string GetUniqueHashName(IMethodSymbol methodSymbol)
        {
            var str = methodSymbol.ToDisplayString(ConvertHashDisplayFormat);
            string name = methodSymbol.Name;
            if (name == ".ctor")
            {
                SymbolDisplayFormat format = new SymbolDisplayFormat(SymbolDisplayGlobalNamespaceStyle.Omitted,
                    SymbolDisplayTypeQualificationStyle.NameOnly, SymbolDisplayGenericsOptions.None,
                    SymbolDisplayMemberOptions.None, parameterOptions: SymbolDisplayParameterOptions.None);

                var clssName = methodSymbol.ToDisplayString(format);

                name = "ctor";
                //if(clssName == "SyntaxTrivia" && )
                //{
                //    name += "_C";
                //}
            }

            var result = $"{name}_{Hash(str)}";
            return result;
        }

        public static bool HasSource(this ISymbol symbol)
        {
            return symbol.Locations.Where(f => f.IsInSource).Any();
        }

        public static string GetUnitHashNameIfRequire(IMethodSymbol methodSymbol)
        {
            // only consider our defined overload
            if (!HasSource(methodSymbol))
            {
                return methodSymbol.Name;
            }
            // always translate constructor
            if (methodSymbol.Name == ".ctor")
            {
                return GetUniqueHashName(methodSymbol);
            }

            if (methodSymbol.OverriddenMethod != null)
            {
                return GetUnitHashNameIfRequire(methodSymbol.OverriddenMethod);
            }

            // check overloading in the same class
            // ignore interface explicit
            var overloadings = GetOverloading(methodSymbol);
            if (overloadings.Length > 1)
            {
                return GetUniqueHashName(methodSymbol);
            }

            return methodSymbol.Name;
        }

        public static ISymbol[] GetMemberInInterfaces(this ITypeSymbol symbol, string member)
        {
            List<ISymbol> result = new List<ISymbol>();
            result.AddRange(symbol.GetMembers(member));
            result.AddRange(symbol.AllInterfaces.SelectMany(item => item.GetMembers(member)));

            return result.ToArray();
        }

        private static IMethodSymbol[] GetOverloading(this IMethodSymbol methodSymbol)
        {
            return methodSymbol.ContainingType.GetMembers(methodSymbol.Name).OfType<IMethodSymbol>().ToArray();
        }

        public static string Hash(string str)
        {
            return Math.Abs(str.GetHashCode()).ToString("0000").Substring(0, 4);
        }

        public static T GetNodeFromSymbol<T>(this ISymbol symbol) where T : SyntaxNode
        {
            var sourceTree = symbol.Locations[0].SourceTree;
            return (T)sourceTree.GetRoot().DescendantNodes(symbol.Locations[0].SourceSpan).Last();

        }

        public static string ToFullNameWithoutGeneric(this ISymbol symbol)
        {
            return symbol.ToDisplayString(new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.None));
        }

        public static string ToFullNameGeneric(this ISymbol symbol)
        {
            return symbol.ToDisplayString(new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters));
        }

        public static string GetClassOfMethod(IMethodSymbol methodSymbol)
        {
            var displayFormat = new SymbolDisplayFormat(
                genericsOptions: SymbolDisplayGenericsOptions.None,
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces);
            return methodSymbol.ContainingType.ToDisplayString(displayFormat);
        }

        private static bool IsEqualOrBaseOf(INamedTypeSymbol nameTypeSymbol, INamedTypeSymbol classSymbol)
        {
            var current = classSymbol;
            while (current != null)
            {
                if (current.ToString() == nameTypeSymbol.ToString())
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
        }

        public static string StripTypeParameter(string str)
        {
            var indx = str.IndexOf("<");
            if (indx >= 0)
            {
                return str.Substring(0, indx);
            }

            return str;
        }

        public static bool ApplyFunctionBindToCorrectContext(ExpressionTranslation exp)
        {
            if (exp == null || exp.Syntax == null)
            {
                return false;
            }
            var semanticModel = exp.GetSemanticModel();
            if (semanticModel == null)
            {
                return false;
            }

            var identifierName = exp as IdentifierNameTranslation;
            if (identifierName != null)
            {
              

                var symbol = semanticModel.GetSymbolInfo(exp.Syntax);
                var method = symbol.Symbol as IMethodSymbol;
                if (method == null || method.IsStatic)
                {
                    return false;
                }

                identifierName.MethodNeedToBind = "this";

                return true;
            }

            var memberAccess = exp as MemberAccessExpressionTranslation;
            if (memberAccess != null && memberAccess.Name is IdentifierNameTranslation)
            {                
                var symbol = semanticModel.GetSymbolInfo(memberAccess.Name.Syntax);
                var method = symbol.Symbol as IMethodSymbol;
                if (method == null || method.IsStatic)
                {
                    return false;
                }

                ((IdentifierNameTranslation)memberAccess.Name).MethodNeedToBind = memberAccess.Expression.Translate();
                return true;
            }

            return false;
        }
    }
}
