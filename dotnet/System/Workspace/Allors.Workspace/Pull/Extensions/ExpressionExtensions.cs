﻿// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Workspace.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Meta;

    internal class MemberExpressionsVisitor : ExpressionVisitor
    {
        public IList<MemberExpression> MemberExpressions { get; } = new List<MemberExpression>();

        protected override Expression VisitMember(MemberExpression node)
        {
            this.MemberExpressions.Insert(0, node);
            return base.VisitMember(node);
        }
    }

    public static partial class ExpressionExtensions
    {
        public static Node Node<T>(this Expression<Func<T, IRelationEndType>> @this, IMetaPopulation metaPopulation) where T : IComposite
        {
            var visitor = new MemberExpressionsVisitor();
            visitor.Visit(@this);

            return Node<T>(metaPopulation, visitor);
        }

        public static Node Node<T>(this Expression<Func<T, IComposite>> @this, IMetaPopulation metaPopulation) where T : IComposite
        {
            var visitor = new MemberExpressionsVisitor();
            visitor.Visit(@this);

            return Node<T>(metaPopulation, visitor);
        }

        private static Node Node<T>(IMetaPopulation metaPopulation, MemberExpressionsVisitor visitor) where T : IComposite
        {
            Node path = null;
            Node currentPath = null;

            void AddPath(IRelationEndType propertyType)
            {
                var newNode = new Node(propertyType);

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
            string rootName = root.Name.Substring(5); // remove I from IMeta[Name]
            var composite = metaPopulation.FindByName(rootName);

            foreach (var memberExpression in visitor.MemberExpressions)
            {
                if (memberExpression.Type.GetInterfaces().Contains(typeof(IComposite)))
                {
                    var propertyInfo = (PropertyInfo)memberExpression.Member;
                    var propertyType = propertyInfo.PropertyType;
                    string propertyTypeName = propertyType.Name.Substring(5); // remove I from IMeta[Name]
                    composite = metaPopulation.FindByName(propertyTypeName);

                    if (currentPath != null && !currentPath.RelationEndType.ObjectType.Equals(composite))
                    {
                        currentPath.OfType = composite;
                    }
                }

                if (memberExpression.Type.GetInterfaces().Contains(typeof(IRoleType)))
                {
                    var name = memberExpression.Member.Name;
                    var propertyType = composite.RoleTypes.First(v => v.Name.Equals(name));
                    AddPath(propertyType);
                }

                if (memberExpression.Type.GetInterfaces().Contains(typeof(IAssociationType)))
                {
                    var name = memberExpression.Member.Name;
                    var propertyType = composite.AssociationTypes.First(v => v.Name.Equals(name));
                    AddPath(propertyType);
                }
            }

            return path;
        }
    }
}
