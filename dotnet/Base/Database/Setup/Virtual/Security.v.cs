﻿// <copyright file="Security.v.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class Security
    {
        private void OnPreSetup()
        {
            this.BaseOnPreSetup();
            this.CustomOnPreSetup();
        }

        private void OnPostSetup()
        {
            this.BaseOnPostSetup();
            this.CustomOnPostSetup();
        }
    }
}
