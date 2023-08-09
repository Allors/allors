// <copyright file="TranslationVisitor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;

    public class ReactiveVisitor : ExpressionVisitor
    {
        private ParameterExpression dependencies;

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            this.dependencies = Expression.Parameter(typeof(HashSet<INotifyPropertyChanged>), "_dependencies_");
            var body = this.Visit(node.Body);
            return Expression.Lambda(body, node.Name, node.Parameters.Append(this.dependencies));
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var member = node.Member;
            var declaringType = member.DeclaringType;

            if (declaringType.GetInterfaces().Contains(typeof(INotifyPropertyChanged)))
            {
                Console.WriteLine(0);
            }

            return base.VisitMember(node);
        }
    }
}
