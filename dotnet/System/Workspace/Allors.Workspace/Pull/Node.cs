// <copyright file="Tree.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Meta;

    public class Node(IRelationEndType relationEndType = null, IEnumerable<Node> nodes = null)
        : IVisitable
    {
        public IRelationEndType RelationEndType { get; } = relationEndType;

        public IComposite OfType { get; set; }

        public Node[] Nodes { get; private set; } = nodes?.ToArray() ?? Array.Empty<Node>();

        public Node Add(Node node)
        {
            this.Nodes = [.. this.Nodes, node];
            return this;
        }

        public Node Add(IRelationEndType propertyType)
        {
            var node = new Node(propertyType, null);
            return this.Add(node);
        }

        public Node Add(IRelationEndType propertyType, Node childNode)
        {
            var node = new Node(propertyType, childNode.Nodes);
            return this.Add(node);
        }

        public override string ToString()
        {
            var toString = new StringBuilder();
            toString.Append(this.RelationEndType.Name + "\n");
            this.ToString(toString, this.Nodes, 1);
            return toString.ToString();
        }

        private void ToString(StringBuilder toString, IReadOnlyCollection<Node> nodes, int level)
        {
            foreach (var node in nodes)
            {
                var indent = new string(' ', level * 2);
                toString.Append(indent + "- " + node.RelationEndType + "\n");
                this.ToString(toString, node.Nodes, level + 1);
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitNode(this);
    }
}
