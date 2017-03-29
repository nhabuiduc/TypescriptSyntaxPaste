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
using Microsoft.VisualStudio.Shell;
using TypescriptSyntaxPaste.VSIX;

namespace TypescriptSyntaxPaste
{
    internal class PasteCommandHandler : IOleCommandTarget
    {
        private readonly Guid _guid = VSConstants.GUID_VSStandardCommandSet97; // The VSConstants.VSStd97CmdID enumeration
        private readonly uint _commandId = (uint)VSConstants.VSStd97CmdID.Paste; // The paste command in the above enumeration

        private ITextView _textView;
        private IOleCommandTarget _nextCommandTarget;
        private DTE2 _dte;
        private Package package;
        private CSharpToTypescriptConverter csharpToTypescriptConverter = new CSharpToTypescriptConverter();

        public PasteCommandHandler(IVsTextView adapter, ITextView textView, DTE2 dte)
        {
            _textView = textView;
            _dte = dte;
            adapter.AddCommandFilter(this, out _nextCommandTarget);
            this.package = package;
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

            var typescriptCode = csharpToTypescriptConverter.ConvertToTypescript(text, SettingStore.Instance);

            if (typescriptCode == null) return false;            

            InsertIntoDocument(doc, typescriptCode);

            return true;
        }
        
        private bool ShouldHandleThisCommand(Guid pguidCmdGroup, uint nCmdID)
        {
            return !(pguidCmdGroup == _guid && nCmdID == _commandId && Clipboard.ContainsText());
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
                // vs2017 bug, exception raise first time, try to ignore and call one more time
                try
                {
                    _dte.ExecuteCommand("Edit.FormatSelection");
                }
                catch
                {
                    _dte.ExecuteCommand("Edit.FormatSelection");
                }
               
            }
        }       
    }
}