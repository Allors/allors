// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Domain.Derivations.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Allors.Database.Derivations;
    using Allors.Database.Meta;

    public class RolePattern<T, TResult> : IRolePattern<T, TResult>
        where T : class, IObject
        where TResult : class, IObject
    {
        public RolePattern(RoleType roleType)
        {
            this.RoleType = roleType;
        }

        public RolePattern(RoleType roleType, Expression<Func<T, TResult>> select)
            : this(roleType)
        {
            this.Select = select;
        }

        public RolePattern(RoleType roleType, Expression<Func<T, IEnumerable<TResult>>> selectMany)
            : this(roleType)
        {
            this.SelectMany = selectMany;
        }

        public RoleType RoleType { get; }

        public Expression<Func<T, TResult>> Select { get; }

        public Expression<Func<T, IEnumerable<TResult>>> SelectMany { get; }

        public IEnumerable<IObject> Eval(IObject association)
        {
            if (association is not T @object)
            {
                yield break;
            }

            if (this.Select != null)
            {
                var lambda = this.Select.Compile();

                TResult result = default;
                try
                {
                    result = lambda((T)@object);
                }
                catch (NullReferenceException)
                {
                }

                if (result != null)
                {
                    yield return result;
                }
            }
            else if (this.SelectMany != null)
            {
                var lambda = this.SelectMany.Compile();
                IEnumerable<TResult> results = [];

                try
                {
                    results = lambda((T)@object);
                }
                catch (NullReferenceException)
                {
                }

                foreach (var result in results)
                {
                    if (result != null)
                    {
                        yield return result;
                    }
                }
            }
            else
            {
                yield return @object;
            }

        }
    }
}
