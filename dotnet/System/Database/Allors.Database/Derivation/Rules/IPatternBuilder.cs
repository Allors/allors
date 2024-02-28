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
    using Database.Derivations;
    using Meta;

    public interface IPatternBuilder<TResult, TIndex> 
        where TResult : class, IObject 
        where TIndex : CompositeIndex
    {
        IPattern Pattern(Func<TIndex, RoleType> roleTypeFunc);

        IPattern Pattern<T>(RoleType roleType, Expression<Func<T, TResult>> select)
            where T : class, IObject;

        IPattern Pattern<T>(RoleType roleType, Expression<Func<T, IEnumerable<TResult>>> selectMany)
            where T : class, IObject;

        IPattern Pattern(Func<TIndex, AssociationType> associationTypeFunc);

        IPattern Pattern<T>(AssociationType associationType, Expression<Func<T, TResult>> select)
            where T : class, IObject;

        IPattern Pattern<T>(AssociationType associationType, Expression<Func<T, IEnumerable<TResult>>> selectMany)
            where T : class, IObject;
    }
}
