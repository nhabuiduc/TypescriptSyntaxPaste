using EnvDTE;
using EnvDTE80;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using RoslynTypeScript.Translation;
using System;
using System.Windows.Forms;
using System.Linq;

namespace TypescriptSyntaxPaste
{
    internal class PasteCommandHandler : IOleCommandTarget
    {
        private readonly Guid _guid = VSConstants.GUID_VSStandardCommandSet97; // The VSConstants.VSStd97CmdID enumeration
        private readonly uint _commandId = (uint)VSConstants.VSStd97CmdID.Paste; // The paste command in the above enumeration

        private ITextView _textView;
        private IOleCommandTarget _nextCommandTarget;
        private DTE2 _dte;

        public PasteCommandHandler(IVsTextView adapter, ITextView textView, DTE2 dte)
        {
            _textView = textView;
            _dte = dte;
            adapter.AddCommandFilter(this, out _nextCommandTarget);

        }

        private MetadataReference mscorlib;
        private MetadataReference Mscorlib
        {
            get
            {
                if (mscorlib == null)
                {
                    mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
                }

                return mscorlib;
            }
        }      

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {           
            if (HandlePasteCommand(pguidCmdGroup, nCmdID))
            {
                return VSConstants.S_OK;
            }

            return _nextCommandTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup == _guid)
            {
                for (int i = 0; i < cCmds; i++)
                {
                    if (prgCmds[i].cmdID == _commandId)
                    {
                        prgCmds[i].cmdf = (uint)(OLECMDF.OLECMDF_ENABLED | OLECMDF.OLECMDF_SUPPORTED);
                        return VSConstants.S_OK;
                    }
                }
            }

            return _nextCommandTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        private bool HandlePasteCommand(Guid pguidCmdGroup, uint nCmdID)
        {
            if (ShouldHandleThisCommand(pguidCmdGroup, nCmdID))
            {
                return false;
            }

            EnvDTE.TextDocument doc = (EnvDTE.TextDocument)_dte.ActiveDocument.Object("TextDocument");
            if (doc.Language != "TypeScript")
            {
                return false;
            }

            string text = Clipboard.GetText(TextDataFormat.Text);
            var typescriptCode = convertToTypescript(text);

            if (typescriptCode == null)
            {
                return false;
            }

            InsertIntoDocument(doc, typescriptCode);

            return true;
        }
        
        private bool ShouldHandleThisCommand(Guid pguidCmdGroup, uint nCmdID)
        {
            return !(pguidCmdGroup == _guid && nCmdID == _commandId && Clipboard.ContainsText());
        }

        private string convertToTypescript(string text)
        {
            try
            {                
                var tree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(text);

                // detect to see if it's actually C# sourcode by checking whether it has any error
                if (tree.GetDiagnostics().Any(f => f.Severity == DiagnosticSeverity.Error))
                {
                    return null;
                }

                var root = tree.GetRoot();
                var translationNode = TF.Get(root, null);

                var compilation = CSharpCompilation.Create("TemporaryCompilation",
                     syntaxTrees: new[] { tree }, references: new[] { Mscorlib });
                var model = compilation.GetSemanticModel(tree);

                translationNode.Compilation = compilation;
                translationNode.SemanticModel = model;

                translationNode.ApplyPatch();
                return translationNode.Translate();

            }
            catch (Exception ex)
            {
                // TODO
                // swallow exception .!!!!!!!!!!!!!!!!!!!!!!!
            }

            return null;
        }

        private void InsertIntoDocument(EnvDTE.TextDocument doc, string typescriptCode)
        {
            EditPoint start = doc.Selection.TopPoint.CreateEditPoint();

            // First insert plain text
            _dte.UndoContext.Open("Paste");
            doc.Selection.Insert(typescriptCode);
            _dte.UndoContext.Close();
            doc.Selection.MoveToPoint(start, true);

            FormatSelection();
            _textView.Selection.Clear();
        }

        private void FormatSelection()
        {
            Command command = _dte.Commands.Item("Edit.FormatSelection");

            if (command.IsAvailable)
            {
                _dte.ExecuteCommand("Edit.FormatSelection");
            }
        }       
    }
}