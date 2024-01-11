// <copyright file="Config.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Resources;
    using Meta;
    using Population;

    public partial record Config
    {
        public DirectoryInfo DataPath { get; set; }

        public bool SetupSecurity { get; set; } = true;

        public bool SetupAccounting { get; set; } = true;

        public IDictionary<IClass, Record[]> RecordsByClass { get; set; }

        public ITranslation Translation { get; set; }
    }
}
