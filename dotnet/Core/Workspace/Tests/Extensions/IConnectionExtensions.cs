// <copyright file="Test.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests.Workspace
{
    using System.Linq;
    using System.Threading.Tasks;
    using Allors.Workspace;
    using Allors.Workspace.Request;
    using Allors.Workspace.Meta;
    using Allors.Workspace.Response;

    public static class IConnectionExtensions
    {
        public static async Task<IObject> PullObject(this IConnection @this, IComposite objectType, string name)
        {
            var roleType = objectType.RoleTypes.First(v => v.Name.Equals("Name"));
            var pull = new PullRequest { Extent = new Filter(objectType) { Predicate = new Equals(roleType) { Value = name } } };
            var result = await @this.PullAsync(pull);
            var collection = result.GetCollection(objectType);
            return collection[0];
        }
    }
}
