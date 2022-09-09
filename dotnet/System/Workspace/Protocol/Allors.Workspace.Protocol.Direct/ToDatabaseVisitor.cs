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
    using Meta;
    using Request;
    using And = Request.And;
    using Between = Request.Between;
    using ContainedIn = Request.ContainedIn;
    using Contains = Request.Contains;
    using Equals = Request.Equals;
    using Except = Request.Except;
    using Exists = Request.Exists;
    using Extent = Database.Data.Extent;
    using GreaterThan = Request.GreaterThan;
    using IComposite = Database.Meta.IComposite;
    using IExtent = Database.Data.IExtent;
    using Instanceof = Request.Instanceof;
    using Intersect = Request.Intersect;
    using IObjectType = Database.Meta.IObjectType;
    using IPredicate = Database.Data.IPredicate;
    using IPropertyType = Database.Meta.IPropertyType;
    using LessThan = Request.LessThan;
    using Like = Request.Like;
    using Node = Database.Data.Node;
    using Not = Request.Not;
    using Or = Request.Or;
    using Result = Database.Data.Result;
    using Select = Database.Data.Select;
    using Sort = Database.Data.Sort;
    using Union = Request.Union;

    public class ToDatabaseVisitor
    {
        private readonly ITransaction transaction;
        private readonly IMetaPopulation metaPopulation;

        public ToDatabaseVisitor(ITransaction transaction)
        {
            this.transaction = transaction;
            this.metaPopulation = transaction.Database.MetaPopulation;
        }

        public Pull Visit(PullRequest ws) =>
            new Pull
            {
                ExtentRef = ws.ExtentRef,
                Extent = this.Visit(ws.Extent),
                ObjectType = this.Visit(ws.ObjectType),
                Object = this.Visit(ws.Object) ?? this.Visit(ws.ObjectId),
                Results = this.Visit(ws.Results),
                Arguments = this.Visit(ws.Arguments)
            };

        private IExtent Visit(Request.IExtent ws) =>
            ws switch
            {
                Filter filter => this.Visit(filter),
                Except except => this.Visit(except),
                Intersect intersect => this.Visit(intersect),
                Union union => this.Visit(union),
                null => null,
                _ => throw new Exception($"Unknown implementation of IExtent: {ws.GetType()}")
            };

        private Extent Visit(Filter ws) => new Extent(this.Visit(ws.ObjectType))
        {
            Predicate = this.Visit(ws.Predicate),
            Sorting = this.Visit(ws.Sorting)
        };

        private IPredicate Visit(Request.IPredicate ws) =>
            ws switch
            {
                And and => this.Visit(and),
                Between between => this.Visit(between),
                ContainedIn containedIn => this.Visit(containedIn),
                Contains contains => this.Visit(contains),
                Equals equals => this.Visit(equals),
                Exists exists => this.Visit(exists),
                GreaterThan greaterThan => this.Visit(greaterThan),
                Instanceof instanceOf => this.Visit(instanceOf),
                LessThan lessThan => this.Visit(lessThan),
                Like like => this.Visit(like),
                Not not => this.Visit(not),
                Or or => this.Visit(or),
                null => null,
                _ => throw new Exception($"Unknown implementation of IExtent: {ws.GetType()}")
            };

        private IPredicate Visit(And ws) => new Database.Data.And(ws.Operands?.Select(this.Visit).ToArray());

        private IPredicate Visit(Between ws) => new Database.Data.Between(this.Visit(ws.RoleType))
        {
            Parameter = ws.Parameter,
            Values = ws.Values,
            Paths = this.Visit(ws.Paths)
        };

        private IPredicate Visit(ContainedIn ws) => new Database.Data.ContainedIn(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
            Objects = this.Visit(ws.Objects),
            Extent = this.Visit(ws.Extent),
        };

        private IPredicate Visit(Contains ws) => new Database.Data.Contains(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
            Object = this.Visit(ws.Object)
        };

        private IPredicate Visit(Equals ws) => new Database.Data.Equals(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
            Object = this.Visit(ws.Object),
            Value = ws.Value,
            Path = this.Visit(ws.Path),
        };

        private IPredicate Visit(Exists ws) => new Database.Data.Exists(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
        };

        private IPredicate Visit(GreaterThan ws) => new Database.Data.GreaterThan(this.Visit(ws.RoleType))
        {
            Parameter = ws.Parameter,
            Value = ws.Value,
            Path = this.Visit(ws.Path)
        };

        private IPredicate Visit(Instanceof ws) => new Database.Data.Instanceof(this.Visit(ws.PropertyType))
        {
            Parameter = ws.Parameter,
            ObjectType = this.Visit(ws.ObjectType)
        };

        private IPredicate Visit(LessThan ws) => new Database.Data.LessThan(this.Visit(ws.RoleType))
        {
            Parameter = ws.Parameter,
            Value = ws.Value,
            Path = this.Visit(ws.Path)
        };

        private IPredicate Visit(Like ws) => new Database.Data.Like(this.Visit(ws.RoleType))
        {
            Parameter = ws.Parameter,
            Value = ws.Value,
        };

        private IPredicate Visit(Not ws) => new Database.Data.Not(this.Visit(ws.Operand));

        private IPredicate Visit(Or ws) => new Database.Data.Or(ws.Operands?.Select(this.Visit).ToArray());

        private Database.Data.Except Visit(Except ws) => new Database.Data.Except(ws.Operands?.Select(this.Visit).ToArray()) { Sorting = this.Visit(ws.Sorting) };

        private Database.Data.Intersect Visit(Intersect ws) => new Database.Data.Intersect(ws.Operands?.Select(this.Visit).ToArray()) { Sorting = this.Visit(ws.Sorting) };

        private Database.Data.Union Visit(Union ws) => new Database.Data.Union(ws.Operands?.Select(this.Visit).ToArray()) { Sorting = this.Visit(ws.Sorting) };

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

        private Sort[] Visit(Request.Sort[] ws) => ws?.Select(v =>
        {
            return new Sort
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
                AssociationType associationType => this.Visit(associationType),
                RoleType roleType => this.Visit(roleType),
                null => null,
                _ => throw new ArgumentException("Invalid property type")
            };

        private IAssociationType Visit(AssociationType ws) => ws != null ? ((IRelationType)this.metaPopulation.FindByTag(ws.OperandTag)).AssociationType : null;

        private IRoleType Visit(RoleType ws) => ws != null ? ((IRelationType)this.metaPopulation.FindByTag(ws.OperandTag)).RoleType : null;

        private IRoleType[] Visit(IEnumerable<RoleType> ws) => ws?.Select(v => ((IRelationType)this.metaPopulation.FindByTag(v.OperandTag)).RoleType).ToArray();

        private IObject[] Visit(IEnumerable<Response.IObject> ws) => ws != null ? this.transaction.Instantiate(ws.Select(v => v.Id)) : null;

        private IArguments Visit(IDictionary<string, object> ws) => new Arguments(ws);
    }
}
