// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class Domain : RepositoryObject
{
    public Domain(ISet<RepositoryObject> objects, string name, DirectoryInfo directoryInfo)
    {
        this.Name = name;
        this.DirectoryInfo = directoryInfo;

        this.ObjectTypes = new HashSet<ObjectType>();
        this.Properties = new HashSet<Property>();
        this.Methods = new HashSet<Method>();

        objects.Add(this);
    }

    public DirectoryInfo DirectoryInfo { get; }

    public string Name { get; }

    public Domain[] DirectSuperdomains { get; set; }

    public ISet<ObjectType> ObjectTypes { get; }

    public ISet<Property> Properties { get; }

    public ISet<Method> Methods { get; }

    public override string ToString() => this.Name;
}
