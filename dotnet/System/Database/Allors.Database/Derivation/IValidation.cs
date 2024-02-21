// <copyright file="IDomainDerivation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IDomainDerivation type.</summary>

namespace Allors.Database.Derivations;

using Allors.Database.Meta;

public interface IValidation
{
    bool HasErrors { get; }

    IDerivationError[] Errors { get; }

    void AddError(IDerivationError derivationError);

    void AddError(IObject association, RoleType roleType, string errorCode);

    void AddError(IObject role, AssociationType associationType, string errorCode);

    void AddError(string errorCode);

    void AssertExists(IObject association, RoleType roleType);

    void AssertNotExists(IObject association, RoleType roleType);

    void AssertNonEmptyString(IObject association, RoleType roleType);

    void AssertNonWhiteSpaceString(IObject association, RoleType roleType);
    
    void AssertExistsNonEmptyString(IObject association, RoleType roleType);

    void AssertIsUnique(IChangeSet changeSet, IObject association, RoleType roleType);

    void AssertIsUnique(IChangeSet changeSet, IObject association, IComposite objectType, params RoleType[] roleTypes);

    void AssertAtLeastOne(IObject association, params RoleType[] roleTypes);

    void AssertExistsAtMostOne(IObject association, params RoleType[] roleTypes);

    void AssertAreEqual(IObject association, RoleType roleType, RoleType otherRoleType);

    void AssertExists(IObject role, AssociationType associationType);
}
