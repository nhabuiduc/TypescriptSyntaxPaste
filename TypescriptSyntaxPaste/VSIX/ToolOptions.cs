//------------------------------------------------------------------------------
// <copyright file="ToolOptions.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.Generic;

namespace TypescriptSyntaxPaste.VSIX
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#1110", "#1112", "1.0", IconResourceID = 1400)] // Info on this package for Help/About
    [Guid(ToolOptions.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionPageGrid),
    "Typescript Paste", "General", 0, 0, true)]
    public sealed class ToolOptions : Package
    {
        /// <summary>
        /// ToolOptions GUID string.
        /// </summary>
        public const string PackageGuidString = "310878b7-40bd-4350-850a-cf05f5957496";

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolOptions"/> class.
        /// </summary>
        public ToolOptions()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Instance = this;
        }

        public static ToolOptions Instance { get; set; }
        

        #endregion
    }

    public class OptionPageGrid : DialogPage
    {
        private bool isConvertToInterface = false;


        [Category("Typescript Paste")]
        [DisplayName("Convert to interface")]
        [Description("Convert to interfaces for classes, structs")]
        public bool IsConvertToInterface
        {
            get { return isConvertToInterface; }
            set {
                isConvertToInterface = value;
                SettingStore.Instance.IsConvertToInterface = value;
            }
        }

        [Category("Typescript Paste")]
        [DisplayName("Convert members to camelcase")]
        [Description("Convert to member names to camel case")]
        public bool IsConvertMemberToCamelCase
        {
            get { return SettingStore.Instance.IsConvertMemberToCamelCase; }
            set
            {
                SettingStore.Instance.IsConvertMemberToCamelCase = value;
            }
        }

        [Category("Typescript Paste")]
        [DisplayName("Add ? as optional properties or methods")]
        [Description("Add ? as optional properties or methods")]
        public bool IsOptionalInterfacePropertiesMethods
        {
            get { return SettingStore.Instance.IsInterfaceOptionalProperties; }
            set
            {
                SettingStore.Instance.IsInterfaceOptionalProperties = value;
            }
        }

        [Category("Typescript Paste")]
        [DisplayName("Convert List<T> to array T[]")]
        [Description("Convert List to array T[]")]
        public bool IsConvertListToArray
        {
            get { return SettingStore.Instance.IsConvertListToArray; }
            set
            {
                SettingStore.Instance.IsConvertListToArray = value;
            }
        }

        [Category("Typescript Paste")]
        [DisplayName("Replace Type name")]
        [Description("Replace type name with another name")]
        public TypeNameReplacementData[] ReplacedTypeNameArray
        {
            get { return SettingStore.Instance.ReplacedTypeNameArray; }
            set
            {
                SettingStore.Instance.ReplacedTypeNameArray = value;
            }
        }

        [Category("Typescript Paste")]
        [DisplayName("Add 'I' To Interface Declaration")]
        [Description("Add 'I' Prefix To Interface Declaration")]
        public bool AddIPrefixInterfaceDeclaration
        {
            get { return SettingStore.Instance.AddIPrefixInterfaceDeclaration; }
            set
            {
                SettingStore.Instance.AddIPrefixInterfaceDeclaration = value;
            }
        }
    }

    [Description("Define type name to be replaced")]
    public class TypeNameReplacementData
    {
        [DisplayName("Type Name")]
        public string OldTypeName { get; set; }
        [DisplayName("New Type Name")]
        public string NewTypeName { get; set; }

        public override string ToString()
        {
            return string.Format("{0} => {1}", OldTypeName ?? "name", NewTypeName ?? "new name");
        }

    }
}
