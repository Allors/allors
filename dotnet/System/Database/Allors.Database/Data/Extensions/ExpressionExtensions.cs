﻿// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Data;

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public static Node Node<T>(this Expression<Func<T, IRelationEndTypeIndex>> @this, MetaPopulation metaPopulation) where T : ICompositeIndex
    {
        var visitor = new MemberExpressionsVisitor();
        visitor.Visit(@this);

        return Node<T>(metaPopulation, visitor);
    }

    public static Node Node<T>(this Expression<Func<T, ICompositeIndex>> @this, MetaPopulation metaPopulation) where T : ICompositeIndex
    {
        var visitor = new MemberExpressionsVisitor();
        visitor.Visit(@this);

        return Node<T>(metaPopulation, visitor);
    }

    private static Node Node<T>(MetaPopulation metaPopulation, MemberExpressionsVisitor visitor) where T : ICompositeIndex
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
        var compositeName = root.Name.Substring(0, root.Name.Length - "Index".Length);

        if (compositeName == "CC")
        {
            Debugger.Break();
        }

        var composite = metaPopulation.FindCompositeByName(compositeName);

        foreach (var memberExpression in visitor.MemberExpressions)
        {
            if (memberExpression.Type.IsSubclassOf(typeof(ICompositeIndex)))
            {
                var propertyInfo = (PropertyInfo)memberExpression.Member;
                var relationEndType = propertyInfo.PropertyType;
                var propertyName = relationEndType.Name.Substring(0, relationEndType.Name.Length - "Index".Length);
                composite = metaPopulation.FindCompositeByName(propertyName);

                if (currentPath != null && !currentPath.RelationEndType.ObjectType.Equals(composite))
                {
                    currentPath.OfType = composite;
                }
            }

            if (memberExpression.Type.IsSubclassOf(typeof(IRoleTypeIndex)))
            {
                var name = memberExpression.Member.Name;
                var relationEndType = composite.RoleTypes.First(v => v.Name.Equals(name));
                AddPath(relationEndType);
            }

            if (memberExpression.Type.IsSubclassOf(typeof(IAssociationTypeIndex)))
            {
                var name = memberExpression.Member.Name;

                if (name == "AAsWhereMany2Many")
                {
                    Debugger.Break();
                }

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
