// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class Domain : MetaIdentifiableObject, IDomain
{
    public Domain(MetaPopulation metaPopulation, Guid id, string name, params Domain[] directSuperdomains)
        : base(metaPopulation, id)
    {
        this.Name = name;
        this.DirectSuperdomains = directSuperdomains ?? Array.Empty<Domain>();

        this.MetaPopulation.OnCreated(this);
    }

    public string Name { get; }

    IEnumerable<IDomain> IDomain.DirectSuperdomains => this.DirectSuperdomains;

    public Domain[] DirectSuperdomains { get; }

    IEnumerable<IDomain> IDomain.Superdomains => this.Superdomains;

    public Domain[] Superdomains { get; private set; }

    public override IEnumerable<string> WorkspaceNames => this.MetaPopulation.WorkspaceNames;

    internal string ValidationName
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

    public override bool Equals(object other) => this.Id.Equals((other as Domain)?.Id);

    public override int GetHashCode() => this.Id.GetHashCode();

    public override string ToString()
    {
        return !string.IsNullOrEmpty(this.Name) ? this.Name : this.Tag;
    }

    /// <summary>
    ///     Validates the domain.
    /// </summary>
    /// <param name="validationLog">The validation.</param>
    internal void Validate(ValidationLog validationLog)
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

    internal void InitializeSuperdomains(HashSet<Domain> sharedDomains)
    {
        sharedDomains.Clear();
        foreach (var directSuperdomain in this.DirectSuperdomains)
        {
            directSuperdomain.InitializeSuperdomains(this, sharedDomains);
        }

        this.Superdomains = sharedDomains.ToArray();
    }

    private void InitializeSuperdomains(Domain subdomain, HashSet<Domain> superdomains)
    {
        if (this.Equals(subdomain))
        {
            // We have a cycle
            return;
        }

        superdomains.Add(this);

        foreach (var directSuperdomain in this.DirectSuperdomains)
        {
            if (!superdomains.Contains(directSuperdomain))
            {
                directSuperdomain.InitializeSuperdomains(subdomain, superdomains);
            }
        }
    }
}
