// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the Domain type.</summary>

namespace Allors.Database.Meta;

using System;
using System.Collections.Generic;
using System.Linq;

public sealed class Domain : IDomain
{
    private string[] derivedWorkspaceNames;
    private IList<Domain> directSuperdomains;

    private string name;
    private Domain[] structuralDerivedSuperdomains;

    public Domain(MetaPopulation metaPopulation, Guid id)
    {
        this.MetaPopulation = metaPopulation;

        this.Id = id;
        this.Tag = id.Tag();

        this.directSuperdomains = new List<Domain>();

        this.MetaPopulation.OnDomainCreated(this);
    }

    public IEnumerable<Domain> DirectSuperdomains => this.directSuperdomains;

    public IEnumerable<Domain> Superdomains => this.structuralDerivedSuperdomains;

    public MetaPopulation MetaPopulation { get; }

    public string ValidationName
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

    public Guid Id { get; }

    public string Tag { get; }

    public string Name
    {
        get => this.name;

        set
        {
            this.MetaPopulation.AssertUnlocked();
            this.name = value;
            this.MetaPopulation.Stale();
        }
    }

    IEnumerable<IDomain> IDomain.DirectSuperdomains => this.directSuperdomains;

    public IEnumerable<string> WorkspaceNames
    {
        get
        {
            this.MetaPopulation.Derive();
            return this.derivedWorkspaceNames;
        }
    }

    IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

    public int CompareTo(object other) => this.Id.CompareTo((other as Domain)?.Id);

    public override bool Equals(object other) => this.Id.Equals((other as Domain)?.Id);

    public override int GetHashCode() => this.Id.GetHashCode();

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(this.Name))
        {
            return this.Name;
        }

        return this.Tag;
    }

    public void AddDirectSuperdomain(Domain superdomain) =>
        // TODO: Cyclic check
        //if (superdomain.Equals(this) || superdomain.Superdomains.Contains(this))
        //{
        //    throw new Exception("Cycle in domain inheritance");
        //}
        this.directSuperdomains.Add(superdomain);

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

    internal void Bind() => this.directSuperdomains = this.directSuperdomains.ToArray();

    internal void DeriveWorkspaceNames()
    {
        // TODO:
    }

    internal void StructuralDeriveSuperdomains(HashSet<Domain> sharedDomains)
    {
        sharedDomains.Clear();
        foreach (var directSuperdomain in this.DirectSuperdomains)
        {
            directSuperdomain.StructuralDeriveSuperdomains(this, sharedDomains);
        }

        this.structuralDerivedSuperdomains = sharedDomains.ToArray();
    }

    private void StructuralDeriveSuperdomains(Domain subdomain, HashSet<Domain> superdomains)
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
                directSuperdomain.StructuralDeriveSuperdomains(subdomain, superdomains);
            }
        }
    }
}
