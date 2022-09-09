// <copyright file="DebuggerDisplayConstants.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;
using Allors.Workspace.Response;

public class StrategyDebugger
{
    private IObject Object { get; set; }

    public StrategyDebugger(IObject @object) => this.Object = @object;

    public string Name
    {
        get
        {
            var roleType = this.Object.Class.RoleTypes.FirstOrDefault(v => v.SingularName == "Name");
            if (roleType != null)
            {
                return this.Object.GetUnitRole(roleType).ToString();
            }

            return null;
        }
    }
}
