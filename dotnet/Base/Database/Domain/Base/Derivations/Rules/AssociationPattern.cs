// <copyright file="ChangedRoles.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Domain.Derivations.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Allors.Database.Derivations;
    using Allors.Database.Meta;

    public class AssociationPattern<T, TResult> : IAssociationPattern<T, TResult>
        where T : class, IObject
        where TResult : class, IObject
    {
        public AssociationPattern(AssociationType associationType)
        {
            this.AssociationType = associationType;
        }

        public AssociationPattern(AssociationType associationType, Expression<Func<T, TResult>> select)
            : this(associationType)
        {
            this.Select = select;
        }

        public AssociationPattern(AssociationType associationType, Expression<Func<T, IEnumerable<TResult>>> selectMany)
            : this(associationType)
        {
            this.SelectMany = selectMany;
        }

        public AssociationType AssociationType { get; }

        public Expression<Func<T, TResult>> Select { get; }

        public Expression<Func<T, IEnumerable<TResult>>> SelectMany { get; }

        public IEnumerable<IObject> Eval(IObject role)
        {
            if (role is not T @object)
            {
                yield break;
            }

            if (this.Select != null)
            {
                var lambda = this.Select.Compile();
                TResult result = lambda((T)@object);
                if (result != null)
                {
                    yield return result;
                }
            }
            else if (this.SelectMany != null)
            {
                var lambda = this.SelectMany.Compile();
                foreach (var result in lambda((T)@object))
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
