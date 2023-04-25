// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Domain
{
    using Meta;

    public partial class Person
    {
        public string FullName
        {
            get
            {
                return $"{((Person)this).FirstName} {((Person)this).LastName}";

            }
        }
    }
}
