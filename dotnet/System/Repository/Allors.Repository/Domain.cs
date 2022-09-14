// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Domain : RepositoryObject
    {
        public Domain(ISet<RepositoryObject> objects, Guid id, string name, DirectoryInfo directoryInfo)
        {
            this.Id = id;
            this.Name = name;
            this.DirectoryInfo = directoryInfo;

            this.StructuralTypes = new HashSet<StructuralType>();
            this.Properties = new HashSet<Property>();
            this.Methods = new HashSet<Method>();

            objects.Add(this);
        }

        public Guid Id { get; }

        public DirectoryInfo DirectoryInfo { get; }

        public string Name { get; }

        public Domain Base { get; set; }

        public ISet<StructuralType> StructuralTypes { get; }

        public ISet<Property> Properties { get; }

        public ISet<Method> Methods { get; }

        public override string ToString() => this.Name;
    }
}
