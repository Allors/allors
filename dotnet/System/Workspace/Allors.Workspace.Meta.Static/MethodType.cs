﻿// <copyright file="IMethodType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    public sealed class MethodType : MetaIdentifiableObject, IMethodType
    {
        public MethodType(MetaPopulation metaPopulation, string tag)
            : base(metaPopulation, tag)
        {
        }

        public IComposite ObjectType { get; set; }

        private string Name { get; set; }

        public string OperandTag => this.Tag;

        public IRecord Input { get; }

        public IRecord Output { get; }

        public override string ToString() => this.Name;

        public MethodType Init(string tag, IComposite objectType, string name)
        {
            this.Tag = tag;
            this.ObjectType = objectType;
            this.Name = name;

            return this;
        }

    }
}