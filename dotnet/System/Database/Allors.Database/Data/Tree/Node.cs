// <copyright file="TreeNode.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Allors.Database.Meta;

public class Node : IVisitable
{
    public Node(RelationEndType relationEndType, Node node)
        : this(relationEndType, node != null ? [node] : null)
    {
    }

    public Node(RelationEndType relationEndType, IEnumerable<Node> nodes = null)
    {
        this.RelationEndType = relationEndType;

        if (relationEndType.ObjectType.IsComposite)
        {
            this.Nodes = nodes?.Select(this.AssertAssignable).ToArray();
        }

        this.Nodes ??= Array.Empty<Node>();
    }

    public RelationEndType RelationEndType { get; }

    public Node[] Nodes { get; private set; }

    public IComposite OfType { get; set; }

    public void Accept(IVisitor visitor) => visitor.VisitNode(this);

    public IEnumerable<IObject> Resolve(IObject @object)
    {
        if (this.RelationEndType.IsOne)
        {
            var resolved = this.RelationEndType.Get(@object.Strategy, this.OfType);
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
            var resolved = (IEnumerable)this.RelationEndType.Get(@object.Strategy, this.OfType);
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

    public Node Add(IEnumerable<RelationEndType> relationEndTypes)
    {
        foreach (var relationEndType in relationEndTypes)
        {
            this.Add(relationEndType);
        }

        return this;
    }

    public Node Add(RelationEndType relationEndType)
    {
        var treeNode = new Node(relationEndType);
        this.Add(treeNode);
        return this;
    }

    public Node Add(RelationEndType relationEndType, Node[] subTree)
    {
        var treeNode = new Node(relationEndType, subTree);
        this.Add(treeNode);
        return this;
    }

    public void Add(Node node) => this.Nodes = [.. this.Nodes, this.AssertAssignable(node)];

    private Node AssertAssignable(Node node)
    {
        var composite = this.OfType ?? this.RelationEndType.ObjectType as IComposite;

        if (composite != null)
        {
            IComposite addedComposite = null;

            if (node.RelationEndType is RoleType roleType)
            {
                addedComposite = roleType.AssociationType.ObjectTypeAsComposite;
            }
            else if (node.RelationEndType is AssociationType associationType)
            {
                addedComposite = (IComposite)associationType.RoleType.ObjectType;
            }

            if (addedComposite == null ||
                !(composite.Equals(addedComposite) || composite.Classes.Intersect(addedComposite.Classes).Any()))
            {
                throw new ArgumentException(node.RelationEndType + " is not a valid tree node on " + composite + ".");
            }
        }

        return node;
    }
}
