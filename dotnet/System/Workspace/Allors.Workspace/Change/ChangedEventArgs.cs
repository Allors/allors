﻿// <copyright file="ChangedEventArgs.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;

    public class ChangedEventArgs : EventArgs
    {
        public ISet<IOperand> Operands { get; }

        public ChangedEventArgs(ISet<IOperand> operands)
        {
            this.Operands = operands;
        }
    }
}
