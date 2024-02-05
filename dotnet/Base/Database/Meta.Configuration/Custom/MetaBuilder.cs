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
        private static readonly string[] DefaultWorkspaceNames = new[] { "Default" };
        private static readonly string[] XWorkspaceNames = new[] { "X" };
        private static readonly string[] YWorkspaceNames = new[] { "Y" };
        private static readonly string[] XYWorkspaceNames = new[] { "X", "Y" };

        static void AddWorkspace(Class @class, params string[] workspaceNames) => @class.AssignedWorkspaceNames = (@class.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        static void AddWorkspace(MethodType methodType, params string[] workspaceNames) => methodType.AssignedWorkspaceNames = (methodType.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        static void AddWorkspace(RelationType relationType, params string[] workspaceNames) => relationType.AssignedWorkspaceNames = (relationType.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        private void BuildCustom(M m, Domains domains, RelationTypes relationTypes, MethodTypes methodTypes)
        {
            this.DefaultWorkspace(m, domains, relationTypes, methodTypes);
        }

        private void DefaultWorkspace(M m, Domains domains, RelationTypes relationTypes, MethodTypes methodTypes)
        {

            // RelationTypes & MethodTypes
            // Counter;

            // Singleton;
            AddWorkspace(relationTypes.SingletonDefaultLocale, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.SingletonAdditionalLocales, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.SingletonLocales, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.SingletonLogoImage, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.SingletonAutocompleteDefault, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.SingletonSelectDefault, DefaultWorkspaceNames);

            // Media;
            AddWorkspace(relationTypes.MediaRevision, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaMediaContent, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaInType, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaInData, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaInDataUri, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaInFileName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaType, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaFileName, DefaultWorkspaceNames);

            // MediaContent;
            AddWorkspace(relationTypes.MediaContentType, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.MediaContentData, DefaultWorkspaceNames);

            // PrintDocument;
            AddWorkspace(relationTypes.PrintDocumentMedia, DefaultWorkspaceNames);

            // Template;

            // TemplateType;

            // PersistentPreparedExtent;

            // PersistentPreparedSelect;

            // Country;
            AddWorkspace(relationTypes.CountryCurrency, DefaultWorkspaceNames);

            // Currency;

            // Language;
            AddWorkspace(relationTypes.LanguageNativeName, DefaultWorkspaceNames);

            // Locale;
            AddWorkspace(relationTypes.LocaleLanguage, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.LocaleCountry, DefaultWorkspaceNames);

            // LocalisedMedia;
            AddWorkspace(relationTypes.LocalisedMediaMedia, DefaultWorkspaceNames);

            // LocalisedText;
            AddWorkspace(relationTypes.LocalisedTextText, DefaultWorkspaceNames);

            // AutomatedAgent;

            // Person;
            AddWorkspace(relationTypes.PersonFirstName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonMiddleName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonLastName, DefaultWorkspaceNames);

            AddWorkspace(relationTypes.PersonMailboxAddress, DefaultWorkspaceNames);

            AddWorkspace(relationTypes.PersonBirthDate, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonFullName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonDomainFullName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonDomainGreeting, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonGender, DefaultWorkspaceNames);

            AddWorkspace(relationTypes.PersonIsStudent, DefaultWorkspaceNames);


            AddWorkspace(relationTypes.PersonPhoto, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonPictures, DefaultWorkspaceNames);



            AddWorkspace(relationTypes.PersonWeight, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonCycleOne, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PersonCycleMany, DefaultWorkspaceNames);

            AddWorkspace(relationTypes.PersonDefaultWorkspaceField, DefaultWorkspaceNames);

            // EmailMessage;
            AddWorkspace(relationTypes.EmailMessageDateCreated, DefaultWorkspaceNames);

            AddWorkspace(relationTypes.EmailMessageDateSent, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.EmailMessageSender, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.EmailMessageRecipients, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.EmailMessageRecipientEmailAddress, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.EmailMessageSubject, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.EmailMessageBody, DefaultWorkspaceNames);

            // Notification;
            AddWorkspace(relationTypes.NotificationConfirmed, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.NotificationTitle, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.NotificationDescription, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.NotificationDateCreated, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.NotificationConfirm, DefaultWorkspaceNames);

            // NotificationList;
            AddWorkspace(relationTypes.NotificationListNotifications, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.NotificationListUnconfirmedNotifications, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.NotificationListConfirmedNotifications, DefaultWorkspaceNames);

            // TaskAssignment;
            AddWorkspace(relationTypes.TaskAssignmentUser, DefaultWorkspaceNames);

            AddWorkspace(relationTypes.TaskAssignmentTask, DefaultWorkspaceNames);

            // AccessClass
            AddWorkspace(relationTypes.AccessClassProperty, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.AccessClassAnotherProperty, DefaultWorkspaceNames);


            // Employment;
            AddWorkspace(relationTypes.EmploymentEmployee, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.EmploymentEmployer, DefaultWorkspaceNames);

            // Gender;

            // Organization;
            AddWorkspace(relationTypes.OrganizationEmployees, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationManager, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationOwner, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationShareholders, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationActiveEmployments, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationInactiveEmployments, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationActiveEmployees, DefaultWorkspaceNames);


            AddWorkspace(relationTypes.OrganizationIncorporationDate, DefaultWorkspaceNames);




            AddWorkspace(relationTypes.OrganizationName, DefaultWorkspaceNames);

            AddWorkspace(relationTypes.OrganizationCycleOne, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationCycleMany, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationOneData, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationManyDatas, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationJustDidIt, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationJustDidItDerived, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrganizationCountry, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.OrganizationJustDoIt, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.OrganizationToggleCanWrite, DefaultWorkspaceNames);

            // BadUI;

            // Build;

            // C1;
            AddWorkspace(relationTypes.C1C1AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1AllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DateTimeLessThan, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DateTimeGreaterThan, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DateTimeBetweenA, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DateTimeBetweenB, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DecimalLessThan, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DecimalGreaterThan, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DecimalBetweenA, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DecimalBetweenB, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1AllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DoubleLessThan, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DoubleGreaterThan, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DoubleBetweenA, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1DoubleBetweenB, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1IntegerLessThan, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1IntegerGreaterThan, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1IntegerBetweenA, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1IntegerBetweenB, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1AllorsString, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1AllorsStringEquals, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1AllorsStringMax, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1AllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1C1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1C2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1C2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I12One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C1C1I2One2One, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.C1ClassMethod, DefaultWorkspaceNames);


            // C2;
            AddWorkspace(relationTypes.C2C2AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2C1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2AllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I12One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2AllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2C2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2C2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2AllorsString, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2AllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2C2AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.C2S1One2One, DefaultWorkspaceNames);

            // ClassWithoutRoles;

            // Data;
            AddWorkspace(relationTypes.DataCheckbox, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataChips, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataString, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataDecimal, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataDate, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataDateTime, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataDateTime2, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataFile, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataMonth, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataMultipleFiles, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataRadioGroup, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataSlider, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataSlideToggle, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataPlainText, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataMarkdown, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataHtml, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataStatic, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataAutocompleteFilter, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataAutocompleteOptions, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataAutocompleteAssignedFilter, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataAutocompleteDerivedFilter, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataAutocompleteAssignedOptions, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataAutocompleteDerivedOptions, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataSelect, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataSelectAssigned, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataSelectDerived, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataLocalisedTexts, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.DataLocalisedMarkdowns, DefaultWorkspaceNames);

            // Dependee;

            // Dependent;

            // Extender;

            // First;

            // Four;

            // From;

            // HomeAddress;
            AddWorkspace(relationTypes.HomeAddressStreet, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.HomeAddressHouseNumber, DefaultWorkspaceNames);

            // Left;

            // MailboxAddress;
            AddWorkspace(relationTypes.MailboxAddressPoBox, DefaultWorkspaceNames);

            // MediaTyped;
            AddWorkspace(relationTypes.MediaTypedMarkdown, DefaultWorkspaceNames);

            // Middle;

            // One;

            // Order;
            AddWorkspace(relationTypes.OrderCurrentVersion, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrderAllVersions, DefaultWorkspaceNames);

            // OrderLine;
            AddWorkspace(relationTypes.OrderLineCurrentVersion, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.OrderLineAllVersions, DefaultWorkspaceNames);

            // OrderLineVersion;

            // OrderState;

            // OrderVersion;

            // Page;
            AddWorkspace(relationTypes.PageName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PageContent, DefaultWorkspaceNames);

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
            AddWorkspace(relationTypes.UnitSampleAllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UnitSampleAllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UnitSampleAllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UnitSampleAllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UnitSampleAllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UnitSampleAllorsString, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UnitSampleAllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UnitSampleAllorsDecimal, DefaultWorkspaceNames);









            // ValidationC1;

            // ValidationC2;

            // WorkspaceNoneObject1;
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXString, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYString, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXYString, XYWorkspaceNames);

            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceYObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceXYObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceXToWorkspaceNonObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceXObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceYObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYToWorkspacXYObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceNoneObject1WorkspaceYToWorkspaceNonObject2, YWorkspaceNames);




            AddWorkspace(methodTypes.WorkspaceNoneObject1DoX, XWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoXY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceNoneObject1DoNone, YWorkspaceNames);

            // WorkspaceNonObject2;

            // WorkspaceXObject1;
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXString, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYString, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXYString, XYWorkspaceNames);

            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXToWorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXToWorkspaceYObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXToWorkspaceXYObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceXToWorkspaceNonObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYToWorkspaceXObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYToWorkspaceYObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYToWorkspacXYObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXObject1WorkspaceYToWorkspaceNonObject2, YWorkspaceNames);




            AddWorkspace(methodTypes.WorkspaceXObject1DoX, XWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXObject1DoY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXObject1DoXY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXObject1DoNone, YWorkspaceNames);

            // WorkspaceXObject2;

            // WorkspaceXYObject1;
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXString, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYString, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXYString, XYWorkspaceNames);

            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXToWorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXToWorkspaceYObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXToWorkspaceXYObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceXToWorkspaceNonObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYToWorkspaceXObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYToWorkspaceYObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYToWorkspacXYObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceXYObject1WorkspaceYToWorkspaceNonObject2, YWorkspaceNames);




            AddWorkspace(methodTypes.WorkspaceXYObject1DoX, XWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXYObject1DoY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXYObject1DoXY, YWorkspaceNames);
            AddWorkspace(methodTypes.WorkspaceXYObject1DoNone, YWorkspaceNames);

            // WorkspaceXYObject2;

            // WorkspaceYObject1;
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXString, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYString, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXYString, XYWorkspaceNames);

            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXToWorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXToWorkspaceYObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXToWorkspaceXYObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceXToWorkspaceNonObject2, XWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYToWorkspaceXObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYToWorkspaceYObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYToWorkspacXYObject2, YWorkspaceNames);
            AddWorkspace(relationTypes.WorkspaceYObject1WorkspaceYToWorkspaceNonObject2, YWorkspaceNames);




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
            AddWorkspace(relationTypes.RoleName, "Default");

            // SecurityToken;

            // UserGroup;
            AddWorkspace(relationTypes.UserGroupMembers, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UserGroupName, DefaultWorkspaceNames);

            // Cacheable;

            // Enumeration;
            AddWorkspace(relationTypes.EnumerationKey, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.EnumerationLocalisedNames, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.EnumerationIsActive, DefaultWorkspaceNames);

            // Period;
            AddWorkspace(relationTypes.PeriodFromDate, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.PeriodThroughDate, DefaultWorkspaceNames);

            // Version;
            AddWorkspace(relationTypes.VersionDerivationTimeStamp, DefaultWorkspaceNames);

            // Versioned;

            // Printable;
            AddWorkspace(relationTypes.PrintablePrintDocument, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.PrintablePrint, DefaultWorkspaceNames);

            // Localised;
            AddWorkspace(relationTypes.LocalisedLocale, DefaultWorkspaceNames);

            // Auditable;
            AddWorkspace(relationTypes.AuditableCreatedBy, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.AuditableLastModifiedBy, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.AuditableCreationDate, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.AuditableLastModifiedDate, DefaultWorkspaceNames);

            // ApproveTask;
            AddWorkspace(relationTypes.ApproveTaskComment, DefaultWorkspaceNames);


            AddWorkspace(methodTypes.ApproveTaskApprove, DefaultWorkspaceNames);
            AddWorkspace(methodTypes.ApproveTaskReject, DefaultWorkspaceNames);

            // Task;
            AddWorkspace(relationTypes.TaskWorkItem, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.TaskTitle, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.TaskDateCreated, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.TaskDateDue, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.TaskDateClosed, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.TaskParticipants, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.TaskPerformer, DefaultWorkspaceNames);

            // Transitional;

            // TransitionalVersion;

            // TrimFrom
            AddWorkspace(relationTypes.TrimFromName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.TrimFromMany2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.TrimFromMany2One, DefaultWorkspaceNames);

            // TrimTo
            AddWorkspace(relationTypes.TrimToName, DefaultWorkspaceNames);

            // User;
            AddWorkspace(relationTypes.UserNotificationList, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UserGroupMembers, "Default");
            AddWorkspace(relationTypes.UserUserName, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UserUserEmail, DefaultWorkspaceNames);







            // WorkItem;
            AddWorkspace(relationTypes.WorkItemWorkItemDescription, DefaultWorkspaceNames);

            // AccessInterface;

            // Address;

            // Addressable;
            AddWorkspace(relationTypes.AddressableAddress, DefaultWorkspaceNames);

            // DerivationCounted;

            // I1;
            AddWorkspace(relationTypes.I1I1I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1AllorsString, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1AllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1C2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1C1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I12One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1C2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1AllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I1I1AllorsUnique, DefaultWorkspaceNames);

            // I12;
            AddWorkspace(relationTypes.I12I12AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12C2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12AllorsDouble, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12AllorsString, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12Name, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12Order, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12AllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12C1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I12One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12Dependencies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12I1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I12I12AllorsDateTime, DefaultWorkspaceNames);

            // I2;
            AddWorkspace(relationTypes.I2I2I2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2C1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I12Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2AllorsBoolean, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2C1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2C1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2AllorsDecimal, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2AllorsBinary, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2AllorsUnique, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I1Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2AllorsDateTime, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I12One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I12One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2C2Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2C2Many2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2AllorsString, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2C2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I1One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I1One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I12Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2AllorsInteger, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2I2One2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2C1Many2Manies, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2C2One2One, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.I2I2AllorsDouble, DefaultWorkspaceNames);

            // S1;

            // Shared;

            // SyncDepthI1;

            // ValidationI12;

            // Deletable;
            AddWorkspace(methodTypes.DeletableDelete, DefaultWorkspaceNames);

            // Object;

            // UniquelyIdentifiable;
            AddWorkspace(relationTypes.UniquelyIdentifiableUniqueId, DefaultWorkspaceNames);

            // DelegatedAccessObject;

            // Permission;

            // SecurityTokenOwner;

            // UserPasswordReset;
            AddWorkspace(relationTypes.UserPasswordResetInExistingUserPassword, DefaultWorkspaceNames);
            AddWorkspace(relationTypes.UserPasswordResetInUserPassword, DefaultWorkspaceNames);

            // Classes
            AddWorkspace(m.AccessClass, DefaultWorkspaceNames);
            AddWorkspace(m.Denied, DefaultWorkspaceNames);
            AddWorkspace(m.Media, DefaultWorkspaceNames);
            AddWorkspace(m.MediaContent, DefaultWorkspaceNames);
            AddWorkspace(m.MediaTyped, DefaultWorkspaceNames);
            AddWorkspace(m.Country, DefaultWorkspaceNames);
            AddWorkspace(m.Gender, DefaultWorkspaceNames);
            AddWorkspace(m.Language, DefaultWorkspaceNames);
            AddWorkspace(m.Locale, DefaultWorkspaceNames);
            AddWorkspace(m.LocalisedText, DefaultWorkspaceNames);
            AddWorkspace(m.Person, DefaultWorkspaceNames);
            AddWorkspace(m.TaskAssignment, DefaultWorkspaceNames);
            AddWorkspace(m.Employment, DefaultWorkspaceNames);
            AddWorkspace(m.MailboxAddress, DefaultWorkspaceNames);
            AddWorkspace(m.Organization, DefaultWorkspaceNames);
            AddWorkspace(m.C1, DefaultWorkspaceNames);
            AddWorkspace(m.C2, DefaultWorkspaceNames);
            AddWorkspace(m.Data, DefaultWorkspaceNames);
            AddWorkspace(m.HomeAddress, DefaultWorkspaceNames);
            AddWorkspace(m.TrimFrom, DefaultWorkspaceNames);
            AddWorkspace(m.TrimTo, DefaultWorkspaceNames);
            AddWorkspace(m.UnitSample, DefaultWorkspaceNames);
            AddWorkspace(m.WorkspaceXObject1, XWorkspaceNames);
            AddWorkspace(m.WorkspaceXObject2, XWorkspaceNames);
            AddWorkspace(m.WorkspaceXYObject1, XYWorkspaceNames);
            AddWorkspace(m.WorkspaceXYObject2, XYWorkspaceNames);
            AddWorkspace(m.WorkspaceYObject1, YWorkspaceNames);
            AddWorkspace(m.WorkspaceYObject2, YWorkspaceNames);
        }
    }
}
