// <copyright file="Type.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain
{
    using System;

    public abstract class StructuralType
    {
        protected StructuralType(Guid id, string name, Domain domain)
        {
            this.Id = id;
            this.SingularName = name;
            this.Domain = domain;

            domain.Types.Add(this);
        }

        public Guid Id { get; }

        public string SingularName { get; }

        public Domain Domain { get; }

        public bool IsInterface => this is Interface;

        public bool IsClass => this is Class;

        public bool IsComposite => !this.IsUnit;

        public bool IsUnit => this is Unit;

        public override string ToString() => this.SingularName;
    }
}
