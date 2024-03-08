// <copyright file="Method.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta;

public abstract class Method(IObject @object)
{
    public abstract MethodInvocation MethodInvocation { get; }

    public IObject Object { get; } = @object;

    public bool Executed { get; set; } = false;

    public bool StopPropagation { get; set; }

    public virtual void Execute() => this.MethodInvocation.Execute(this);
}
