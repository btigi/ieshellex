﻿using System;
using System.Runtime.InteropServices;
using SharpShell.Attributes;
using SharpShell.SharpPreviewHandler;

namespace IETISPreview
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".tis")]
    [DisplayName("TIS Preview Handler")]
    [Guid("29cddbf5-ac95-41f2-961d-efaf8ba82016")]
    [PreviewHandler(DisableLowILProcessIsolation = false)]
    public class TisPreviewHandler : SharpPreviewHandler
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
            var handler = new TisPreviewHandlerControl();

            //  Do we have a file path? If so, we can do a preview.
            if (!string.IsNullOrEmpty(SelectedFilePath))
                handler.DoPreview(SelectedFilePath);

            //  Return the handler control.
            return handler;
        }
    }
}