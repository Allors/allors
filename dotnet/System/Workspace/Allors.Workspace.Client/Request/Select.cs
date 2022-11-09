// <copyright file="Select.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using System.Collections.Generic;
    using System.Text;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request.Visitor;

    public class Select : IVisitable
    {
        public Select()
        {
        }

        public Select(params IRelationEndType[] relationEndTypes) : this(relationEndTypes, 0)
        {
        }

        internal Select(IRelationEndType[] relationEndTypes, int index)
        {
            this.RelationEndType = relationEndTypes[index];

            var nextIndex = index + 1;
            if (nextIndex < relationEndTypes.Length)
            {
                this.Next = new Select(relationEndTypes, nextIndex);
            }
        }

        public bool IsOne
        {
            get
            {
                if (this.RelationEndType.IsMany)
                {
                    return false;
                }

                return this.ExistNext ? this.Next.IsOne : this.RelationEndType.IsOne;
            }
        }

        public IEnumerable<Node> Include { get; set; }

        public IRelationEndType RelationEndType { get; set; }

        public IComposite OfType { get; set; }

        public Select Next { get; set; }

        public bool ExistNext => this.Next != null;

        public Select End => this.ExistNext ? this.Next.End : this;

        public void Accept(IVisitor visitor) => visitor.VisitSelect(this);

        public IObjectType GetObjectType()
        {
            if (this.ExistNext)
            {
                return this.Next.GetObjectType();
            }

            return this.RelationEndType.ObjectType;
        }

        public override string ToString()
        {
            var name = new StringBuilder();
            name.Append(this.RelationEndType.Name);
            if (this.ExistNext)
            {
                this.Next.ToStringAppendToName(name);
            }

            return name.ToString();
        }

        private void ToStringAppendToName(StringBuilder name)
        {
            name.Append('.').Append(this.RelationEndType.Name);

            if (this.ExistNext)
            {
                this.Next.ToStringAppendToName(name);
            }
        }
    }
}
