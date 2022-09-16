// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReferenceOne2ManyTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters;

using System;
using System.Linq;
using Meta;
using Xunit;

public abstract class ReferenceOne2ManyTest : ReferenceSubjectTest
{
    [Fact]
    [Trait("Category", "Dynamic")]
    public void DifferentAssociation()
    {
        for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
        {
            var repeat = this.GetRepeats()[iRepeat];
            for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
            {
                var testRepeat = this.GetTestRepeats()[iTestRepeat];
                for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                {
                    var assertRepeat = this.GetAssertRepeats()[iAssertRepeat];
                    for (var iTransactionFlag = 0; iTransactionFlag < this.GetBooleanFlags().Length; iTransactionFlag++)
                    {
                        var transactionFlag = this.GetBooleanFlags()[iTransactionFlag];
                        for (var iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                        {
                            var relationType = this.GetRelations()[iRelation];
                            var emptyRoles = this.CreateArray(relationType.RoleType.ObjectType, 0);

                            // Different AssociationTypes With Same ObjectType
                            var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                            for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                            {
                                var associationType = associationTypes[iAssociationType];
                                var associations = this.CreateAssociationsWithSameClass(relationType, associationType);
                                var roleTypes = this.GetClasses(relationType);
                                for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                {
                                    var roleClass = roleTypes[iRoleType];

                                    // One Role
                                    var role = this.GetTransaction().Build(roleClass);
                                    var roles = this.CreateRoles(relationType, role);
                                    new DifferentAssociationSameRole(this).Test(
                                        relationType,
                                        associations,
                                        role,
                                        roles,
                                        emptyRoles,
                                        transactionFlag,
                                        repeat,
                                        assertRepeat,
                                        testRepeat);
                                }

                                for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                {
                                    var roleClass = roleTypes[iRoleType];

                                    // Many RoleTypes With Same ObjectType
                                    var roles = this.CreateRolesWithSameClass(relationType, roleClass);
                                    new DifferentAssociationSameRolesByOne(this).Test(
                                        relationType,
                                        associations,
                                        roles,
                                        transactionFlag,
                                        repeat,
                                        assertRepeat,
                                        testRepeat);
                                    new DifferentAssociationDifferentRolesByOne(this).Test(
                                        relationType,
                                        associations,
                                        roles,
                                        transactionFlag,
                                        repeat,
                                        testRepeat,
                                        assertRepeat);
                                    new DifferentAssociationOverlappingRolesByOne(this).Test(
                                        relationType,
                                        associations,
                                        roles,
                                        emptyRoles,
                                        transactionFlag,
                                        repeat,
                                        testRepeat,
                                        assertRepeat);
                                }

                                {
                                    // Many RoleTypes Different ObjectTypes
                                    var roles = this.CreateRolesWithDifferentClass(relationType);
                                    new DifferentAssociationSameRolesByOne(this).Test(relationType, associations, roles, transactionFlag,
                                        repeat, assertRepeat, testRepeat);
                                    new DifferentAssociationDifferentRolesByOne(this).Test(relationType, associations, roles,
                                        transactionFlag, repeat, testRepeat, assertRepeat);
                                    new DifferentAssociationOverlappingRolesByOne(this).Test(relationType, associations, roles, emptyRoles,
                                        transactionFlag, repeat, testRepeat, assertRepeat);
                                }
                            }

                            {
                                // Different Associaitons With Different ObjectTypes
                                var associations = this.CreateAssociationsWithDifferentClass(relationType);
                                var roleTypes = this.GetClasses(relationType);
                                for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                {
                                    var roleClass = roleTypes[iRoleType];

                                    // One Role
                                    var role = this.GetTransaction().Build(roleClass);
                                    var roles = this.CreateRoles(relationType, role);
                                    new DifferentAssociationSameRole(this).Test(relationType, associations, role, roles, emptyRoles,
                                        transactionFlag, repeat, assertRepeat, testRepeat);
                                }

                                for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                {
                                    var roleClass = roleTypes[iRoleType];

                                    // Many RoleTypes With Same ObjectType
                                    var roles = this.CreateRolesWithSameClass(relationType, roleClass);
                                    new DifferentAssociationSameRolesByOne(this).Test(relationType, associations, roles, transactionFlag,
                                        repeat, assertRepeat, testRepeat);
                                    new DifferentAssociationDifferentRolesByOne(this).Test(relationType, associations, roles,
                                        transactionFlag, repeat, testRepeat, assertRepeat);
                                    new DifferentAssociationOverlappingRolesByOne(this).Test(relationType, associations, roles, emptyRoles,
                                        transactionFlag, repeat, testRepeat, assertRepeat);
                                }

                                {
                                    // Many RoleTypes Different ObjectTypes
                                    var roles = this.CreateRolesWithDifferentClass(relationType);
                                    new DifferentAssociationSameRolesByOne(this).Test(relationType, associations, roles, transactionFlag,
                                        repeat, assertRepeat, testRepeat);
                                    new DifferentAssociationDifferentRolesByOne(this).Test(relationType, associations, roles,
                                        transactionFlag, repeat, testRepeat, assertRepeat);
                                    new DifferentAssociationOverlappingRolesByOne(this).Test(relationType, associations, roles, emptyRoles,
                                        transactionFlag, repeat, testRepeat, assertRepeat);
                                }
                            }
                        }
                    }

                    if (this.IsRollbackSupported())
                    {
                    }
                }
            }
        }
    }

    [Fact]
    [Trait("Category", "Dynamic")]
    public void SameAssociation()
    {
        for (var iRepeat = 0; iRepeat < this.GetRepeats().Length; iRepeat++)
        {
            var repeat = this.GetRepeats()[iRepeat];
            for (var iTestRepeat = 0; iTestRepeat < this.GetTestRepeats().Length; iTestRepeat++)
            {
                var testRepeat = this.GetTestRepeats()[iTestRepeat];
                for (var iAssertRepeat = 0; iAssertRepeat < this.GetAssertRepeats().Length; iAssertRepeat++)
                {
                    var assertRepeat = this.GetAssertRepeats()[iAssertRepeat];
                    for (var iTransactionFlag = 0; iTransactionFlag < this.GetBooleanFlags().Length; iTransactionFlag++)
                    {
                        var transactionFlag = this.GetBooleanFlags()[iTransactionFlag];
                        for (var iRelation = 0; iRelation < this.GetRelations().Length; iRelation++)
                        {
                            var relationType = this.GetRelations()[iRelation];
                            var emptyRoles = this.CreateArray(relationType.RoleType.ObjectType, 0);

                            var associationTypes = relationType.AssociationType.ObjectType.Classes.ToArray();
                            for (var iAssociationType = 0; iAssociationType < associationTypes.Count(); iAssociationType++)
                            {
                                var associationType = associationTypes[iAssociationType];
                                var roleTypes = this.GetClasses(relationType);
                                for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                {
                                    var roleType = roleTypes[iRoleType];

                                    // One Role
                                    var association = this.GetTransaction().Build(associationType);
                                    var role = this.GetTransaction().Build(roleType);
                                    var allRoles = this.CreateRoles(relationType, role);
                                    new SameAssociationSameRole(this).Test(
                                        relationType,
                                        association,
                                        role,
                                        allRoles,
                                        emptyRoles,
                                        transactionFlag,
                                        repeat,
                                        testRepeat,
                                        assertRepeat);
                                }

                                for (var iRoleType = 0; iRoleType < roleTypes.Count(); iRoleType++)
                                {
                                    var roleClass = roleTypes[iRoleType];

                                    // Many RoleTypes With Same ObjectType
                                    var association = this.GetTransaction().Build(associationType);
                                    var allRoles = this.CreateRolesWithSameClass(relationType, roleClass);
                                    var rolesOtherDatabase =
                                        this.CreateRolesWithSameClass(this.GetTransaction2(), relationType, roleClass);
                                    new SameAssociationSameRolesByOne(this).Test(
                                        relationType,
                                        association,
                                        allRoles,
                                        emptyRoles,
                                        rolesOtherDatabase,
                                        transactionFlag,
                                        repeat,
                                        testRepeat,
                                        assertRepeat);
                                    new SameAssociationSameRolesByAll(this).Test(
                                        relationType,
                                        association,
                                        allRoles,
                                        emptyRoles,
                                        transactionFlag,
                                        repeat,
                                        testRepeat,
                                        assertRepeat);
                                }

                                {
                                    // Many RoleTypes Different ObjectTypes
                                    var association = this.GetTransaction().Build(associationType);
                                    var allRoles = this.CreateRolesWithDifferentClass(relationType);
                                    var rolesOtherDatabase = this.CreateRolesWithDifferentClass(this.GetTransaction2(), relationType);
                                    new SameAssociationSameRolesByOne(this).Test(relationType, association, allRoles, emptyRoles,
                                        rolesOtherDatabase, transactionFlag, repeat, testRepeat, assertRepeat);
                                    new SameAssociationSameRolesByAll(this).Test(relationType, association, allRoles, emptyRoles,
                                        transactionFlag, repeat, testRepeat, assertRepeat);
                                }
                            }
                        }
                    }

                    if (this.IsRollbackSupported())
                    {
                    }
                }
            }
        }
    }

    private IObject[] CreateAssociationsWithDifferentClass(IRelationType relationType)
    {
        var associations = this.CreateArray(relationType.AssociationType.ObjectType, this.GetAssociationCount());
        var concreteClasses = relationType.AssociationType.ObjectType.Classes.ToArray();
        for (var i = 0; i < associations.Count(); i++)
        {
            var classIndex = i % concreteClasses.Count();
            var associationType = concreteClasses[classIndex];
            associations[i] = this.GetTransaction().Build(associationType);
        }

        return associations;
    }

    private IObject[] CreateAssociationsWithSameClass(IRelationType relationType, IClass associationClass)
    {
        var associations = this.CreateArray(relationType.AssociationType.ObjectType, this.GetAssociationCount());
        for (var i = 0; i < this.GetAssociationCount(); i++)
        {
            associations[i] = this.GetTransaction().Build(associationClass);
        }

        return associations;
    }

    private IObject[] CreateRoles(IRelationType relationType, IObject role)
    {
        var roles = this.CreateArray(relationType.RoleType.ObjectType, 1);
        roles[0] = role;
        return roles;
    }

    private IObject[] CreateRolesWithDifferentClass(IRelationType relationType) =>
        this.CreateRolesWithDifferentClass(this.GetTransaction(), relationType);

    private IObject[] CreateRolesWithDifferentClass(ITransaction transaction, IRelationType relationType)
    {
        var allRoles = this.CreateArray(relationType.RoleType.ObjectType, this.GetRoleCount());
        var concreteClasses = this.GetClasses(relationType);
        for (var i = 0; i < allRoles.Count(); i++)
        {
            var classIndex = i % concreteClasses.Count();
            var roleType = concreteClasses[classIndex];
            allRoles[i] = transaction.Build(roleType);
        }

        return allRoles;
    }

    private IObject[] CreateRolesWithSameClass(IRelationType relationType, IClass roleClass) =>
        this.CreateRolesWithSameClass(this.GetTransaction(), relationType, roleClass);

    private IObject[] CreateRolesWithSameClass(ITransaction transaction, IRelationType relationType, IClass roleClass)
    {
        var allRoles = this.CreateArray(relationType.RoleType.ObjectType, this.GetRoleCount());
        for (var i = 0; i < allRoles.Count(); i++)
        {
            allRoles[i] = transaction.Build(roleClass);
        }

        return allRoles;
    }

    private IRelationType[] GetRelations() => this.GetOne2ManyRelations(this.GetMetaPopulation());

    private class DifferentAssociationDifferentRolesByOne : ReferenceProxyTest
    {
        public DifferentAssociationDifferentRolesByOne(ReferenceTest referenceTest) : base(referenceTest)
        {
        }

        public void Test(IRelationType relationType, IObject[] associations, IObject[] roles, bool transactionFlag, int repeat,
            int testRepeat, int assertRepeat)
        {
            this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, assertRepeat, testRepeat);

            // Add Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                var role = roles[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardExists(relationType, associations, roles, i, transactionFlag, testRepeat);
                }
            }

            // Remove Forward
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                var role = roles[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardNotExists(relationType, associations, roles, i, transactionFlag, testRepeat);
                }
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                var role = roles[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardExists(relationType, associations, roles, i, transactionFlag, testRepeat);
                }
            }

            // Remove Backward
            for (var i = associations.Count() - 1; i >= 0; i--)
            {
                var association = associations[i];
                var role = roles[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationBackwardNotExists(relationType, associations, roles, i, transactionFlag, testRepeat);
                }
            }
        }

        private void AssertRelationNotExists(IRelationType relationType, IObject[] associations, IObject[] roles, bool transactionFlag,
            int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var iAssociation = 0; iAssociation < associations.Count(); iAssociation++)
                {
                    var association = associations[iAssociation];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Empty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                    }
                }

                for (var iRole = 0; iRole < roles.Count(); iRole++)
                {
                    var testRole = roles[iRole];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }
            }
        }

        private void AssertRelationForwardExists(IRelationType relationType, IObject[] associations, IObject[] roles, int index,
            bool transactionFlag, int testRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < associations.Count(); k++)
                {
                    var testAssociation = associations[k];
                    var testRole = roles[k];
                    if (k <= index)
                    {
                        Assert.Single((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Contains(testRole, (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                    else
                    {
                        Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.DoesNotContain(testRole,
                            (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationForwardNotExists(IRelationType relationType, IObject[] associations, IObject[] roles, int index,
            bool transactionFlag, int testRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < associations.Count(); k++)
                {
                    var testAssociation = associations[k];
                    var testRole = roles[k];
                    if (k <= index)
                    {
                        Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.DoesNotContain(testRole,
                            (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                    else
                    {
                        Assert.Single((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Contains(testRole, (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationBackwardNotExists(IRelationType relationType, IObject[] associations, IObject[] roles, int index,
            bool transactionFlag, int testRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < associations.Count(); k++)
                {
                    var testAssociation = associations[k];
                    var testRole = roles[k];
                    if (k >= index)
                    {
                        Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.DoesNotContain(testRole,
                            (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                    else
                    {
                        Assert.Single((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Contains(testRole, (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                this.Commit(transactionFlag);
            }
        }
    }

    private class DifferentAssociationOverlappingRolesByOne : ReferenceProxyTest
    {
        public DifferentAssociationOverlappingRolesByOne(ReferenceTest referenceTest) : base(referenceTest)
        {
        }

        public void Test(IRelationType relationType, IObject[] associations, IObject[] roles, IObject[] emptyRoles, bool transactionFlag,
            int repeat, int testRepeat, int assertRepeat)
        {
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                this.AssertRoleNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRoleNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRoleNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationForwardExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Forward
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    var role = roles[j];
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        association.Strategy.RemoveCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationForwardNotExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRoleNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationForwardExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Backward
            for (var i = associations.Count() - 1; i >= 0; i--)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    var role = roles[j];
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        association.Strategy.RemoveCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationBackwardNotExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRoleNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationForwardExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove All
            for (var i = associations.Count() - 1; i >= 0; i--)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        association.Strategy.RemoveCompositesRole(relationType.RoleType);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationBackwardNotExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationForwardExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Set Null
            for (var i = associations.Count() - 1; i >= 0; i--)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        association.Strategy.SetCompositesRole(relationType.RoleType, null);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationBackwardNotExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationForwardExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Set Empty
            for (var i = associations.Count() - 1; i >= 0; i--)
            {
                var association = associations[i];
                for (var j = i; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        association.Strategy.SetCompositesRole(relationType.RoleType, emptyRoles);
                        this.Commit(transactionFlag);
                    }
                }

                this.AssertRelationBackwardNotExists(relationType, associations, i, roles, transactionFlag, testRepeat, assertRepeat);
            }
        }

        private void AssertRoleNotExists(IRelationType relationType, IObject[] associations, IObject[] roles, bool transactionFlag,
            int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var iAssociation = 0; iAssociation < associations.Count(); iAssociation++)
                {
                    var association = associations[iAssociation];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Empty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                    }
                }

                for (var iRole = 0; iRole < roles.Count(); iRole++)
                {
                    var testRole = roles[iRole];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }
            }
        }

        private void AssertRelationExists(IRelationType relationType, IObject[] associations, IObject[] roles, bool transactionFlag,
            int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < associations.Count(); k++)
                {
                    var testAssociation = associations[k];
                    if (k < associations.Count() - 1)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.NotEmpty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var l = 0; l < roles.Count(); l++)
                        {
                            var testRole = roles[l];
                            if (l == k)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.NotEqual(testAssociation,
                                        testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.NotEmpty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var l = 0; l < roles.Count(); l++)
                        {
                            var testRole = roles[l];
                            if (l >= k)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.NotEqual(testAssociation,
                                        testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationForwardExists(IRelationType relationType, IObject[] associations, int associationIndex, IObject[] roles,
            bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < associations.Count(); k++)
                {
                    var testAssociation = associations[k];
                    if (k < associationIndex)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.NotEmpty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var l = 0; l < roles.Count(); l++)
                        {
                            var testRole = roles[l];
                            if (l == k)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.NotEqual(testAssociation,
                                        testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                    else if (k == associationIndex)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.NotEmpty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var l = 0; l < roles.Count(); l++)
                        {
                            var testRole = roles[l];
                            if (l >= k)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.NotEqual(testAssociation,
                                        testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationForwardNotExists(IRelationType relationType, IObject[] associations, int associationIndex,
            IObject[] roles, bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < associations.Count(); k++)
                {
                    var testAssociation = associations[k];

                    if (k <= associationIndex)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.NotEmpty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        if (k == associations.Count() - 1)
                        {
                            for (var l = 0; l < roles.Count(); l++)
                            {
                                var testRole = roles[l];
                                if (l >= k)
                                {
                                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                    {
                                        Assert.Contains(testRole,
                                            (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                    }

                                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                    {
                                        Assert.Equal(testAssociation,
                                            testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                    }
                                }
                                else
                                {
                                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                    {
                                        Assert.DoesNotContain(testRole,
                                            (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                    }

                                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                    {
                                        Assert.NotEqual(testAssociation,
                                            testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (var l = 0; l < roles.Count(); l++)
                            {
                                var testRole = roles[l];
                                if (l == k)
                                {
                                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                    {
                                        Assert.Contains(testRole,
                                            (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                    }

                                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                    {
                                        Assert.Equal(testAssociation,
                                            testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                    }
                                }
                                else
                                {
                                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                    {
                                        Assert.DoesNotContain(testRole,
                                            (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                    }

                                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                    {
                                        Assert.NotEqual(testAssociation,
                                            testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                    }
                                }
                            }
                        }
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationBackwardNotExists(IRelationType relationType, IObject[] associations, int associationIndex,
            IObject[] roles, bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < associations.Count(); k++)
                {
                    var testAssociation = associations[k];

                    if (k >= associationIndex)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.NotEmpty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var l = 0; l < roles.Count(); l++)
                        {
                            var testRole = roles[l];
                            if (l == k)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.NotEqual(testAssociation,
                                        testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                }

                this.Commit(transactionFlag);
            }
        }
    }

    private class DifferentAssociationSameRole : ReferenceProxyTest
    {
        public DifferentAssociationSameRole(ReferenceTest referenceTest) : base(referenceTest)
        {
        }

        public void Test(IRelationType relationType, IObject[] associations, IObject role, IObject[] roles, IObject[] emptyRoles,
            bool transactionFlag, int repeat, int testRepeat, int assertRepeat)
        {
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                this.AssertRelationNotExists(relationType, associations, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Add Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationNotExists(relationType, associations, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                var association = associations[this.GetAssociationCount() - 1];
                association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, associations, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);
                }

                this.AssertExclusiveRelation(relationType, associations, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Add Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertExclusiveRelation(relationType, associations, associations[associations.Count() - 1], role, transactionFlag,
                    testRepeat, assertRepeat);
            }

            // Remove Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                var association = associations[this.GetAssociationCount() - 1];
                association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);
                this.AssertExclusiveRelation(relationType, associations, associations[associations.Count() - 1], role, transactionFlag,
                    testRepeat, assertRepeat);
            }

            // Remove
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                associations[this.GetAssociationCount() - 1].Strategy.RemoveCompositesRole(relationType.RoleType, role);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, associations, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationNotExists(relationType, associations, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Set All
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                    this.Commit(transactionFlag);
                }

                this.AssertExclusiveRelation(relationType, associations, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove All
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                associations[this.GetAssociationCount() - 1].Strategy.RemoveCompositesRole(relationType.RoleType);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, associations, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Set
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                    this.Commit(transactionFlag);
                }

                this.AssertExclusiveRelation(relationType, associations, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Set Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                associations[this.GetAssociationCount() - 1].Strategy.SetCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, associations, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Set
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                    this.Commit(transactionFlag);
                }

                this.AssertExclusiveRelation(relationType, associations, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            // Set Empty
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                associations[this.GetAssociationCount() - 1].Strategy.SetCompositesRole(relationType.RoleType, emptyRoles);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, associations, role, transactionFlag, testRepeat, assertRepeat);
            }
        }

        private void AssertRelationNotExists(IRelationType relationType, IObject[] associations, IObject role, bool transactionFlag,
            int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var iAssociation = 0; iAssociation < associations.Count(); iAssociation++)
                {
                    var association = associations[iAssociation];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Empty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                    }
                }

                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                }

                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }
            }
        }

        private void AssertExclusiveRelation(IRelationType relationType, IObject[] associations, IObject association, IObject role,
            bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Contains(role, (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Single((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var iAssociation = 0; iAssociation < associations.Count(); iAssociation++)
                {
                    var testAssociation = associations[iAssociation];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        if (testAssociation.Equals(association))
                        {
                            Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                        }
                        else
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }
                    }
                }

                this.Commit(transactionFlag);
            }
        }
    }

    private class DifferentAssociationSameRolesByOne : ReferenceProxyTest
    {
        public DifferentAssociationSameRolesByOne(ReferenceTest referenceTest) : base(referenceTest)
        {
        }

        public void Test(IRelationType relationType, IObject[] associations, IObject[] roles, bool transactionFlag, int repeat,
            int testRepeat, int assertRepeat)
        {
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = 0; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);

                        this.AssertRelationForwardExists(relationType, associations, i, roles, j, transactionFlag, testRepeat,
                            assertRepeat);
                    }
                }
            }

            // Add Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationExists(relationType, associations, associations[associations.Count() - 1], roles, transactionFlag,
                    testRepeat, assertRepeat);
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationExists(relationType, associations, associations[associations.Count() - 1], roles, transactionFlag,
                    testRepeat, assertRepeat);
            }

            // Remove Forward
            for (var i = 0; i < roles.Count(); i++)
            {
                var association = associations[associations.Count() - 1];
                var role = roles[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardNotExists(relationType, associations, association, roles, i, transactionFlag, testRepeat,
                        assertRepeat);
                }
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = 0; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);

                        this.AssertRelationForwardExists(relationType, associations, i, roles, j, transactionFlag, testRepeat,
                            assertRepeat);
                    }
                }
            }

            // Remove Backward
            for (var i = roles.Count() - 1; i >= 0; i--)
            {
                var association = associations[associations.Count() - 1];
                var role = roles[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationBackwardNotExists(relationType, associations, association, roles, i, transactionFlag, testRepeat,
                        assertRepeat);
                }
            }

            // Remove Null
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                    this.Commit(transactionFlag);
                }

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove All
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                var association = associations[associations.Count() - 1];
                association.Strategy.RemoveCompositesRole(relationType.RoleType);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = 0; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);

                        this.AssertRelationForwardExists(relationType, associations, i, roles, j, transactionFlag, testRepeat,
                            assertRepeat);
                    }
                }
            }

            // Set Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                var association = associations[associations.Count() - 1];
                association.Strategy.SetCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var i = 0; i < associations.Count(); i++)
            {
                var association = associations[i];
                for (var j = 0; j < roles.Count(); j++)
                {
                    for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                    {
                        var role = roles[j];
                        association.Strategy.AddCompositesRole(relationType.RoleType, role);
                        this.Commit(transactionFlag);

                        this.AssertRelationForwardExists(relationType, associations, i, roles, j, transactionFlag, testRepeat,
                            assertRepeat);
                    }
                }
            }

            // Set Empty
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                var association = associations[associations.Count() - 1];
                association.Strategy.SetCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, associations, roles, transactionFlag, testRepeat, assertRepeat);
            }
        }

        private void AssertRelationNotExists(IRelationType relationType, IObject[] associations, IObject[] roles, bool transactionFlag,
            int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var iAssociation = 0; iAssociation < associations.Count(); iAssociation++)
                {
                    var association = associations[iAssociation];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Empty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                    }
                }

                for (var iRole = 0; iRole < roles.Count(); iRole++)
                {
                    var testRole = roles[iRole];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                if (transactionFlag)
                {
                    this.GetTransaction().Commit();
                }
            }
        }

        private void AssertRelationExists(IRelationType relationType, IObject[] associations, IObject association, IObject[] roles,
            bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var iAssociation = 0; iAssociation < associations.Count(); iAssociation++)
                {
                    var testAssociation = associations[iAssociation];
                    if (testAssociation.Equals(association))
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Equal(this.GetRoleCount(),
                                ((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType)).Count());
                        }

                        for (var iRole = 0; iRole < roles.Count(); iRole++)
                        {
                            var role = roles[iRole];
                            for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                            {
                                Assert.Contains(role, (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                            }

                            for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                            {
                                Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                            }
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationForwardExists(IRelationType relationType, IObject[] associations, int associationIndex, IObject[] roles,
            int roleIndex, bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < associations.Count(); k++)
                {
                    var testAssociation = associations[k];
                    if (k == associationIndex)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.NotEmpty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var l = 0; l < roles.Count(); l++)
                        {
                            var testRole = roles[l];
                            if (l <= roleIndex)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.NotEqual(testAssociation,
                                        testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                    else if (k + 1 == associationIndex)
                    {
                        for (var l = 0; l < roles.Count(); l++)
                        {
                            var testRole = roles[l];
                            if (l <= roleIndex)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.NotEqual(testAssociation,
                                        testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationForwardNotExists(IRelationType relationType, IObject[] associations, IObject association,
            IObject[] roles, int roleIndex, bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var iAssociation = 0; iAssociation < associations.Count(); iAssociation++)
                {
                    var testAssociation = associations[iAssociation];
                    if (association.Equals(testAssociation))
                    {
                        for (var k = 0; k < roles.Count(); k++)
                        {
                            var testRole = roles[k];
                            if (k <= roleIndex)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var k = 0; k < roles.Count(); k++)
                        {
                            var testRole = roles[k];
                            for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                            {
                                Assert.NotEqual(testRole, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                            }
                        }
                    }
                }
            }

            this.Commit(transactionFlag);
        }

        private void AssertRelationBackwardNotExists(IRelationType relationType, IObject[] associations, IObject association,
            IObject[] roles, int roleIndex, bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var iAssociation = 0; iAssociation < associations.Count(); iAssociation++)
                {
                    var testAssociation = associations[iAssociation];
                    if (association.Equals(testAssociation))
                    {
                        for (var k = 0; k < roles.Count(); k++)
                        {
                            var testRole = roles[k];
                            if (k >= roleIndex)
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.DoesNotContain(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                            else
                            {
                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Contains(testRole,
                                        (IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                                }

                                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                                {
                                    Assert.Equal(testAssociation, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Empty((IObject[])testAssociation.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var k = 0; k < roles.Count(); k++)
                        {
                            var testRole = roles[k];
                            for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                            {
                                Assert.NotEqual(testRole, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                            }
                        }
                    }
                }
            }

            this.Commit(transactionFlag);
        }
    }

    private class SameAssociationSameRole : ReferenceProxyTest
    {
        public SameAssociationSameRole(ReferenceTest referenceTest) : base(referenceTest)
        {
        }

        public void Test(IRelationType relationType, IObject association, IObject role, IObject[] roles, IObject[] emptyRoles,
            bool transactionFlag, int repeat, int testRepeat, int assertRepeat)
        {
            this.AssertRelationNotExists(relationType, association, role, transactionFlag, testRepeat, assertRepeat);

            // Add
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, role);
                this.Commit(transactionFlag);
                this.AssertRelation(relationType, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.RemoveCompositesRole(relationType.RoleType);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                this.Commit(transactionFlag);
                this.AssertRelation(relationType, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.RemoveCompositesRole(relationType.RoleType);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                this.Commit(transactionFlag);
                this.AssertRelation(relationType, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, role, transactionFlag, testRepeat, assertRepeat);
            }

            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, emptyRoles);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, role, transactionFlag, testRepeat, assertRepeat);
            }
        }

        private void AssertRelation(IRelationType relationType, IObject association, IObject role, bool transactionFlag, int testRepeat,
            int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Single((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Contains(role, (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Equal(association, role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationNotExists(IRelationType relationType, IObject association, IObject role, bool transactionFlag,
            int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Empty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Null(role.Strategy.GetCompositeAssociation(relationType.AssociationType));
                }

                this.Commit(transactionFlag);
            }
        }
    }

    private class SameAssociationSameRolesByAll : ReferenceProxyTest
    {
        public SameAssociationSameRolesByAll(ReferenceTest referenceTest) : base(referenceTest)
        {
        }

        public void Test(IRelationType relationType, IObject association, IObject[] roles, IObject[] emptyRoles, bool transactionFlag,
            int repeat, int testRepeat, int assertRepeat)
        {
            this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);

            // Add Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Add Empty
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Set
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add Empty
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.RemoveCompositesRole(relationType.RoleType);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Set
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove All
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.RemoveCompositesRole(relationType.RoleType);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove All
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.RemoveCompositesRole(relationType.RoleType);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Set
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Set Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Set
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, roles);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Set Empty Array
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, emptyRoles);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // TODO: (Exist)
        }

        private void AssertRelationNotExists(IRelationType relationType, IObject association, IObject[] roles, bool transactionFlag,
            int assertRepeat, int testRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Empty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var iRole = 0; iRole < roles.Count(); iRole++)
                {
                    var testRole = roles[iRole];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationExists(IRelationType relationType, IObject association, IObject[] roles, bool transactionFlag,
            int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.NotEmpty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var k = 0; k < roles.Count(); k++)
                {
                    var testRole = roles[k];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Contains(testRole, (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                    }

                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Equal(association, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                this.Commit(transactionFlag);
            }
        }
    }

    private class SameAssociationSameRolesByOne : ReferenceProxyTest
    {
        public SameAssociationSameRolesByOne(ReferenceTest referenceTest) : base(referenceTest)
        {
        }

        public void Test(IRelationType relationType, IObject association, IObject[] roles, IObject[] emptyRoles,
            IObject[] rolesOtherDatabase, bool transactionFlag, int repeat, int testRepeat, int assertRepeat)
        {
            this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);

            // Add Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Add Empty
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Add
            for (var i = 0; i < roles.Count(); i++)
            {
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    var role = roles[i];
                    association.Strategy.AddCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardExists(relationType, association, roles, i, transactionFlag, testRepeat, assertRepeat);
                }
            }

            // Add Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Add Empty
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.AddCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.RemoveCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationExists(relationType, association, roles, transactionFlag, testRepeat, assertRepeat);
            }

            // Remove Forwards
            for (var i = 0; i < roles.Count(); i++)
            {
                var role = roles[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardNotExists(relationType, association, roles, i, transactionFlag, testRepeat, assertRepeat);
                }
            }

            this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);

            // Add
            for (var i = 0; i < roles.Count(); i++)
            {
                var role = roles[i];
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardExists(relationType, association, roles, i, transactionFlag, testRepeat, assertRepeat);
                }
            }

            // Remove Backwards
            for (var i = roles.Count() - 1; i >= 0; i--)
            {
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    var role = roles[i];
                    association.Strategy.RemoveCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationBackwardsNotExists(relationType, association, roles, i, transactionFlag, testRepeat, assertRepeat);
                }
            }

            this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);

            // Add
            for (var i = 0; i < roles.Count(); i++)
            {
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    var role = roles[i];
                    association.Strategy.AddCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardExists(relationType, association, roles, i, transactionFlag, testRepeat, assertRepeat);
                }
            }

            // RemoveAll
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.RemoveCompositesRole(relationType.RoleType);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Add
            for (var i = 0; i < roles.Count(); i++)
            {
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    var role = roles[i];
                    association.Strategy.AddCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardExists(relationType, association, roles, i, transactionFlag, testRepeat, assertRepeat);
                }
            }

            // Set Null
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, null);
                this.Commit(transactionFlag);

                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Add
            for (var i = 0; i < roles.Count(); i++)
            {
                for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
                {
                    var role = roles[i];
                    association.Strategy.AddCompositesRole(relationType.RoleType, role);
                    this.Commit(transactionFlag);

                    this.AssertRelationForwardExists(relationType, association, roles, i, transactionFlag, testRepeat, assertRepeat);
                }
            }

            // Set Empty Array
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                association.Strategy.SetCompositesRole(relationType.RoleType, emptyRoles);
                this.Commit(transactionFlag);
                this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
            }

            // Add different Population
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                try
                {
                    association.Strategy.AddCompositesRole(relationType.RoleType, rolesOtherDatabase[0]);
                    Assert.True(false); // Fail
                }
                catch (ArgumentException exception)
                {
                    Assert.NotNull(exception);
                }

                this.Commit(transactionFlag);

                for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                {
                    this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
                }
            }

            // Set different Population
            for (var repeatIndex = 0; repeatIndex < repeat; repeatIndex++)
            {
                try
                {
                    association.Strategy.SetCompositesRole(relationType.RoleType, rolesOtherDatabase);
                    Assert.True(false); // Fail
                }
                catch (ArgumentException exception)
                {
                    Assert.NotNull(exception);
                }

                this.Commit(transactionFlag);

                for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
                {
                    this.AssertRelationNotExists(relationType, association, roles, transactionFlag, assertRepeat, testRepeat);
                }
            }

            // TODO: (Exist)
        }

        private void AssertRelationNotExists(IRelationType relationType, IObject association, IObject[] roles, bool transactionFlag,
            int assertRepeat, int testRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.Empty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var iRole = 0; iRole < roles.Count(); iRole++)
                {
                    var testRole = roles[iRole];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationExists(IRelationType relationType, IObject association, IObject[] roles, bool transactionFlag,
            int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.NotEmpty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var k = 0; k < roles.Count(); k++)
                {
                    var testRole = roles[k];
                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Contains(testRole, (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                    }

                    for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                    {
                        Assert.Equal(association, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationForwardExists(IRelationType relationType, IObject association, IObject[] roles, int roleIndex,
            bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                {
                    Assert.NotEmpty((IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                }

                for (var k = 0; k < roles.Count(); k++)
                {
                    var testRole = roles[k];
                    if (k <= roleIndex)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Contains(testRole, (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Equal(association, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.DoesNotContain(testRole,
                                (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                        }
                    }
                }

                this.Commit(transactionFlag);
            }
        }

        private void AssertRelationForwardNotExists(IRelationType relationType, IObject association, IObject[] roles, int roleIndex,
            bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < roles.Count(); k++)
                {
                    var testRole = roles[k];
                    if (k <= roleIndex)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.DoesNotContain(testRole,
                                (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Contains(testRole, (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Equal(association, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                        }
                    }
                }
            }

            this.Commit(transactionFlag);
        }

        private void AssertRelationBackwardsNotExists(IRelationType relationType, IObject association, IObject[] roles, int roleIndex,
            bool transactionFlag, int testRepeat, int assertRepeat)
        {
            for (var testRepeatIndex = 0; testRepeatIndex < testRepeat; testRepeatIndex++)
            {
                for (var k = 0; k < roles.Count(); k++)
                {
                    var testRole = roles[k];
                    if (k >= roleIndex)
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.DoesNotContain(testRole,
                                (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Null(testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                        }
                    }
                    else
                    {
                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Contains(testRole, (IObject[])association.Strategy.GetCompositesRole<IObject>(relationType.RoleType));
                        }

                        for (var assertRepeateIndex = 0; assertRepeateIndex < assertRepeat; assertRepeateIndex++)
                        {
                            Assert.Equal(association, testRole.Strategy.GetCompositeAssociation(relationType.AssociationType));
                        }
                    }
                }
            }

            this.Commit(transactionFlag);
        }
    }
}
