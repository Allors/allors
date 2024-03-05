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

    public class AssociationPatternSelect<T, TResult> : IAssociationPattern
        where T : class, IObject
        where TResult : class, IObject
    {
        public AssociationPatternSelect(AssociationType associationType, Expression<Func<T, TResult>> select)
        {
            this.AssociationType = associationType;
            this.Select = select;
        }

        public AssociationType AssociationType { get; }

        public Expression<Func<T, TResult>> Select { get; }

        public IEnumerable<IObject> Eval(IObject role)
        {
            if (role is not T @object)
            {
                yield break;
            }

            var lambda = this.Select.Compile();
            TResult result = lambda(@object);
            if (result != null)
            {
                yield return result;
            }
        }
    }
}
