// <copyright file="MethodInvocation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using Allors.Database.Services;

namespace Allors.Database.Meta;

using System;

public class MethodInvocation
{
    public MethodInvocation(Class @class, MethodType methodType)
    {
        this.Class = @class;
        this.MethodType = methodType;
    }

    public Class Class { get; }

    public MethodType MethodType { get; }

    //[DebuggerStepThrough]
    public void Execute(Method method)
    {
        if (method.Executed)
        {
            throw new Exception("Method already executed.");
        }

        method.Executed = true;

        var methodService = method.Object.Strategy.Transaction.Database.Services.Get<IMethodService>();

        foreach (var action in methodService.Get(this.Class, this.MethodType))
        {
            // TODO: Add test for deletion
            if (!method.Object.Strategy.IsDeleted && !method.StopPropagation)
            {
                action(method.Object, method);
            }
        }
    }
}
