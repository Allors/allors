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

    public class AssociationPatternSelectMany<T, TResult> : IAssociationPattern
        where T : class, IObject
        where TResult : class, IObject
    {
        public AssociationPatternSelectMany(AssociationType associationType, Expression<Func<T, IEnumerable<TResult>>> selectMany)
        {
            this.AssociationType = associationType;
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

            var lambda = this.SelectMany.Compile();
            foreach (var result in lambda((T)@object))
            {
                if (result != null)
                {
                    yield return result;
                }
            }
        }
    }
}
