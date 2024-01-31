﻿// <copyright file="Method.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using Meta;

    public interface IMethod : IOperand, IObservable<IMethod>
    {
        IStrategy Object { get; }

        IMethodType MethodType { get; }

        bool CanExecute { get; }
    }
}
