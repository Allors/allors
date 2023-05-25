﻿// <copyright file="Tree.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Meta;

    public class Node : IVisitable
    {
        public Node(IRelationEndType propertyType = null, IEnumerable<Node> nodes = null)
        {
            this.PropertyType = propertyType;
            this.Nodes = nodes?.ToArray() ?? Array.Empty<Node>();
        }

        public IRelationEndType PropertyType { get; }

        public IComposite OfType { get; set; }

        public Node[] Nodes { get; private set; }

        public Node Add(Node node)
        {
            this.Nodes = this.Nodes.Append(node).ToArray();
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

        public IEnumerable<IStrategy> Resolve(IStrategy @object)
        {
            if (this.PropertyType.IsOne)
            {
                var resolved = this.PropertyType.Get(@object, this.OfType);
                if (resolved != null)
                {
                    if (this.Nodes.Length > 0)
                    {
                        foreach (var node in this.Nodes)
                        {
                            foreach (var next in node.Resolve((IStrategy)resolved))
                            {
                                yield return next;
                            }
                        }
                    }
                    else
                    {
                        yield return (IStrategy)resolved;
                    }
                }
            }
            else
            {
                var resolved = (IEnumerable)this.PropertyType.Get(@object, this.OfType);
                if (resolved != null)
                {
                    if (this.Nodes.Length > 0)
                    {
                        foreach (var resolvedItem in resolved)
                        {
                            foreach (var node in this.Nodes)
                            {
                                foreach (var next in node.Resolve((IStrategy)resolvedItem))
                                {
                                    yield return next;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (IStrategy child in resolved)
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
            toString.Append(this.PropertyType.Name + "\n");
            this.ToString(toString, this.Nodes, 1);
            return toString.ToString();
        }

        private void ToString(StringBuilder toString, IReadOnlyCollection<Node> nodes, int level)
        {
            foreach (var node in nodes)
            {
                var indent = new string(' ', level * 2);
                toString.Append(indent + "- " + node.PropertyType + "\n");
                this.ToString(toString, node.Nodes, level + 1);
            }
        }

        public void Accept(IVisitor visitor) => visitor.VisitNode(this);
    }
}
