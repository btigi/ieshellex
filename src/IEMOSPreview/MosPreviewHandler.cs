using System;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using SharpShell.SharpPreviewHandler;

namespace IEMOSPreview
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".mos")]
    [DisplayName("MOS Preview Handler")]
    [Guid("fcd96c4e-d541-4802-8144-3182b2ff9aa3")]
    [PreviewHandler(DisableLowILProcessIsolation = false)]
    public class MosPreviewHandler : SharpPreviewHandler
    {
        /// <summary>
        /// DoPreview must create the preview handler user interface and initialize it with data
        /// provided by the shell.
        /// </summary>
        /// <returns>
        /// The preview handler user interface.
        /// </returns>
        protected override PreviewHandlerControl DoPreview()
        {
            //  Create the handler control.
            var handler = new MosPreviewHandlerControl();

            //  Do we have a file path? If so, we can do a preview.
            if (!string.IsNullOrEmpty(SelectedFilePath))
                handler.DoPreview(SelectedFilePath);

            //  Return the handler control.
            return handler;
        }
    }
}