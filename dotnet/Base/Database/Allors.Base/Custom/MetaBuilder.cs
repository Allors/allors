// <copyright file="Organization.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta.Configuration
{
    using System;
    using System.Linq;

    public partial class MetaBuilder
    {
        private static readonly string[] DefaultWorkspaceNames = ["Default"];
        private static readonly string[] XWorkspaceNames = ["X"];
        private static readonly string[] YWorkspaceNames = ["Y"];
        private static readonly string[] XYWorkspaceNames = ["X", "Y"];

        static void AddWorkspace(Class @class, params string[] workspaceNames) => @class.AssignedWorkspaceNames = (@class.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        static void AddWorkspace(MethodType methodType, params string[] workspaceNames) => methodType.AssignedWorkspaceNames = (methodType.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        static void AddWorkspace(RoleType roleType, params string[] workspaceNames) => roleType.AssignedWorkspaceNames = (roleType.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        private void BuildCustom(MetaPopulation m, Domains domains, ObjectTypes objectTypes, RoleTypes roleTypes, MethodTypes methodTypes)
        {
            this.DefaultWorkspace(m, domains, objectTypes, roleTypes, methodTypes);
        }

        private void DefaultWorkspace(MetaPopulation m, Domains domains, ObjectTypes objectTypes, RoleTypes roleTypes, MethodTypes methodTypes)
        {

            // RelationTypes & MethodTypes
            // Counter;

            // Singleton;
            AddWorkspace(roleTypes.SingletonDefaultLocale, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.SingletonAdditionalLocales, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.SingletonLocales, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.SingletonLogoImage, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.SingletonAutocompleteDefault, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.SingletonSelectDefault, DefaultWorkspaceNames);

            // Media;
            AddWorkspace(roleTypes.MediaRevision, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaMediaContent, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaInType, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaInData, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaInDataUri, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaInFileName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaType, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaFileName, DefaultWorkspaceNames);

            // MediaContent;
            AddWorkspace(roleTypes.MediaContentType, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.MediaContentData, DefaultWorkspaceNames);

            // PrintDocument;
            AddWorkspace(roleTypes.PrintDocumentMedia, DefaultWorkspaceNames);

            // Template;

            // TemplateType;

            // PersistentPreparedExtent;

            // PersistentPreparedSelect;

            // Country;
            AddWorkspace(roleTypes.CountryCurrency, DefaultWorkspaceNames);

            // Currency;

            // Language;
            AddWorkspace(roleTypes.LanguageNativeName, DefaultWorkspaceNames);

            // Locale;
            AddWorkspace(roleTypes.LocaleLanguage, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.LocaleCountry, DefaultWorkspaceNames);

            // LocalisedMedia;
            AddWorkspace(roleTypes.LocalisedMediaMedia, DefaultWorkspaceNames);

            // LocalisedText;
            AddWorkspace(roleTypes.LocalisedTextText, DefaultWorkspaceNames);

            // AutomatedAgent;

            // Person;
            AddWorkspace(roleTypes.PersonFirstName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonMiddleName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonLastName, DefaultWorkspaceNames);

            AddWorkspace(roleTypes.PersonMailboxAddress, DefaultWorkspaceNames);

            AddWorkspace(roleTypes.PersonBirthDate, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonFullName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonDomainFullName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonDomainGreeting, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonGender, DefaultWorkspaceNames);

            AddWorkspace(roleTypes.PersonIsStudent, DefaultWorkspaceNames);


            AddWorkspace(roleTypes.PersonPhoto, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonPictures, DefaultWorkspaceNames);



            AddWorkspace(roleTypes.PersonWeight, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonCycleOne, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PersonCycleMany, DefaultWorkspaceNames);

            AddWorkspace(roleTypes.PersonDefaultWorkspaceField, DefaultWorkspaceNames);

            // EmailMessage;
            AddWorkspace(roleTypes.EmailMessageDateCreated, DefaultWorkspaceNames);

            AddWorkspace(roleTypes.EmailMessageDateSent, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.EmailMessageSender, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.EmailMessageRecipients, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.EmailMessageRecipientEmailAddress, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.EmailMessageSubject, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.EmailMessageBody, DefaultWorkspaceNames);

            // Notification;
            AddWorkspace(roleTypes.NotificationConfirmed, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.NotificationTitle, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.NotificationDescription, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.NotificationDateCreated, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.NotificationConfirm, DefaultWorkspaceNames);

            // NotificationList;
            AddWorkspace(roleTypes.NotificationListNotifications, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.NotificationListUnconfirmedNotifications, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.NotificationListConfirmedNotifications, DefaultWorkspaceNames);

            // TaskAssignment;
            AddWorkspace(roleTypes.TaskAssignmentUser, DefaultWorkspaceNames);

            AddWorkspace(roleTypes.TaskAssignmentTask, DefaultWorkspaceNames);

            // AccessClass
            AddWorkspace(roleTypes.AccessClassProperty, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.AccessClassAnotherProperty, DefaultWorkspaceNames);


            // Employment;
            AddWorkspace(roleTypes.EmploymentEmployee, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.EmploymentEmployer, DefaultWorkspaceNames);

            // Gender;

            // Organization;
            AddWorkspace(roleTypes.OrganizationEmployees, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationManager, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationOwner, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationShareholders, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationActiveEmployments, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationInactiveEmployments, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationActiveEmployees, DefaultWorkspaceNames);


            AddWorkspace(roleTypes.OrganizationIncorporationDate, DefaultWorkspaceNames);




            AddWorkspace(roleTypes.OrganizationName, DefaultWorkspaceNames);

            AddWorkspace(roleTypes.OrganizationCycleOne, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationCycleMany, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationOneData, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationManyDatas, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationJustDidIt, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationJustDidItDerived, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrganizationCountry, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.OrganizationJustDoIt, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.OrganizationToggleCanWrite, DefaultWorkspaceNames);

            // BadUI;

            // Build;

            // C1;
            AddWorkspace(roleTypes.C1C1AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1AllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DateTimeLessThan, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DateTimeGreaterThan, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DateTimeBetweenA, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DateTimeBetweenB, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DecimalLessThan, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DecimalGreaterThan, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DecimalBetweenA, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DecimalBetweenB, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1AllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DoubleLessThan, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DoubleGreaterThan, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DoubleBetweenA, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1DoubleBetweenB, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1IntegerLessThan, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1IntegerGreaterThan, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1IntegerBetweenA, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1IntegerBetweenB, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1AllorsString, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1AllorsStringEquals, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1AllorsStringMax, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1AllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1C1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1C2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1C2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I12One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C1C1I2One2One, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.C1ClassMethod, DefaultWorkspaceNames);


            // C2;
            AddWorkspace(roleTypes.C2C2AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2C1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2AllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I12One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2AllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2C2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2C2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2AllorsString, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2AllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2C2AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.C2S1One2One, DefaultWorkspaceNames);

            // ClassWithoutRoles;

            // Data;
            AddWorkspace(roleTypes.DataCheckbox, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataChips, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataString, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataDecimal, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataDate, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataDateTime, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataDateTime2, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataFile, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataMonth, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataMultipleFiles, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataRadioGroup, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataSlider, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataSlideToggle, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataPlainText, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataMarkdown, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataHtml, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataStatic, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataAutocompleteFilter, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataAutocompleteOptions, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataAutocompleteAssignedFilter, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataAutocompleteDerivedFilter, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataAutocompleteAssignedOptions, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataAutocompleteDerivedOptions, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataSelect, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataSelectAssigned, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataSelectDerived, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataLocalisedTexts, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.DataLocalisedMarkdowns, DefaultWorkspaceNames);

            // Dependee;

            // Dependent;

            // Extender;

            // First;

            // Four;

            // From;

            // HomeAddress;
            AddWorkspace(roleTypes.HomeAddressStreet, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.HomeAddressHouseNumber, DefaultWorkspaceNames);

            // Left;

            // MailboxAddress;
            AddWorkspace(roleTypes.MailboxAddressPoBox, DefaultWorkspaceNames);

            // MediaTyped;
            AddWorkspace(roleTypes.MediaTypedMarkdown, DefaultWorkspaceNames);

            // Middle;

            // One;

            // Order;
            AddWorkspace(roleTypes.OrderCurrentVersion, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrderAllVersions, DefaultWorkspaceNames);

            // OrderLine;
            AddWorkspace(roleTypes.OrderLineCurrentVersion, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.OrderLineAllVersions, DefaultWorkspaceNames);

            // OrderLineVersion;

            // OrderState;

            // OrderVersion;

            // Page;
            AddWorkspace(roleTypes.PageName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PageContent, DefaultWorkspaceNames);

            // PaymentState;

            // Place;

            // Post;

            // Right;

            // Second;

            // ShipmentState;

            // SimpleJob;

            // StatefulCompany;

            // Subdependee;

            // SyncDepth2;

            // SyncDepthC1;

            // SyncRoot;

            // Third;

            // Three;

            // To;

            // Two;

            // UnitSample;
            AddWorkspace(roleTypes.UnitSampleAllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UnitSampleAllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UnitSampleAllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UnitSampleAllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UnitSampleAllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UnitSampleAllorsString, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UnitSampleAllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UnitSampleAllorsDecimal, DefaultWorkspaceNames);









            // ValidationC1;

            // ValidationC2;

            // WorkspaceNoneObject1;
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceXString, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceYString, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceXYString, XYWorkspaceNames);

            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceYObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceXYObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceNonObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceXObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceYObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceYToWorkspacXYObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceNonObject2, YWorkspaceNames);




            AddWorkspace(methodTypes.WorkspaceNoneObject1DoX, XWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoXY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoNone, YWorkspaceNames);

            // WorkspaceNonObject2;

            // WorkspaceXObject1;
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceXString, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceYString, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceXYString, XYWorkspaceNames);

            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceXToWorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceXToWorkspaceYObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceXToWorkspaceXYObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceXToWorkspaceNonObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceYToWorkspaceXObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceYToWorkspaceYObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceYToWorkspacXYObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXObject1WorkspaceYToWorkspaceNonObject2, YWorkspaceNames);




            AddWorkspace(methodTypes.WorkspaceXObject1DoX, XWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXObject1DoY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXObject1DoXY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXObject1DoNone, YWorkspaceNames);

            // WorkspaceXObject2;

            // WorkspaceXYObject1;
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceXString, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceYString, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceXYString, XYWorkspaceNames);

            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceXToWorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceXToWorkspaceYObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceXToWorkspaceXYObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceXToWorkspaceNonObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceYToWorkspaceXObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceYToWorkspaceYObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceYToWorkspacXYObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceXYObject1WorkspaceYToWorkspaceNonObject2, YWorkspaceNames);




            AddWorkspace(methodTypes.WorkspaceXYObject1DoX, XWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXYObject1DoY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXYObject1DoXY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXYObject1DoNone, YWorkspaceNames);

            // WorkspaceXYObject2;

            // WorkspaceYObject1;
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceXString, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceYString, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceXYString, XYWorkspaceNames);

            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceXToWorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceXToWorkspaceYObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceXToWorkspaceXYObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceXToWorkspaceNonObject2, XWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceYToWorkspaceXObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceYToWorkspaceYObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceYToWorkspacXYObject2, YWorkspaceNames);
            AddWorkspace(roleTypes.WorkspaceYObject1WorkspaceYToWorkspaceNonObject2, YWorkspaceNames);




            AddWorkspace(methodTypes.WorkspaceYObject1DoX, XWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceYObject1DoY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceYObject1DoXY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceYObject1DoNone, YWorkspaceNames);

            // WorkspaceYObject2;

            // Grant;

            // Login;

            // CreatePermission;

            // ExecutePermission;

            // ReadPermission;

            // WritePermission;

            // Revocation;

            // Role;
            AddWorkspace(roleTypes.RoleName, "Default");

            // SecurityToken;

            // UserGroup;
            AddWorkspace(roleTypes.UserGroupMembers, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UserGroupName, DefaultWorkspaceNames);

            // Cacheable;

            // Enumeration;
            AddWorkspace(roleTypes.EnumerationKey, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.EnumerationLocalisedNames, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.EnumerationIsActive, DefaultWorkspaceNames);

            // Period;
            AddWorkspace(roleTypes.PeriodFromDate, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.PeriodThroughDate, DefaultWorkspaceNames);

            // Version;
            AddWorkspace(roleTypes.VersionDerivationTimeStamp, DefaultWorkspaceNames);

            // Versioned;

            // Printable;
            AddWorkspace(roleTypes.PrintablePrintDocument, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.PrintablePrint, DefaultWorkspaceNames);

            // Localised;
            AddWorkspace(roleTypes.LocalisedLocale, DefaultWorkspaceNames);

            // Auditable;
            AddWorkspace(roleTypes.AuditableCreatedBy, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.AuditableLastModifiedBy, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.AuditableCreationDate, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.AuditableLastModifiedDate, DefaultWorkspaceNames);

            // ApproveTask;
            AddWorkspace(roleTypes.ApproveTaskComment, DefaultWorkspaceNames);


            AddWorkspace(methodTypes.ApproveTaskApprove, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.ApproveTaskReject, DefaultWorkspaceNames);

            // Task;
            AddWorkspace(roleTypes.TaskWorkItem, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.TaskTitle, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.TaskDateCreated, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.TaskDateDue, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.TaskDateClosed, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.TaskParticipants, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.TaskPerformer, DefaultWorkspaceNames);

            // Transitional;

            // TransitionalVersion;

            // TrimFrom
            AddWorkspace(roleTypes.TrimFromName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.TrimFromMany2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.TrimFromMany2One, DefaultWorkspaceNames);

            // TrimTo
            AddWorkspace(roleTypes.TrimToName, DefaultWorkspaceNames);

            // User;
            AddWorkspace(roleTypes.UserNotificationList, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UserGroupMembers, "Default");
            AddWorkspace(roleTypes.UserUserName, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UserUserEmail, DefaultWorkspaceNames);







            // WorkItem;
            AddWorkspace(roleTypes.WorkItemWorkItemDescription, DefaultWorkspaceNames);

            // AccessInterface;

            // Address;

            // Addressable;
            AddWorkspace(roleTypes.AddressableAddress, DefaultWorkspaceNames);

            // DerivationCounted;

            // I1;
            AddWorkspace(roleTypes.I1I1I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1AllorsString, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1AllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1C2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1C1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I12One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1C2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1AllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I1I1AllorsUnique, DefaultWorkspaceNames);

            // I12;
            AddWorkspace(roleTypes.I12I12AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12C2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12AllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12AllorsString, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12Name, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12Order, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12AllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12C1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I12One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12Dependencies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12I1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I12I12AllorsDateTime, DefaultWorkspaceNames);

            // I2;
            AddWorkspace(roleTypes.I2I2I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2C1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2AllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2AllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I12One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2AllorsString, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2C2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I1One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2C2One2One, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.I2I2AllorsDouble, DefaultWorkspaceNames);

            // S1;

            // Shared;

            // SyncDepthI1;

            // ValidationI12;

            // Deletable;
            AddWorkspace(methodTypes.DeletableDelete, DefaultWorkspaceNames);

            // Object;

            // UniquelyIdentifiable;
            AddWorkspace(roleTypes.UniquelyIdentifiableUniqueId, DefaultWorkspaceNames);

            // DelegatedAccessObject;

            // Permission;

            // SecurityTokenOwner;

            // UserPasswordReset;
            AddWorkspace(roleTypes.UserPasswordResetInExistingUserPassword, DefaultWorkspaceNames);
            AddWorkspace(roleTypes.UserPasswordResetInUserPassword, DefaultWorkspaceNames);

            // Classes
            AddWorkspace(objectTypes.AccessClass, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Denied, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Media, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.MediaContent, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.MediaTyped, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Country, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Gender, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Language, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Locale, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.LocalisedText, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Person, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.TaskAssignment, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Employment, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.MailboxAddress, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Organization, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.C1, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.C2, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.Data, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.HomeAddress, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.TrimFrom, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.TrimTo, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.UnitSample, DefaultWorkspaceNames);
            AddWorkspace(objectTypes.WorkspaceXObject1, XWorkspaceNames);
            AddWorkspace(objectTypes.WorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(objectTypes.WorkspaceXYObject1, XYWorkspaceNames);
            AddWorkspace(objectTypes.WorkspaceXYObject2, XYWorkspaceNames);
            AddWorkspace(objectTypes.WorkspaceYObject1, YWorkspaceNames);
            AddWorkspace(objectTypes.WorkspaceYObject2, YWorkspaceNames);
        }
    }
}
