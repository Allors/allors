// <copyright file="BarcodeGenerator.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration
{
    using System;
    using System.IO;
    using Domain;
    using SkiaSharp;
    using ZXing;
    using ZXing.Common;
    using ZXing.SkiaSharp;

    public class ZXingBarcodeGenerator : IBarcodeGenerator
    {
        public byte[] Generate(string content, BarcodeType type, int? width, int? height, int? margin, bool? pure)
        {
            BarcodeFormat barcodeFormat = type switch
            {
                BarcodeType.AZTEC => BarcodeFormat.AZTEC,
                BarcodeType.CODABAR => BarcodeFormat.CODABAR,
                BarcodeType.CODE_39 => BarcodeFormat.CODE_39,
                BarcodeType.CODE_93 => BarcodeFormat.CODE_93,
                BarcodeType.CODE_128 => BarcodeFormat.CODE_128,
                BarcodeType.DATA_MATRIX => BarcodeFormat.DATA_MATRIX,
                BarcodeType.EAN_8 => BarcodeFormat.EAN_8,
                BarcodeType.EAN_13 => BarcodeFormat.EAN_13,
                BarcodeType.ITF => BarcodeFormat.ITF,
                BarcodeType.MAXICODE => BarcodeFormat.MAXICODE,
                BarcodeType.PDF_417 => BarcodeFormat.PDF_417,
                BarcodeType.QR_CODE => BarcodeFormat.QR_CODE,
                BarcodeType.RSS_14 => BarcodeFormat.RSS_14,
                BarcodeType.RSS_EXPANDED => BarcodeFormat.RSS_EXPANDED,
                BarcodeType.UPC_A => BarcodeFormat.UPC_A,
                BarcodeType.UPC_E => BarcodeFormat.UPC_E,
                BarcodeType.UPC_EAN_EXTENSION => BarcodeFormat.UPC_EAN_EXTENSION,
                BarcodeType.MSI => BarcodeFormat.MSI,
                BarcodeType.PLESSEY => BarcodeFormat.PLESSEY,
                BarcodeType.IMB => BarcodeFormat.IMB,
                _ => throw new ArgumentException()
            };

            var barcodeWriter = new BarcodeWriter
            {
                Format = barcodeFormat,
            };

            if (width.HasValue || height.HasValue || margin.HasValue)
            {
                barcodeWriter.Options = new EncodingOptions();
                if (width.HasValue)
                {
                    barcodeWriter.Options.Width = width.Value;
                }

                if (height.HasValue)
                {
                    barcodeWriter.Options.Height = height.Value;
                }

                if (margin.HasValue)
                {
                    barcodeWriter.Options.Margin = margin.Value;
                }

                if (pure.HasValue)
                {
                    barcodeWriter.Options.PureBarcode = pure.Value;
                }
            }

            using (var image = barcodeWriter.Write(content))
            {
                using (var memStream = new MemoryStream())
                {
                    using (var wstream = new SKManagedWStream(memStream))
                    {
                        if (image.Encode(wstream, SKEncodedImageFormat.Png, 100))
                        {
                            return memStream.ToArray();
                        }

                        return null;
                    }
                }
            }
        }
    }
}
