﻿// <copyright file="Enumeration.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Extent type.</summary>

namespace Allors.Repository
{
    using Attributes;
    

    #region Allors
    [Id("b7bcc22f-03f0-46fd-b738-4e035921d445")]
    #endregion
    public partial interface Enumeration : Object
    {
        #region Allors
        [Id("0480FA94-E067-4195-A656-534E2273E061")]
        #endregion
        [Indexed]
        [Required]
        string Key { get; set; }

        #region Allors
        [Id("07e034f1-246a-4115-9662-4c798f31343f")]
        #endregion
        [Multiplicity(Multiplicity.OneToMany)]
        [Indexed]
        LocalisedText[] LocalisedNames { get; set; }

        #region Allors
        [Id("f57bb62e-77a8-4519-81e6-539d54b71cb7")]
        #endregion
        [Indexed]
        bool IsActive { get; set; }
    }
}
