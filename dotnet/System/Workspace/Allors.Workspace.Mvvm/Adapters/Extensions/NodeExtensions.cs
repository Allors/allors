// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workspace;
using Workspace.Meta;

public static class NodeExtensions
{
    public static void Flatten(this Node @this, IStrategy @object, IList<IRelationEnd> relationEnds, out bool complete)
    {
        complete = true;

        if (@object == null)
        {
            complete = false;
            return;
        }

        if (@this.RelationEndType is IRoleType roleType)
        {
            if (roleType.IsMany)
            {
                throw new NotSupportedException("Multiplicity many is not supported");
            }

            if (@this.Nodes.Length > 1)
            {
                throw new NotSupportedException("More than one child node is not supported");
            }

            if (roleType.ObjectType.IsUnit)
            {
                var role = @object.Role(roleType);
                relationEnds.Add(role);
            }
            else
            {
                var compositeRole = @object.CompositeRole(roleType);
                relationEnds.Add(compositeRole);

                if (@this.Nodes.Any())
                {
                    if (!compositeRole.Exist)
                    {
                        complete = false;
                        return;
                    }

                    @this.Nodes[0].Flatten(compositeRole.Value, relationEnds, out complete);
                }
            }
        }
        else if (@this.RelationEndType is IAssociationType associationType)
        {
            if (associationType.IsMany)
            {
                throw new NotSupportedException("Multiplicity many is not supported");
            }

            if (@this.Nodes.Length > 1)
            {
                throw new NotSupportedException("More than one child node is not supported");
            }

            var compositeAssociation = @object.CompositeAssociation(associationType);
            relationEnds.Add(compositeAssociation);

            if (@this.Nodes.Any())
            {
                if (compositeAssociation.Value == null)
                {
                    complete = false;
                    return;
                }

                @this.Nodes[0].Flatten(compositeAssociation.Value, relationEnds, out complete);
            }
        }
    }
}
