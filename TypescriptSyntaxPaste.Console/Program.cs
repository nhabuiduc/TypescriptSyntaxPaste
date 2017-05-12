using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypescriptSyntaxPaste.VSIX;

namespace TypescriptSyntaxPaste.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var cScript = @"class Foo{}";
            CSharpToTypescriptConverter csharpToTypescriptConverter = new CSharpToTypescriptConverter();
            var typescript = csharpToTypescriptConverter.ConvertToTypescript(cScript, new MySettingStore());

            System.Console.WriteLine(typescript);
        }
    }


    class MySettingStore : ISettingStore
    {
        public bool AddIPrefixInterfaceDeclaration
        {
            get
            {
                return true;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsConvertListToArray
        {
            get
            {
                return true;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsConvertMemberToCamelCase
        {
            get
            {
               return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsConvertToInterface
        {
            get
            {
                return true;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInterfaceOptionalProperties
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public TypeNameReplacementData[] ReplacedTypeNameArray
        {
            get
            {
                return new TypeNameReplacementData[0];
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
