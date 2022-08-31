// <copyright file="ToJsonVisitor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Protocol.Direct
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Database;
    using Database.Data;
    using Database.Meta;

    public class ToDatabaseVisitor
    {
        private readonly ITransaction transaction;
        private readonly IMetaPopulation metaPopulation;

        public ToDatabaseVisitor(ITransaction transaction)
        {
            this.transaction = transaction;
            this.metaPopulation = transaction.Database.MetaPopulation;
        }

        public Pull Visit(Request.Pull ws) =>
            new Pull
            {
                ExtentRef = ws.ExtentRef,
                Extent = this.Visit(ws.Extent),
                ObjectType = this.Visit(ws.ObjectType),
                Object = this.Visit(ws.Object) ?? this.Visit(ws.ObjectId),
                Results = this.Visit(ws.Results),
                Arguments = this.Visit(ws.Arguments)
            };

        public Procedure Visit(Request.Procedure ws) =>
            new Procedure(ws.Name)
            {
                Collections = ws.Collections?.ToDictionary(v => v.Key, v => this.transaction.Instantiate(v.Value?.Select(w => w.Id))),
                Objects = ws.Objects?.ToDictionary(v => v.Key, v => v.Value != null ? this.transaction.Instantiate(v.Value.Id) : null),
                Values = ws.Values,
                Pool = ws.Pool?.ToDictionary(v => this.transaction.Instantiate(v.Key.Id), v => v.Value),
            };

        private IExtent Visit(Request.IExtent ws) =>
            ws switch
            {
                Request.Filter filter => this.Visit(filter),
                Request.Except except => this.Visit(except),
                Request.Intersect intersect => this.Visit(intersect),
                Request.Union union => this.Visit(union),
                null => null,
                _ => throw new Exception($"Unknown implementation of IExtent: {ws.GetType()}")
            };

        private Database.Data.Extent Visit(Request.Filter ws) => new Database.Data.Extent(this.Visit(ws.ObjectType))
        {
            Predicate = this.Visit(ws.Predicate),
            Sorting = this.Visit(ws.Sorting)
        };

        private IPredicate Visit(Request.IPredicate ws) =>
            ws switch
            {
                Request.And and => this.Visit(and),
                Request.Between between => this.Visit(between),
                Request.ContainedIn containedIn => this.Visit(containedIn),
                Request.Contains contains => this.Visit(contains),
                Request.Equals equals => this.Visit(equals),
                Request.Exists exists => this.Visit(exists),
                Request.GreaterThan greaterThan => this.Visit(greaterThan),
                Request.Instanceof instanceOf => this.Visit(instanceOf),
                Request.LessThan lessThan => this.Visit(lessThan),
                Request.Like like => this.Visit(like),
                Request.Not not => this.Visit(not),
                Request.Or or => this.Visit(or),
                null => null,
                _ => throw new Exception($"Unknown implementation of IExtent: {ws.GetType()}")
            };

        private IPredicate Visit(Request.And ws) => new And(ws.Operands?.Select(this.Visit).ToArray());

        private IPredicate Visit(Request.Between ws) => new Between(this.Visit(ws.RoleType))
        {
            Parameter = ws.Parameter,
            Values = ws.Values,
            Paths = this.Visit(ws.Paths)
        };

        private IPredicate Visit(Request.ContainedIn ws) => new ContainedIn(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
            Objects = this.Visit(ws.Objects),
            Extent = this.Visit(ws.Extent),
        };

        private IPredicate Visit(Request.Contains ws) => new Contains(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
            Object = this.Visit(ws.Object)
        };

        private IPredicate Visit(Request.Equals ws) => new Equals(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
            Object = this.Visit(ws.Object),
            Value = ws.Value,
            Path = this.Visit(ws.Path),
        };

        private IPredicate Visit(Request.Exists ws) => new Exists(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
        };

        private IPredicate Visit(Request.GreaterThan ws) => new GreaterThan(this.Visit(ws.RoleType))
        {
            Parameter = ws.Parameter,
            Value = ws.Value,
            Path = this.Visit(ws.Path)
        };

        private IPredicate Visit(Request.Instanceof ws) => new Instanceof(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
            ObjectType = this.Visit(ws.ObjectType)
        };

        private IPredicate Visit(Request.LessThan ws) => new LessThan(this.Visit(ws.RoleType))
        {
            Parameter = ws.Parameter,
            Value = ws.Value,
            Path = this.Visit(ws.Path)
        };

        private IPredicate Visit(Request.Like ws) => new Like(this.Visit(ws.RoleType))
        {
            Parameter = ws.Parameter,
            Value = ws.Value,
        };

        private IPredicate Visit(Request.Not ws) => new Not(this.Visit(ws.Operand));

        private IPredicate Visit(Request.Or ws) => new Or(ws.Operands?.Select(this.Visit).ToArray());

        private Except Visit(Request.Except ws) => new Except(ws.Operands?.Select(this.Visit).ToArray()) { Sorting = this.Visit(ws.Sorting) };

        private Intersect Visit(Request.Intersect ws) => new Intersect(ws.Operands?.Select(this.Visit).ToArray()) { Sorting = this.Visit(ws.Sorting) };

        private Union Visit(Request.Union ws) => new Union(ws.Operands?.Select(this.Visit).ToArray()) { Sorting = this.Visit(ws.Sorting) };

        private IObject Visit(Response.IObject ws) => ws != null ? this.transaction.Instantiate(ws.Id) : null;

        private IObject Visit(long? id) => id != null ? this.transaction.Instantiate(id.Value) : null;

        private Result[] Visit(Request.Result[] ws) =>
            ws?.Select(v => new Result
            {
                Name = v.Name,
                Select = this.Visit(v.Select),
                SelectRef = v.SelectRef,
                Skip = v.Skip,
                Take = v.Take,
                Include = this.Visit(v.Include)
            }).ToArray();

        private Select Visit(Request.Select ws) => ws != null ? new Select { Include = this.Visit(ws.Include), PropertyType = this.Visit(ws.PropertyType), Next = this.Visit(ws.Next) } : null;

        private Node[] Visit(IEnumerable<Request.Node> ws) => ws?.Select(this.Visit).ToArray();

        private Node Visit(Request.Node ws) => ws != null ? new Node(this.Visit(ws.PropertyType), ws.Nodes?.Select(this.Visit).ToArray()) : null;

        private Database.Data.Sort[] Visit(Request.Sort[] ws) => ws?.Select(v =>
        {
            return new Database.Data.Sort
            {
                RoleType = this.Visit(v.RoleType),
                SortDirection = v.SortDirection ?? SortDirection.Ascending
            };
        }).ToArray();

        private IObjectType Visit(Meta.IObjectType ws) => ws != null ? (IObjectType)this.metaPopulation.FindByTag(ws.Tag) : null;

        private IComposite Visit(Meta.IComposite ws) => ws != null ? (IComposite)this.metaPopulation.FindByTag(ws.Tag) : null;

        private IPropertyType Visit(Meta.IPropertyType ws) =>
            ws switch
            {
                Meta.AssociationType associationType => this.Visit(associationType),
                Meta.RoleType roleType => this.Visit(roleType),
                null => null,
                _ => throw new ArgumentException("Invalid property type")
            };

        private IAssociationType Visit(Meta.AssociationType ws) => ws != null ? ((IRelationType)this.metaPopulation.FindByTag(ws.OperandTag)).AssociationType : null;

        private IRoleType Visit(Meta.RoleType ws) => ws != null ? ((IRelationType)this.metaPopulation.FindByTag(ws.OperandTag)).RoleType : null;

        private IRoleType[] Visit(IEnumerable<Meta.RoleType> ws) => ws?.Select(v => ((IRelationType)this.metaPopulation.FindByTag(v.OperandTag)).RoleType).ToArray();

        private IObject[] Visit(IEnumerable<Response.IObject> ws) => ws != null ? this.transaction.Instantiate(ws.Select(v => v.Id)) : null;

        private IArguments Visit(IDictionary<string, object> ws) => new Arguments(ws);
    }
}
