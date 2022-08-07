// <copyright file="PrintableExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;

    public static partial class PrintableExtensions
    {
        public static void RenderPrintDocument(this Printable @this, Template template, object model, IDictionary<string, byte[]> images)
        {
            var document = template?.Render(model, images);
            @this.UpdatePrintDocument(document);
        }

        public static void RenderPrintDocument(this Printable @this, Template template, IDictionary<string, object> model, IDictionary<string, byte[]> images)
        {
            var document = template?.Render(model, images);
            @this.UpdatePrintDocument(document);
        }

        public static void ResetPrintDocument(this Printable @this)
        {
            if (!@this.ExistPrintDocument)
            {
                @this.PrintDocument = @this.Transaction().Create<PrintDocument>();
            }

            @this.PrintDocument.Media?.Delete();
        }

        private static void UpdatePrintDocument(this Printable @this, byte[] document)
        {
            if (document != null)
            {
                if (!@this.ExistPrintDocument)
                {
                    @this.PrintDocument = @this.Transaction().Create<PrintDocument>();
                }

                if (!@this.PrintDocument.ExistMedia)
                {
                    @this.PrintDocument.Media = @this.Transaction().Create<Media>();
                }

                @this.PrintDocument.Media.InData = document;
            }
            else
            {
                @this.ResetPrintDocument();
            }
        }
    }
}
