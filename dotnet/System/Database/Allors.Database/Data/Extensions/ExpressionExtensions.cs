﻿// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Allors.Database.Meta;

internal class MemberExpressionsVisitor : ExpressionVisitor
{
    public MemberExpressionsVisitor() => this.MemberExpressions = new List<MemberExpression>();

    public IList<MemberExpression> MemberExpressions { get; }

    protected override Expression VisitMember(MemberExpression node)
    {
        this.MemberExpressions.Insert(0, node);
        return base.VisitMember(node);
    }
}

public static class ExpressionExtensions
{
    public static Node Node<T>(this Expression<Func<T, RelationEndType>> @this, MetaPopulation metaPopulation) where T : IComposite
    {
        var visitor = new MemberExpressionsVisitor();
        visitor.Visit(@this);

        return Node<T>(metaPopulation, visitor);
    }

    public static Node Node<T>(this Expression<Func<T, IComposite>> @this, MetaPopulation metaPopulation) where T : IComposite
    {
        var visitor = new MemberExpressionsVisitor();
        visitor.Visit(@this);

        return Node<T>(metaPopulation, visitor);
    }

    private static Node Node<T>(MetaPopulation metaPopulation, MemberExpressionsVisitor visitor) where T : IComposite
    {
        Node path = null;
        Node currentPath = null;

        void AddPath(RelationEndType relationEndType)
        {
            var newNode = new Node(relationEndType);

            if (path == null)
            {
                currentPath = newNode;
                path = currentPath;
            }
            else
            {
                currentPath.Add(newNode);
                currentPath = newNode;
            }
        }

        var root = visitor.MemberExpressions[0].Member.DeclaringType;
        var compositeName = root.Name.Substring(4);
        var composite = metaPopulation.FindCompositeByName(compositeName);

        foreach (var memberExpression in visitor.MemberExpressions)
        {
            if (memberExpression.Type.GetInterfaces().Contains(typeof(IComposite)))
            {
                var propertyInfo = (PropertyInfo)memberExpression.Member;
                var relationEndType = propertyInfo.PropertyType;
                var propertyName = relationEndType.Name.Substring(4);
                composite = metaPopulation.FindCompositeByName(propertyName);

                if (currentPath != null && !currentPath.RelationEndType.ObjectType.Equals(composite))
                {
                    currentPath.OfType = composite;
                }
            }

            if (memberExpression.Type.GetInterfaces().Contains(typeof(RoleType)))
            {
                var name = memberExpression.Member.Name;
                var relationEndType = composite.RoleTypes.First(v => v.Name.Equals(name));
                AddPath(relationEndType);
            }

            if (memberExpression.Type.GetInterfaces().Contains(typeof(AssociationType)))
            {
                var name = memberExpression.Member.Name;
                var relationEndType = composite.AssociationTypes.First(v =>
                {
                    return v.Name.Equals(name);
                });
                AddPath(relationEndType);
            }
        }

        return path;
    }
}
