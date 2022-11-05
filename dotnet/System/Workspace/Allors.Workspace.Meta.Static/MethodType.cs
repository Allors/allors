// <copyright file="IMethodType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the RoleType type.</summary>

namespace Allors.Workspace.Meta
{
    public sealed class MethodType : MetaIdentifiableObject, IMethodType
    {
        public MethodType(MetaPopulation metaPopulation, string tag, IComposite objectType, string name)
            : base(metaPopulation, tag)
        {
            this.ObjectType = objectType;
            this.Name = name;
        }

        public IComposite ObjectType { get; set; }

        private string Name { get; set; }

        public string OperandTag => this.Tag;

        public IRecordType Input { get; }

        public IRecordType Output { get; }

        public override string ToString() => this.Name;
    }
}
