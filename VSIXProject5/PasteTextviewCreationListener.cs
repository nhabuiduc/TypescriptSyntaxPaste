using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace TypescriptSyntaxPaste
{
    [PackageRegistration()]
    [Export(typeof(IVsTextViewCreationListener))]
    [ContentType("code")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    class PasteTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        internal IVsEditorAdaptersFactoryService EditorAdaptersFactoryService { get; set; }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            var textView = EditorAdaptersFactoryService.GetWpfTextView(textViewAdapter);
            var dte = ServiceProvider.GlobalProvider.GetService(typeof(DTE)) as DTE2;
            
            textView.Properties.GetOrCreateSingletonProperty<PasteCommandHandler>(() => new PasteCommandHandler(textViewAdapter, textView, dte));
        }
    }
}
