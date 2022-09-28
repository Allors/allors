// <copyright file="Organization.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta.Configuration
{
    using System;
    using System.Linq;

    public partial class MetaBuilder
    {
        static void AddWorkspace(Class @class, params string[] workspaceNames) => @class.AssignedWorkspaceNames = (@class.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        static void AddWorkspace(MethodType methodType, params string[] workspaceNames) => methodType.AssignedWorkspaceNames = (methodType.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        static void AddWorkspace(RelationType relationType, params string[] workspaceNames) => relationType.AssignedWorkspaceNames = (relationType.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        private void BuildCustom(MetaPopulation m, Domains domains, RelationTypes relationTypes, MethodTypes methodTypes)
        {
            // RelationTypes & MethodTypes
            // Deletable
            AddWorkspace(methodTypes.DeletableDelete, new[] { "Default" });

            // Object

            // UniquelyIdentifiable
            AddWorkspace(relationTypes.UniquelyIdentifiableUniqueId, new[] { "Default" });

            // DelegatedAccess

            // Grant

            // Login

            // CreatePermission

            // ExecutePermission

            // Permission

            // ReadPermission

            // WritePermission

            // Revocation

            // Role

            // SecurityToken

            // SecurityTokenOwner

            // User
            AddWorkspace(relationTypes.UserUserName, new[] { "Default" });


            AddWorkspace(relationTypes.UserUserEmail, new[] { "Default" });











            // UserGroup
            AddWorkspace(relationTypes.UserGroupMembers, new[] { "Default" });
            AddWorkspace(relationTypes.UserGroupName, new[] { "Default" });

            // UserPasswordReset
            AddWorkspace(relationTypes.UserPasswordResetInExistingUserPassword, new[] { "Default" });
            AddWorkspace(relationTypes.UserPasswordResetInUserPassword, new[] { "Default" });

            // C1
            AddWorkspace(relationTypes.C1C1AllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DateTimeLessThan, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DateTimeGreaterThan, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DateTimeBetweenA, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DateTimeBetweenB, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DecimalLessThan, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DecimalGreaterThan, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DecimalBetweenA, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DecimalBetweenB, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DoubleLessThan, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DoubleGreaterThan, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DoubleBetweenA, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1DoubleBetweenB, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1IntegerLessThan, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1IntegerGreaterThan, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1IntegerBetweenA, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1IntegerBetweenB, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsStringEquals, new[] { "Default" });
            AddWorkspace(relationTypes.C1AllorsStringMax, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsUnique, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1C1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1C1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1C1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1C1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1C2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1C2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1C2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1C2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I12Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I12Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I12One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I12One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1I2One2One, new[] { "Default" });
            AddWorkspace(methodTypes.C1ClassMethod, new[] { "Default" });
            AddWorkspace(methodTypes.C1Sum, new[] { "Default" });

            // C2
            AddWorkspace(relationTypes.C2C2AllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsUnique, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I12Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I12One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I12Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I12One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.C2S1One2One, new[] { "Default" });

            // AA

            // BB

            // CC

            // ValidationC1

            // ValidationC2

            // ValidationI12

            // DerivationCounted

            // I1
            AddWorkspace(relationTypes.I1I1I1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I12Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1AllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I12Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1AllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1C2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1C1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1AllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1C2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1AllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1AllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I12One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1C2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1C1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1AllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1C1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1AllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1C1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1I12One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1C2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I1I1AllorsUnique, new[] { "Default" });

            // I12
            AddWorkspace(relationTypes.I12I12AllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12C2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12AllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12AllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I12Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12AllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12C2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I12One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12Name, new[] { "Default" });
            AddWorkspace(relationTypes.I12Order, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12C1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12AllorsUnique, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12AllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12C1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I12One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12Dependencies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12C2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I12Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12AllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12I1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12C1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12C1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I12I12AllorsDateTime, new[] { "Default" });

            // I2
            AddWorkspace(relationTypes.I2I2I2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2C1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I12Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2AllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2C1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2C1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2AllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2AllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2AllorsUnique, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2AllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I12One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I12One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2C2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2C2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2AllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2C2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I12Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2AllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2I2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2C1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2C2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.I2I2AllorsDouble, new[] { "Default" });

            // S1

            // S12

            // AccessClass
            AddWorkspace(relationTypes.AccessClassProperty, new[] { "Default" });
            AddWorkspace(relationTypes.AccessClassAnotherProperty, new[] { "Default" });

            // AccessInterface

            // Denied
            AddWorkspace(relationTypes.DeniedDefaultWorkspaceProperty, new[] { "Default" });
            AddWorkspace(relationTypes.DeniedWorkspaceXProperty, new[] { "X" });

            // Build

            // ClassWithoutRoles

            // Data
            AddWorkspace(relationTypes.DataAutocompleteFilter, new[] { "Default" });
            AddWorkspace(relationTypes.DataAutocompleteOptions, new[] { "Default" });
            AddWorkspace(relationTypes.DataCheckbox, new[] { "Default" });
            AddWorkspace(relationTypes.DataChips, new[] { "Default" });
            AddWorkspace(relationTypes.DataString, new[] { "Default" });
            AddWorkspace(relationTypes.DataDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.DataDate, new[] { "Default" });
            AddWorkspace(relationTypes.DataDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.DataDateTime2, new[] { "Default" });
            AddWorkspace(relationTypes.DataRadioGroup, new[] { "Default" });
            AddWorkspace(relationTypes.DataSlider, new[] { "Default" });
            AddWorkspace(relationTypes.DataSlideToggle, new[] { "Default" });
            AddWorkspace(relationTypes.DataPlainText, new[] { "Default" });
            AddWorkspace(relationTypes.DataMarkdown, new[] { "Default" });
            AddWorkspace(relationTypes.DataHtml, new[] { "Default" });

            // MediaTyped
            AddWorkspace(relationTypes.MediaTypedMarkdown, new[] { "Default" });

            // Organization
            AddWorkspace(relationTypes.OrganizationEmployees, new[] { "Default" });
            AddWorkspace(relationTypes.OrganizationManager, new[] { "Default" });
            AddWorkspace(relationTypes.OrganizationOwner, new[] { "Default" });
            AddWorkspace(relationTypes.OrganizationShareholders, new[] { "Default" });




            AddWorkspace(relationTypes.OrganizationName, new[] { "Default" });

            AddWorkspace(relationTypes.OrganizationCycleOne, new[] { "Default" });
            AddWorkspace(relationTypes.OrganizationCycleMany, new[] { "Default" });
            AddWorkspace(relationTypes.OrganizationOneData, new[] { "Default" });
            AddWorkspace(relationTypes.OrganizationManyDatas, new[] { "Default" });
            AddWorkspace(relationTypes.OrganizationJustDidIt, new[] { "Default" });
            AddWorkspace(relationTypes.OrganizationJustDidItDerived, new[] { "Default" });


            AddWorkspace(methodTypes.OrganizationJustDoIt, new[] { "Default" });
            AddWorkspace(methodTypes.OrganizationToggleCanWrite, new[] { "Default" });

            // OverrideClass

            // OverrideInterface
            AddWorkspace(relationTypes.OverrideInterfaceOverrideRequired, new[] { "Default" });

            // Person
            AddWorkspace(relationTypes.PersonFirstName, new[] { "Default" });
            AddWorkspace(relationTypes.PersonMiddleName, new[] { "Default" });
            AddWorkspace(relationTypes.PersonLastName, new[] { "Default" });

            AddWorkspace(relationTypes.PersonBirthDate, new[] { "Default" });
            AddWorkspace(relationTypes.PersonDomainFullName, new[] { "Default" });
            AddWorkspace(relationTypes.PersonDomainGreeting, new[] { "Default" });
            AddWorkspace(relationTypes.PersonCustomFullName, new[] { "Default" });

            AddWorkspace(relationTypes.PersonIsStudent, new[] { "Default" });



            AddWorkspace(relationTypes.PersonWeight, new[] { "Default" });
            AddWorkspace(relationTypes.PersonCycleOne, new[] { "Default" });
            AddWorkspace(relationTypes.PersonCycleMany, new[] { "Default" });


            AddWorkspace(relationTypes.PersonDefaultWorkspaceField, new[] { "Default" });

            // UnitSample
            AddWorkspace(relationTypes.UnitSampleAllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsUnique, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsDecimal, new[] { "Default" });









            // ValiData
            AddWorkspace(relationTypes.ValiDataRequiredPerson, new[] { "Default" });

            // TraceX
            AddWorkspace(relationTypes.TraceXAllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXAllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXAllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXAllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXAllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXAllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXAllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXAllorsUnique, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXMany2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXMany2One, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXOne2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.TraceXOne2One, new[] { "Default" });

            // TraceY
            AddWorkspace(relationTypes.TraceYAllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYAllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYAllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYAllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYAllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYAllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYAllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYAllorsUnique, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYMany2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYMany2One, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYOne2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.TraceYOne2One, new[] { "Default" });

            // TraceZ
            AddWorkspace(relationTypes.TraceZAllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.TraceZAllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.TraceZAllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.TraceZAllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.TraceZAllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.TraceZAllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.TraceZAllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.TraceZAllorsUnique, new[] { "Default" });

            // TrimFrom
            AddWorkspace(relationTypes.TrimFromName, new[] { "Default" });
            AddWorkspace(relationTypes.TrimFromMany2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.TrimFromMany2One, new[] { "Default" });

            // TrimTo
            AddWorkspace(relationTypes.TrimToName, new[] { "Default" });

            // WorkspaceNoneObject1
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXString, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYString, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXYString, new[] { "X", "Y" });

            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceXObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceYObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceXYObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceNonObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceXObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceYObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYToWorkspacXYObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceNonObject2, new[] { "Y" });




            AddWorkspace(methodTypes.WorkspaceNoneObject1DoX, new[] { "X" });
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoY, new[] { "Y" });
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoXY, new[] { "Y" });
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoNone, new[] { "Y" });

            // WorkspaceNonObject2

            // WorkspaceXObject1
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXString, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYString, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXYString, new[] { "X", "Y" });

            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXToWorkspaceXObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXToWorkspaceYObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXToWorkspaceXYObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXToWorkspaceNonObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYToWorkspaceXObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYToWorkspaceYObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYToWorkspacXYObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYToWorkspaceNonObject2, new[] { "Y" });




            AddWorkspace(methodTypes.WorkspaceXObject1DoX, new[] { "X" });
            AddWorkspace(methodTypes.WorkspaceXObject1DoY, new[] { "Y" });
            AddWorkspace(methodTypes.WorkspaceXObject1DoXY, new[] { "Y" });
            AddWorkspace(methodTypes.WorkspaceXObject1DoNone, new[] { "Y" });

            // WorkspaceXObject2

            // WorkspaceXYObject1
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXString, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYString, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXYString, new[] { "X", "Y" });

            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXToWorkspaceXObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXToWorkspaceYObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXToWorkspaceXYObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXToWorkspaceNonObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYToWorkspaceXObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYToWorkspaceYObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYToWorkspacXYObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYToWorkspaceNonObject2, new[] { "Y" });




            AddWorkspace(methodTypes.WorkspaceXYObject1DoX, new[] { "X" });
            AddWorkspace(methodTypes.WorkspaceXYObject1DoY, new[] { "Y" });
            AddWorkspace(methodTypes.WorkspaceXYObject1DoXY, new[] { "Y" });
            AddWorkspace(methodTypes.WorkspaceXYObject1DoNone, new[] { "Y" });

            // WorkspaceXYObject2

            // WorkspaceYObject1
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXString, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYString, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXYString, new[] { "X", "Y" });

            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXToWorkspaceXObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXToWorkspaceYObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXToWorkspaceXYObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXToWorkspaceNonObject2, new[] { "X" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYToWorkspaceXObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYToWorkspaceYObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYToWorkspacXYObject2, new[] { "Y" });
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYToWorkspaceNonObject2, new[] { "Y" });




            AddWorkspace(methodTypes.WorkspaceYObject1DoX, new[] { "X" });
            AddWorkspace(methodTypes.WorkspaceYObject1DoY, new[] { "Y" });
            AddWorkspace(methodTypes.WorkspaceYObject1DoXY, new[] { "Y" });
            AddWorkspace(methodTypes.WorkspaceYObject1DoNone, new[] { "Y" });

            // WorkspaceYObject2


            // Classes
            AddWorkspace(m.C1, new[] { "Default" });
            AddWorkspace(m.C2, new[] { "Default" });





            AddWorkspace(m.AccessClass, new[] { "Default" });
            AddWorkspace(m.Denied, new[] { "Default" });


            AddWorkspace(m.Data, new[] { "Default" });

            AddWorkspace(m.Organization, new[] { "Default" });
            AddWorkspace(m.OverrideClass, new[] { "Default" });
            AddWorkspace(m.Person, new[] { "Default" });
            AddWorkspace(m.UnitSample, new[] { "Default" });
            AddWorkspace(m.ValiData, new[] { "Default" });
            AddWorkspace(m.TraceX, new[] { "Default" });
            AddWorkspace(m.TraceY, new[] { "Default" });
            AddWorkspace(m.TraceZ, new[] { "Default" });
            AddWorkspace(m.TrimFrom, new[] { "Default" });
            AddWorkspace(m.TrimTo, new[] { "Default" });

            AddWorkspace(m.WorkspaceXObject1, new[] { "X" });
            AddWorkspace(m.WorkspaceXObject2, new[] { "X" });
            AddWorkspace(m.WorkspaceXYObject1, new[] { "X", "Y" });
            AddWorkspace(m.WorkspaceXYObject2, new[] { "X", "Y" });
            AddWorkspace(m.WorkspaceYObject1, new[] { "Y" });
            AddWorkspace(m.WorkspaceYObject2, new[] { "Y" });

            // Custom

            // Methods
            AddWorkspace(methodTypes.DeletableDelete, "Default");

            // Relations
            AddWorkspace(relationTypes.UserUserEmail, "Default");
            AddWorkspace(relationTypes.UserUserName, "Default");

            AddWorkspace(relationTypes.RoleName, "Default");

            // Objects
            AddWorkspace(m.UserGroup, "Default");

            // Classes
            var classes = m.Classes.Where(@class =>
                @class.RoleTypes.Any(v => v.RelationType.AssignedWorkspaceNames?.Contains("Default") == true) ||
                @class.AssociationTypes.Any(v => v.RelationType.AssignedWorkspaceNames?.Contains("Default") == true) ||
                @class.MethodTypes.Any(v => v.AssignedWorkspaceNames?.Contains("Default") == true))
                .ToArray();

            foreach (Class @class in classes)
            {
                AddWorkspace(@class, "Default");
            }
        }
    }
}
