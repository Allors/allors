﻿// <copyright file="Inheritance.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Inheritance type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Linq;

public sealed class Inheritance : IInheritance, IComparable
{
    public Inheritance(MetaPopulation metaPopulation, Composite subtype, Interface supertype)
    {
        this.MetaPopulation = metaPopulation;
        this.Subtype = subtype;
        this.Supertype = supertype;
        this.MetaPopulation.OnCreated(this);
    }

    public MetaPopulation MetaPopulation { get; }

    IComposite IInheritance.Subtype => this.Subtype;

    public Composite Subtype { get; }

    IInterface IInheritance.Supertype => this.Supertype;

    public Interface Supertype { get; }

    internal string ValidationName
    {
        get
        {
            if (this.Supertype != null && this.Subtype != null)
            {
                return "inheritance " + this.Subtype + "::" + this.Supertype;
            }

            return "unknown inheritance";
        }
    }

    public int CompareTo(object otherObject)
    {
        var other = otherObject as Inheritance;
        return string.CompareOrdinal($"{this.Subtype.Id}{this.Supertype.Id}", $"{other?.Subtype.Id}{other?.Supertype.Id}");
    }

    public override bool Equals(object other) => this.Subtype.Id.Equals((other as Inheritance)?.Subtype.Id) &&
                                                 this.Supertype.Id.Equals((other as Inheritance)?.Supertype.Id);

    public override int GetHashCode() => this.Subtype.Id.GetHashCode() ^ this.Supertype.Id.GetHashCode();

    public override string ToString() => (this.Subtype != null ? this.Subtype.Name : string.Empty) + "::" +
                                         (this.Supertype != null ? this.Supertype.Name : string.Empty);

    internal void Validate(ValidationLog validationLog)
    {
        if (this.Subtype != null && this.Supertype != null)
        {
            if (this.MetaPopulation.Inheritances.Count(inheritance =>
                    this.Subtype.Equals(inheritance.Subtype) && this.Supertype.Equals(inheritance.Supertype)) != 1)
            {
                var message = "name of " + this.ValidationName + " is already in use";
                validationLog.AddError(message, this, ValidationKind.Unique, "Inheritance.Supertype");
            }

            IObjectType tempQualifier = this.Supertype;
            if (tempQualifier is IClass)
            {
                var message = this.ValidationName + " can not have a concrete superclass";
                validationLog.AddError(message, this, ValidationKind.Hierarchy, "Inheritance.Supertype");
            }
        }
        else if (this.Supertype == null)
        {
            var message = this.ValidationName + " has a missing Supertype";
            validationLog.AddError(message, this, ValidationKind.Unique, "Inheritance.Supertype");
        }
        else
        {
            var message = this.ValidationName + " has a missing Subtype";
            validationLog.AddError(message, this, ValidationKind.Unique, "Inheritance.Supertype");
        }
    }
}
