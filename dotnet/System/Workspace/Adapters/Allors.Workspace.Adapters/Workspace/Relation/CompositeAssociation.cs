// <copyright file="Object.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using Adapters;
    using Meta;

    public class CompositeAssociation<T> : ICompositeAssociation<T>
        where T : class, IObject
    {
        public CompositeAssociation(Strategy @object, IAssociationType associationType)
        {
            this.Object = @object;
            this.AssociationType = associationType;
        }

        IStrategy IRelationEnd.Object => this.Object;

        public Strategy Object { get; }

        public IRelationType RelationType => this.AssociationType.RelationType;

        T ICompositeAssociation<T>.Value => this.Object.Workspace.ObjectFactory.Object<T>(this.Value);

        public IAssociationType AssociationType { get; }

        object IRelationEnd.Value => this.Value;

        public IStrategy Value => this.Object.GetCompositeAssociation(this.AssociationType);

        public long WorkspaceVersion => this.Object.WorkspaceVersion(this.AssociationType);

        public override string ToString()
        {
            return $"[{Value?.Id}]";
        }
    }
}
