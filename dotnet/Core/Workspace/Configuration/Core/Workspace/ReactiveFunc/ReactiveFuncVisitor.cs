// <copyright file="TranslationVisitor.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Configuration
{
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ReactiveFuncVisitor : ExpressionVisitor
    {
        private ParameterExpression dependencies;

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            this.dependencies = Expression.Parameter(typeof(IDependencyTracker), "_tracker_");
            var body = this.Visit(node.Body);
            return Expression.Lambda(body, node.Name, node.Parameters.Append(this.dependencies));
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var member = node.Member;

            if (member is EventInfo eventInfo)
            {

            }
            else if (member is FieldInfo fieldInfo)
            {

            }
            else if (member is MethodInfo methodInfo)
            {
                var x = member.ToString();
            }
            else if (member is PropertyInfo propertyInfo)
            {
                var propertyType = propertyInfo.PropertyType;

                if (propertyType.GetInterfaces().Contains(typeof(INotifyPropertyChanged)))
                {
                    var trackMethodInfo = typeof(INotifyPropertyChangedExtensions).GetMethod("Track").MakeGenericMethod(node.Type);

                    var methodCallExpression = Expression.Call(null, trackMethodInfo, base.VisitMember(node), this.dependencies);

                    return methodCallExpression;
                }
            }

            return base.VisitMember(node);
        }

    }
}
