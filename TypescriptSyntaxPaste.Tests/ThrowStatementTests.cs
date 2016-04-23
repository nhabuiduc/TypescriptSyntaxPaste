using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypescriptSyntaxPaste.Tests
{
    [TestClass]
    public class ThrowStatementTests
    {
        [TestMethod]
        public void Convert_Without_CatchClauseDeclarationAndThrowVariable()
        {
            ConvertHelper.AssertConvertingIgnoreSpaces(@"public void Foo2()
        {
            try
            { }
            catch 
            {
                throw;
            }
        }", @"public Foo2(): void {
        try {

        }
        catch (err) {
            throw err;
        }

    }");
        }

        [TestMethod]
        public void Convert_Without_CatchClauseVariableAndThrowVariable()
        {
            ConvertHelper.AssertConvertingIgnoreSpaces(@"
public void Foo1()
        {
            try
            { }
            catch (Exception)
            {
                throw;
            }
        }
",
@"
public Foo1(): void {
        try {

        }
        catch (err) {
            throw err;
        }

    }
        ");
        }

        [TestMethod]
        public void Convert_WithCatchVariableAndNoThrowVariable()
        {
            ConvertHelper.AssertConvertingIgnoreSpaces(@"
public void Foo3()
        {
            try
            { }
            catch(Exception ett)
            {
                throw;
            }
        }
",
@"
public Foo3(): void {
        try {

        }
        catch (ett) {
            throw ett;
        }

    }
        ");
        }

        [TestMethod]
        public void Convert_WithCatchAndThrowVariable()
        {
            ConvertHelper.AssertConvertingIgnoreSpaces(@"
public void Foo4()
        {
            try
            { }
            catch (Exception ett)
            {
                throw ett;
            }
        }
",
@"
public Foo4(): void {
        try {

        }
        catch (ett) {
            throw ett;
        }

    }
        ");
        }
    }
    
}
