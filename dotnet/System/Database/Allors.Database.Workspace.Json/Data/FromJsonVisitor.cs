// <copyright file="FromJsonVisitor.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Allors.Protocol.Json.Data;
using Allors.Database.Data;
using Allors.Database.Meta;
using Extent = Allors.Protocol.Json.Data.Extent;
using IVisitor = Allors.Protocol.Json.Data.IVisitor;
using Node = Allors.Database.Data.Node;
using Pull = Allors.Database.Data.Pull;
using Result = Allors.Database.Data.Result;
using Select = Allors.Database.Data.Select;
using Sort = Allors.Database.Data.Sort;

public class FromJsonVisitor : IVisitor
{
    private readonly Stack<IExtent> extents;
    private readonly FromJson fromJson;
    private readonly Stack<Node> nodes;
    private readonly Stack<IPredicate> predicates;
    private readonly Stack<Result> results;
    private readonly Stack<Select> selects;
    private readonly Stack<Sort> sorts;

    public FromJsonVisitor(FromJson fromJson)
    {
        this.fromJson = fromJson;

        this.extents = new Stack<IExtent>();
        this.predicates = new Stack<IPredicate>();
        this.results = new Stack<Result>();
        this.selects = new Stack<Select>();
        this.nodes = new Stack<Node>();
        this.sorts = new Stack<Sort>();
    }

    public Pull Pull { get; private set; }

    public IExtent Extent => this.extents?.Peek();

    public Select Select => this.selects?.Peek();

    public void VisitExtent(Extent visited)
    {
        IExtentOperator extentOperator = null;
        IExtent sortable = null;

        switch (visited.k)
        {
            case ExtentKind.Filter:
                if (string.IsNullOrWhiteSpace(visited.t))
                {
                    throw new Exception("Unknown object type for " + visited.k);
                }

                var objectType = (IComposite)this.fromJson.MetaPopulation.FindByTag(visited.t);
                var extent = new Data.Extent(objectType);
                sortable = extent;

                this.extents.Push(extent);

                if (visited.p != null)
                {
                    visited.p.Accept(this);
                    extent.Predicate = this.predicates.Pop();
                }

                break;

            case ExtentKind.Union:
                extentOperator = new Union();
                break;

            case ExtentKind.Except:
                extentOperator = new Except();
                break;

            case ExtentKind.Intersect:
                extentOperator = new Intersect();
                break;

            default:
                throw new Exception("Unknown extent kind " + visited.k);
        }

        sortable ??= extentOperator;

        if (visited.s?.Length > 0)
        {
            var length = visited.s.Length;

            sortable.Sorting = new Sort[length];
            for (var i = 0; i < length; i++)
            {
                var sorting = visited.s[i];
                sorting.Accept(this);
                sortable.Sorting[i] = this.sorts.Pop();
            }
        }

        if (extentOperator != null)
        {
            this.extents.Push(extentOperator);

            if (visited.o?.Length > 0)
            {
                var length = visited.o.Length;

                extentOperator.Operands = new IExtent[length];
                for (var i = 0; i < length; i++)
                {
                    var operand = visited.o[i];
                    operand.Accept(this);
                    extentOperator.Operands[i] = this.extents.Pop();
                }
            }
        }
    }

    public void VisitSelect(Allors.Protocol.Json.Data.Select visited)
    {
        var select = new Select
        {
            RelationEndType =
                (IRelationEndType)this.fromJson.MetaPopulation.FindAssociationType(visited.a) ??
                this.fromJson.MetaPopulation.FindRoleType(visited.r),
            OfType = this.fromJson.MetaPopulation.FindComposite(visited.o),
        };

        this.selects.Push(select);

        if (visited.n != null)
        {
            visited.n.Accept(this);
            select.Next = this.selects.Pop();
        }

        if (visited.i?.Length > 0)
        {
            select.Include = new Node[visited.i.Length];
            for (var i = 0; i < visited.i.Length; i++)
            {
                visited.i[i].Accept(this);
                select.Include[i] = this.nodes.Pop();
            }
        }
    }

    public void VisitNode(Allors.Protocol.Json.Data.Node visited)
    {
        var relationEndType = (IRelationEndType)this.fromJson.MetaPopulation.FindAssociationType(visited.a) ??
                           this.fromJson.MetaPopulation.FindRoleType(visited.r);
        var node = new Node(relationEndType);

        this.nodes.Push(node);

        if (visited.n?.Length > 0)
        {
            foreach (var childNode in visited.n)
            {
                childNode.Accept(this);
                node.Add(this.nodes.Pop());
            }
        }
    }

    public void VisitPredicate(Predicate visited)
    {
        switch (visited.k)
        {
            case PredicateKind.And:
                var and = new And();

                this.predicates.Push(and);

                if (visited.ops?.Length > 0)
                {
                    var length = visited.ops.Length;

                    and.Operands = new IPredicate[length];
                    for (var i = 0; i < length; i++)
                    {
                        var operand = visited.ops[i];
                        operand.Accept(this);
                        and.Operands[i] = this.predicates.Pop();
                    }
                }

                break;

            case PredicateKind.Or:
                var or = new Or();

                this.predicates.Push(or);

                if (visited.ops?.Length > 0)
                {
                    var length = visited.ops.Length;

                    or.Operands = new IPredicate[length];
                    for (var i = 0; i < length; i++)
                    {
                        var operand = visited.ops[i];
                        operand.Accept(this);
                        or.Operands[i] = this.predicates.Pop();
                    }
                }

                break;

            case PredicateKind.Not:
                var not = new Not();

                this.predicates.Push(not);

                if (visited.op != null)
                {
                    visited.op.Accept(this);
                    not.Operand = this.predicates.Pop();
                }

                break;

            default:
                var associationType = this.fromJson.MetaPopulation.FindAssociationType(visited.a);
                var roleType = this.fromJson.MetaPopulation.FindRoleType(visited.r);
                var relationEndType = (IRelationEndType)associationType ?? roleType;

                switch (visited.k)
                {
                    case PredicateKind.InstanceOf:

                        var instanceOf = new Instanceof(relationEndType)
                        {
                            ObjectType = visited.o != null ? (IComposite)this.fromJson.MetaPopulation.FindByTag(visited.o) : null,
                            Parameter = visited.p,
                        };

                        this.predicates.Push(instanceOf);
                        break;

                    case PredicateKind.Exists:

                        var exists = new Exists(relationEndType) { Parameter = visited.p };

                        this.predicates.Push(exists);
                        break;

                    case PredicateKind.Contains:

                        var contains = new Contains(relationEndType) { Parameter = visited.p };

                        if (visited.ob.HasValue)
                        {
                            this.fromJson.Resolve(contains, visited.ob.Value);
                        }

                        this.predicates.Push(contains);
                        break;

                    case PredicateKind.ContainedIn:

                        var containedIn = new ContainedIn(relationEndType) { Parameter = visited.p };

                        this.predicates.Push(containedIn);

                        if (visited.obs != null)
                        {
                            this.fromJson.Resolve(containedIn, visited.obs);
                        }
                        else if (visited.e != null)
                        {
                            visited.e.Accept(this);
                            containedIn.Extent = this.extents.Pop();
                        }

                        break;

                    case PredicateKind.Equals:

                        var equals = new Equals(relationEndType)
                        {
                            Parameter = visited.p, Path = this.fromJson.MetaPopulation.FindRoleType(visited.pa),
                        };

                        this.predicates.Push(equals);

                        if (visited.ob != null)
                        {
                            this.fromJson.Resolve(equals, visited.ob.Value);
                        }
                        else if (visited.v != null)
                        {
                            if (roleType?.ObjectType.IsUnit == true)
                            {
                                equals.Value = this.fromJson.UnitConvert.UnitFromJson(((IRoleType)relationEndType).ObjectType.Tag, visited.v);
                            }
                            else
                            {
                                var id = XmlConvert.ToInt64(visited.v.ToString());
                                this.fromJson.Resolve(equals, id);
                            }
                        }

                        break;

                    case PredicateKind.Between:

                        var between = new Between(roleType)
                        {
                            Parameter = visited.p,
                            Values = visited.vs?.Select(v => this.fromJson.UnitConvert.UnitFromJson(roleType.ObjectType.Tag, v))
                                .ToArray(),
                            Paths = visited.pas?.Select(v => this.fromJson.MetaPopulation.FindRoleType(v)).ToArray(),
                        };

                        this.predicates.Push(between);

                        break;

                    case PredicateKind.GreaterThan:

                        var greaterThan = new GreaterThan(roleType)
                        {
                            Parameter = visited.p,
                            Value = this.fromJson.UnitConvert.UnitFromJson(roleType.ObjectType.Tag, visited.v),
                            Path = this.fromJson.MetaPopulation.FindRoleType(visited.pa),
                        };

                        this.predicates.Push(greaterThan);

                        break;

                    case PredicateKind.LessThan:

                        var lessThan = new LessThan(roleType)
                        {
                            Parameter = visited.p,
                            Value = this.fromJson.UnitConvert.UnitFromJson(roleType.ObjectType.Tag, visited.v),
                            Path = this.fromJson.MetaPopulation.FindRoleType(visited.pa),
                        };

                        this.predicates.Push(lessThan);

                        break;

                    case PredicateKind.Like:

                        var like = new Like(roleType)
                        {
                            Parameter = visited.p,
                            Value = this.fromJson.UnitConvert.UnitFromJson(roleType.ObjectType.Tag, visited.v)?.ToString(),
                        };

                        this.predicates.Push(like);

                        break;

                    default:
                        throw new Exception("Unknown predicate kind " + visited.k);
                }

                break;
        }
    }

    public void VisitPull(Allors.Protocol.Json.Data.Pull visited)
    {
        var pull = new Pull
        {
            ExtentRef = visited.er,
            ObjectType = !string.IsNullOrWhiteSpace(visited.t) ? (IObjectType)this.fromJson.MetaPopulation.FindByTag(visited.t) : null,
            Arguments = visited.a != null ? new Arguments(visited.a, this.fromJson.UnitConvert) : null,
        };

        if (visited.o != null)
        {
            this.fromJson.Resolve(pull, visited.o.Value);
        }

        if (visited.e != null)
        {
            visited.e.Accept(this);
            pull.Extent = this.extents.Pop();
        }

        if (visited.r?.Length > 0)
        {
            var length = visited.r.Length;

            pull.Results = new Result[length];
            for (var i = 0; i < length; i++)
            {
                var result = visited.r[i];
                result.Accept(this);
                pull.Results[i] = this.results.Pop();
            }
        }

        this.Pull = pull;
    }

    public void VisitResult(Allors.Protocol.Json.Data.Result visited)
    {
        var result = new Result { SelectRef = visited.r, Name = visited.n, Skip = visited.k, Take = visited.t };

        if (visited.s != null)
        {
            visited.s.Accept(this);
            result.Select = this.selects.Pop();
        }

        if (visited.i?.Length > 0)
        {
            result.Include = new Node[visited.i.Length];
            for (var i = 0; i < visited.i.Length; i++)
            {
                visited.i[i].Accept(this);
                result.Include[i] = this.nodes.Pop();
            }
        }

        this.results.Push(result);
    }

    public void VisitSort(Allors.Protocol.Json.Data.Sort visited)
    {
        var sort = new Sort
        {
            SortDirection = visited.d ?? SortDirection.Ascending,
            RoleType = !string.IsNullOrWhiteSpace(visited.r)
                ? ((IRelationType)this.fromJson.MetaPopulation.FindByTag(visited.r)).RoleType
                : null,
        };

        this.sorts.Push(sort);
    }
}
