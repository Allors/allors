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
    
    public class PatternBuilder<TResult, TIndex> : IPatternBuilder<TResult, TIndex> where TResult : class, IObject
        where TIndex : CompositeIndex
    {
        public IMetaIndex M { get; }

        public TIndex CompositeIndex { get; }

        public PatternBuilder(IMetaIndex m, TIndex compositeIndex)
        {
            this.M = m;
            this.CompositeIndex = compositeIndex;
        }

        public IPattern Pattern(Func<TIndex, RoleType> roleTypeFunc)
        {
            var roleType = roleTypeFunc(this.CompositeIndex);
            return new RolePattern(roleType);
        }

        public IPattern Pattern<T>(RoleType roleType, Expression<System.Func<T, TResult>> select)
            where T : class, IObject
        {
            return new RolePatternSelect<T, TResult>(roleType, select);
        }

        public IPattern Pattern<T>(RoleType roleType, Expression<System.Func<T, IEnumerable<TResult>>> selectMany)
            where T : class, IObject
        {
            return new RolePatternSelectMany<T, TResult>(roleType, selectMany);
        }

        public IPattern Pattern(Func<TIndex, AssociationType> associationTypeFunc)
        {
            var associationType = associationTypeFunc(this.CompositeIndex);
            return new AssociationPattern(associationType);
        }

        public IPattern Pattern<T>(AssociationType associationType, Expression<System.Func<T, TResult>> select)
            where T : class, IObject
        {
            return new AssociationPatternSelect<T, TResult>(associationType, select);
        }

        public IPattern Pattern<T>(AssociationType associationType, Expression<System.Func<T, IEnumerable<TResult>>> selectMany)
            where T : class, IObject
        {
            return new AssociationPatternSelectMany<T, TResult>(associationType, selectMany);
        }
    }
}
