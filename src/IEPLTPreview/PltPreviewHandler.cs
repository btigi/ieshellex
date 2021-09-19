using System;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using SharpShell.SharpPreviewHandler;


namespace IEPLTPreview
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".plt")]
    [DisplayName("PLT Preview Handler")]
    [Guid("ebb90723-e429-4c3a-81fb-dda972e46c80")]
    [PreviewHandler(DisableLowILProcessIsolation = false)]
    public class PltPreviewHandler : SharpPreviewHandler
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
            var handler = new PltPreviewHandlerControl();

            //  Do we have a file path? If so, we can do a preview.
            if (!string.IsNullOrEmpty(SelectedFilePath))
                handler.DoPreview(SelectedFilePath);

            //  Return the handler control.
            return handler;
        }
    }
}
