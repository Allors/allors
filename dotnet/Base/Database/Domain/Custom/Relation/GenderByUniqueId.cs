// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public partial class GenderByUniqueId
    {
        public Gender Male => this.cache[Gender.MaleId];

        public Gender Female => this.cache[Gender.FemaleId];

        public Gender Other => this.cache[Gender.OtherId];

        public Gender PreferNotToSay => this.cache[Gender.PreferNotToSayId];
    }
}
