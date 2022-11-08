// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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
    public Node(IRelationEndType relationEndType, IEnumerable<Node> nodes = null)
    {
        this.RelationEndType = relationEndType;
        this.Composite = this.RelationEndType.ObjectType.IsComposite ? (IComposite)relationEndType.ObjectType : null;

        if (relationEndType.ObjectType.IsComposite)
        {
            this.Nodes = nodes?.Select(this.AssertAssignable).ToArray();
        }

        this.Nodes ??= Array.Empty<Node>();
    }

    public IRelationEndType RelationEndType { get; }

    public IComposite Composite { get; }

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

    public Node Add(IEnumerable<IRelationEndType> relationEndTypes)
    {
        foreach (var relationEndType in relationEndTypes)
        {
            this.Add(relationEndType);
        }

        return this;
    }

    public Node Add(IRelationEndType relationEndType)
    {
        var treeNode = new Node(relationEndType);
        this.Add(treeNode);
        return this;
    }

    public Node Add(IRelationEndType relationEndType, Node[] subTree)
    {
        var treeNode = new Node(relationEndType, subTree);
        this.Add(treeNode);
        return this;
    }

    public void Add(Node node) => this.Nodes = this.Nodes.Append(this.AssertAssignable(node)).ToArray();

    private Node AssertAssignable(Node node)
    {
        if (this.Composite != null)
        {
            IComposite addedComposite = null;

            if (node.RelationEndType is IRoleType roleType)
            {
                addedComposite = roleType.AssociationType.ObjectType;
            }
            else if (node.RelationEndType is IAssociationType associationType)
            {
                addedComposite = (IComposite)associationType.RoleType.ObjectType;
            }

            if (addedComposite == null ||
                !(this.Composite.Equals(addedComposite) || this.Composite.Classes.Intersect(addedComposite.Classes).Any()))
            {
                throw new ArgumentException(node.RelationEndType + " is not a valid tree node on " + this.Composite + ".");
            }
        }

        return node;
    }
}
