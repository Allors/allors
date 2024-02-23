// <copyright file="Domain.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;
using Embedded;
using Embedded.Meta;

public sealed class Domain : EmbeddedObject, IMetaIdentifiableObject, IComparable
{
    public Domain(IEmbeddedPopulation embeddedPopulation, EmbeddedObjectType embeddedObjectType)
     : base(embeddedPopulation, embeddedObjectType)
    {
        this.Attributes = new MetaExtension();
        this.MetaPopulation = (MetaPopulation)embeddedPopulation;
        this.MetaPopulation.OnCreated(this);
    }

    public dynamic Attributes { get; }

    MetaPopulation IMetaIdentifiableObject.MetaPopulation => this.MetaPopulation;

    public MetaPopulation MetaPopulation { get; }
    
    public Guid Id { get; set; }

    public string Tag { get; set; }

    public string Name { get; set;  }

    public IReadOnlyList<Domain> DirectSuperdomains { get; set;  }

    public IReadOnlyList<Domain> Superdomains { get; private set; }

    public IEnumerable<string> WorkspaceNames => this.MetaPopulation.WorkspaceNames;

    private string ValidationName
    {
        get
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                return "domain " + this.Name;
            }

            return "unknown domain";
        }
    }

    public int CompareTo(object other) => this.Id.CompareTo((other as Domain)?.Id);

    public override string ToString()
    {
        return !string.IsNullOrEmpty(this.Name) ? this.Name : this.Tag;
    }

    /// <summary>
    ///     Validates the domain.
    /// </summary>
    /// <param name="validationLog">The validation.</param>
    public void Validate(ValidationLog validationLog)
    {
        this.ValidateIdentity(validationLog);

        if (string.IsNullOrEmpty(this.Name))
        {
            validationLog.AddError("domain has no name", this, ValidationKind.Required, "Domain.Name");
        }
        else
        {
            if (!char.IsLetter(this.Name[0]))
            {
                var message = this.ValidationName + " should start with an alfabetical character";
                validationLog.AddError(message, this, ValidationKind.Format, "Domain.Name");
            }

            for (var i = 1; i < this.Name.Length; i++)
            {
                if (!char.IsLetter(this.Name[i]) && !char.IsDigit(this.Name[i]))
                {
                    var message = this.ValidationName + " should only contain alfanumerical characters)";
                    validationLog.AddError(message, this, ValidationKind.Format, "Domain.Name");
                    break;
                }
            }
        }

        if (this.Id == Guid.Empty)
        {
            validationLog.AddError(this.ValidationName + " has no id", this, ValidationKind.Required, "IMetaObject.Id");
        }
    }

    public void InitializeSuperdomains()
    {
        var superdomains = new HashSet<Domain>();
        foreach (var directSuperdomain in this.DirectSuperdomains.Cast<Domain>())
        {
            directSuperdomain.InitializeSuperdomains(this, superdomains);
        }

        this.Superdomains = superdomains.ToArray();
    }

    private void InitializeSuperdomains(Domain subdomain, ISet<Domain> superdomains)
    {
        if (this.Equals(subdomain))
        {
            // We have a cycle
            return;
        }

        superdomains.Add(this);

        foreach (var directSuperdomain in this.DirectSuperdomains.Cast<Domain>())
        {
            if (!superdomains.Contains(directSuperdomain))
            {
                directSuperdomain.InitializeSuperdomains(subdomain, superdomains);
            }
        }
    }
}
