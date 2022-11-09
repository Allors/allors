// <copyright file="Tree.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Request.Extensions;
    using Allors.Workspace.Request.Visitor;
    using Allors.Workspace.Response;

    public class Node : IVisitable
    {
        public Node(IRelationEndType relationEndType = null, IEnumerable<Node> nodes = null)
        {
            this.RelationEndType = relationEndType;
            this.Nodes = nodes?.ToArray() ?? Array.Empty<Node>();
        }

        public IRelationEndType RelationEndType { get; }

        public IComposite OfType { get; set; }

        public Node[] Nodes { get; private set; }

        public void Accept(IVisitor visitor) => visitor.VisitNode(this);

        public Node Add(Node node)
        {
            this.Nodes = this.Nodes.Append(node).ToArray();
            return this;
        }

        public Node Add(IRelationEndType relationEndType)
        {
            var node = new Node(relationEndType);
            return this.Add(node);
        }

        public Node Add(IRelationEndType relationEndType, Node childNode)
        {
            var node = new Node(relationEndType, childNode.Nodes);
            return this.Add(node);
        }

        public IEnumerable<IObject> Resolve(IObject @object)
        {
            if (this.RelationEndType.IsOne)
            {
                var resolved = this.RelationEndType.Get(@object, this.OfType);
                if (resolved != null)
                {
                    if (this.Nodes.Length > 0)
                    {
                        foreach (var node in this.Nodes)
                        {
                            foreach (var next in node.Resolve((IObject)resolved))
                            {
                                yield return next;
                            }
                        }
                    }
                    else
                    {
                        yield return (IObject)resolved;
                    }
                }
            }
            else
            {
                var resolved = (IEnumerable)this.RelationEndType.Get(@object, this.OfType);
                if (resolved != null)
                {
                    if (this.Nodes.Length > 0)
                    {
                        foreach (var resolvedItem in resolved)
                        {
                            foreach (var node in this.Nodes)
                            {
                                foreach (var next in node.Resolve((IObject)resolvedItem))
                                {
                                    yield return next;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (IObject child in resolved)
                        {
                            yield return child;
                        }
                    }
                }
            }
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
    }
}
