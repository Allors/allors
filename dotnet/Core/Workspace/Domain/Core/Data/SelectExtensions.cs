// <copyright file="StepExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Domain
{
    using System.Collections;
    using System.Collections.Generic;
    using Allors.Workspace.Request;
    using Allors.Workspace.Request.Extensions;
    using Allors.Workspace.Response;

    public static class SelectExtensions
    {
        public static IEnumerable<IObject> Get(this Select @this, IObject @object)
        {
            if (@this.RelationEndType.IsOne)
            {
                var resolved = @this.RelationEndType.Get(@object);
                if (resolved != null)
                {
                    if (@this.ExistNext)
                    {
                        foreach (var next in @this.Next.Get((IObject)resolved))
                        {
                            yield return next;
                        }
                    }
                    else
                    {
                        yield return (IObject)@this.RelationEndType.Get(@object);
                    }
                }
            }
            else
            {
                var resolved = (IEnumerable)@this.RelationEndType.Get(@object);
                if (resolved != null)
                {
                    if (@this.ExistNext)
                    {
                        foreach (var resolvedItem in resolved)
                        {
                            foreach (var next in @this.Next.Get((IObject)resolvedItem))
                            {
                                yield return next;
                            }
                        }
                    }
                    else
                    {
                        foreach (var child in (IEnumerable<IObject>)@this.RelationEndType.Get(@object))
                        {
                            yield return child;
                        }
                    }
                }
            }
        }
    }
}
