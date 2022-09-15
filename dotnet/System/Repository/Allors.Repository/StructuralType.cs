// <copyright file="Type.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System.Collections.Generic;

    public abstract class ObjectType : FieldObjectType
    {
        protected ObjectType(ISet<RepositoryObject> objects, string name, Domain domain)
        {
            this.SingularName = name;
            this.Domain = domain;

            domain.ObjectTypes.Add(this);
            objects.Add(this);
        }

        public string SingularName { get; }

        public Domain Domain { get; }

        public override string ToString() => this.SingularName;
    }
}
