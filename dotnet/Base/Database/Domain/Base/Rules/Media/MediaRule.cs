// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database.Derivations;
    using Derivations.Rules;
    using DataUtils;

    public class MediaRule : Rule<Media>
    {
        public MediaRule(IMetaIndex m) : base(m, new Guid("436E574A-FE3E-46ED-8AD2-A59CACC2C9C4")) =>
            this.Patterns =
            [
                new RolePattern<Media, Media>(m.Media.InType),
                new RolePattern<Media, Media>(m.Media.InData),
                new RolePattern<Media, Media>(m.Media.InDataUri),
                new RolePattern<Media, Media>(m.Media.InFileName),
            ];

        public override void Derive(ICycle cycle, IEnumerable<Media> matches)
        {
            foreach (var media in matches)
            {
                var InvalidFileNameChars = System.IO.Path.GetInvalidFileNameChars();
                var InvalidFileNames = new[]
                {
                            "CON", "PRN", "AUX", "NUL", "COM", "LPT"
                        };

                media.Revision = Guid.NewGuid();

                if (media.ExistInData || media.ExistInDataUri)
                {
                    if (!media.ExistMediaContent)
                    {
                        media.MediaContent = media.Strategy.Transaction.Build<MediaContent>();
                    }
                }

                if (media.ExistInData)
                {
                    media.MediaContent.Data = media.InData;
                    media.MediaContent.Type = media.InType ?? MediaContent.Sniff(media.InData, media.InFileName);

                    media.RemoveInType();
                    media.RemoveInData();
                }

                if (media.ExistInDataUri)
                {
                    var dataUrl = new DataUrl(media.InDataUri);

                    media.MediaContent.Data = Convert.FromBase64String(dataUrl.ReadAsBase64EncodedString());
                    media.MediaContent.Type = MediaContent.Sniff(media.MediaContent.Data, media.InFileName);

                    media.RemoveInDataUri();
                }

                if (media.ExistInFileName)
                {
                    media.Name = System.IO.Path.GetFileNameWithoutExtension(media.InFileName);
                    media.RemoveInFileName();
                }

                media.Type = media.MediaContent?.Type;

                var name = !string.IsNullOrWhiteSpace(media.Name) ? media.Name : media.UniqueId.ToString();
                var fileName = $"{name}.{MediaContent.GetExtension(media.Type)}";
                var safeFileName = new string(fileName.Where(ch => !InvalidFileNameChars.Contains(ch)).ToArray());

                var uppercaseSafeFileName = safeFileName.ToUpperInvariant();
                if (InvalidFileNames.Any(invalidFileName => uppercaseSafeFileName.StartsWith(invalidFileName)))
                {
                    safeFileName += "_" + safeFileName;
                }

                media.FileName = safeFileName;
            }
        }
    }
}
