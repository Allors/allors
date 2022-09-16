// <copyright file="Interface.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Repository.Domain;

using System.Collections.Generic;
using System.Linq;
using Inflector;

public class Interface : Composite
{
    public Interface(Inflector inflector, ISet<RepositoryObject> objects, string name, Domain domain)
        : base(inflector, objects, name, domain) =>
        this.InheritedPropertyByRoleName = new Dictionary<string, Property>();


    public override Interface[] Interfaces
    {
        get
        {
            var interfaces = new HashSet<Interface>(this.ImplementedInterfaces);
            foreach (var implementedInterface in this.ImplementedInterfaces)
            {
                implementedInterface.AddInterfaces(interfaces);
            }

            return interfaces.ToArray();
        }
    }

    public Dictionary<string, Property> InheritedPropertyByRoleName { get; }

    public Property[] InheritedProperties => this.InheritedPropertyByRoleName.Values.ToArray();

    public override string ToString() => this.SingularName;

    public Property GetImplementedProperty(Property property)
    {
        var implementedProperty = this.Properties.FirstOrDefault(v => v.RoleName == property.RoleName);
        if (implementedProperty != null)
        {
            return implementedProperty;
        }

        foreach (var @interface in this.ImplementedInterfaces)
        {
            implementedProperty = @interface.GetImplementedProperty(property);
            if (implementedProperty != null)
            {
                return implementedProperty;
            }
        }

        return null;
    }

    public Method GetImplementedMethod(Method method)
    {
        var implementedMethod = this.Methods.FirstOrDefault(v => v.Name == method.Name);
        if (implementedMethod != null)
        {
            return implementedMethod;
        }

        foreach (var @interface in this.ImplementedInterfaces)
        {
            implementedMethod = @interface.GetImplementedMethod(method);
            if (implementedMethod != null)
            {
                return implementedMethod;
            }
        }

        return null;
    }

    private void AddInterfaces(ISet<Interface> interfaces)
    {
        interfaces.UnionWith(this.ImplementedInterfaces);
        foreach (var implementedInterface in this.ImplementedInterfaces)
        {
            implementedInterface.AddInterfaces(interfaces);
        }
    }
}
