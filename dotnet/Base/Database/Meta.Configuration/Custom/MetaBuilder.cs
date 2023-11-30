// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta.Configuration
{
    using System;
    using System.Linq;
    using Extensions;

    public partial class MetaBuilder
    {
        static void AddWorkspace(Class @class, params string[] workspaceNames) => @class.AssignedWorkspaceNames = (@class.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        static void AddWorkspace(MethodType methodType, params string[] workspaceNames) => methodType.AssignedWorkspaceNames = (methodType.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        static void AddWorkspace(RelationType relationType, params string[] workspaceNames) => relationType.AssignedWorkspaceNames = (relationType.AssignedWorkspaceNames ?? Array.Empty<string>()).Union(workspaceNames).ToArray();

        private void BuildCustom(MetaPopulation meta, Domains domains, RelationTypes relationTypes, MethodTypes methodTypes)
        {
            this.DefaultWorkspace(meta, domains, relationTypes, methodTypes);
        }

        private void DefaultWorkspace(MetaPopulation meta, Domains domains, RelationTypes relationTypes, MethodTypes methodTypes)
        {

            // RelationTypes & MethodTypes
            // Counter;

            // Singleton;
            AddWorkspace(relationTypes.SingletonDefaultLocale, new[] { "Default" });
            AddWorkspace(relationTypes.SingletonAdditionalLocales, new[] { "Default" });
            AddWorkspace(relationTypes.SingletonLocales, new[] { "Default" });
            AddWorkspace(relationTypes.SingletonLogoImage, new[] { "Default" });
            AddWorkspace(relationTypes.SingletonAutocompleteDefault, new[] { "Default" });
            AddWorkspace(relationTypes.SingletonSelectDefault, new[] { "Default" });

            // Media;
            AddWorkspace(relationTypes.MediaRevision, new[] { "Default" });
            AddWorkspace(relationTypes.MediaMediaContent, new[] { "Default" });
            AddWorkspace(relationTypes.MediaInType, new[] { "Default" });
            AddWorkspace(relationTypes.MediaInData, new[] { "Default" });
            AddWorkspace(relationTypes.MediaInDataUri, new[] { "Default" });
            AddWorkspace(relationTypes.MediaInFileName, new[] { "Default" });
            AddWorkspace(relationTypes.MediaName, new[] { "Default" });
            AddWorkspace(relationTypes.MediaType, new[] { "Default" });
            AddWorkspace(relationTypes.MediaFileName, new[] { "Default" });

            // MediaContent;
            AddWorkspace(relationTypes.MediaContentType, new[] { "Default" });
            AddWorkspace(relationTypes.MediaContentData, new[] { "Default" });

            // PrintDocument;
            AddWorkspace(relationTypes.PrintDocumentMedia, new[] { "Default" });

            // Template;

            // TemplateType;

            // PersistentPreparedExtent;

            // PersistentPreparedSelect;

            // Country;
            AddWorkspace(relationTypes.CountryCurrency, new[] { "Default" });
            AddWorkspace(relationTypes.CountryIsoCode, new[] { "Default" });
            AddWorkspace(relationTypes.CountryLocalisedNames, new[] { "Default" });

            // Currency;
            AddWorkspace(relationTypes.CurrencyIsoCode, new[] { "Default" });

            // Language;
            AddWorkspace(relationTypes.LanguageIsoCode, new[] { "Default" });
            AddWorkspace(relationTypes.LanguageName, new[] { "Default" });
            AddWorkspace(relationTypes.LanguageLocalisedNames, new[] { "Default" });
            AddWorkspace(relationTypes.LanguageNativeName, new[] { "Default" });

            // Locale;
            AddWorkspace(relationTypes.LocaleName, new[] { "Default" });
            AddWorkspace(relationTypes.LocaleLanguage, new[] { "Default" });
            AddWorkspace(relationTypes.LocaleCountry, new[] { "Default" });

            // LocalisedMedia;
            AddWorkspace(relationTypes.LocalisedMediaMedia, new[] { "Default" });

            // LocalisedText;
            AddWorkspace(relationTypes.LocalisedTextText, new[] { "Default" });

            // AutomatedAgent;

            // Person;
            AddWorkspace(relationTypes.PersonFirstName, new[] { "Default" });
            AddWorkspace(relationTypes.PersonMiddleName, new[] { "Default" });
            AddWorkspace(relationTypes.PersonLastName, new[] { "Default" });

            AddWorkspace(relationTypes.PersonMailboxAddress, new[] { "Default" });

            AddWorkspace(relationTypes.PersonBirthDate, new[] { "Default" });
            AddWorkspace(relationTypes.PersonFullName, new[] { "Default" });
            AddWorkspace(relationTypes.PersonDomainFullName, new[] { "Default" });
            AddWorkspace(relationTypes.PersonDomainGreeting, new[] { "Default" });
            AddWorkspace(relationTypes.PersonGender, new[] { "Default" });

            AddWorkspace(relationTypes.PersonIsStudent, new[] { "Default" });


            AddWorkspace(relationTypes.PersonPhoto, new[] { "Default" });
            AddWorkspace(relationTypes.PersonPictures, new[] { "Default" });



            AddWorkspace(relationTypes.PersonWeight, new[] { "Default" });
            AddWorkspace(relationTypes.PersonCycleOne, new[] { "Default" });
            AddWorkspace(relationTypes.PersonCycleMany, new[] { "Default" });

            // EmailMessage;
            AddWorkspace(relationTypes.EmailMessageDateCreated, new[] { "Default" });

            AddWorkspace(relationTypes.EmailMessageDateSent, new[] { "Default" });
            AddWorkspace(relationTypes.EmailMessageSender, new[] { "Default" });
            AddWorkspace(relationTypes.EmailMessageRecipients, new[] { "Default" });
            AddWorkspace(relationTypes.EmailMessageRecipientEmailAddress, new[] { "Default" });
            AddWorkspace(relationTypes.EmailMessageSubject, new[] { "Default" });
            AddWorkspace(relationTypes.EmailMessageBody, new[] { "Default" });

            // Notification;
            AddWorkspace(relationTypes.NotificationConfirmed, new[] { "Default" });
            AddWorkspace(relationTypes.NotificationTitle, new[] { "Default" });
            AddWorkspace(relationTypes.NotificationDescription, new[] { "Default" });
            AddWorkspace(relationTypes.NotificationDateCreated, new[] { "Default" });
            AddWorkspace(methodTypes.NotificationConfirm, new[] { "Default" });

            // NotificationList;
            AddWorkspace(relationTypes.NotificationListNotifications, new[] { "Default" });
            AddWorkspace(relationTypes.NotificationListUnconfirmedNotifications, new[] { "Default" });
            AddWorkspace(relationTypes.NotificationListConfirmedNotifications, new[] { "Default" });

            // TaskAssignment;
            AddWorkspace(relationTypes.TaskAssignmentUser, new[] { "Default" });

            AddWorkspace(relationTypes.TaskAssignmentTask, new[] { "Default" });

            // AccessClass;

            // Employment;
            AddWorkspace(relationTypes.EmploymentEmployee, new[] { "Default" });
            AddWorkspace(relationTypes.EmploymentEmployer, new[] { "Default" });

            // Gender;

            // Organisation;
            AddWorkspace(relationTypes.OrganisationEmployees, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationManager, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationOwner, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationShareholders, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationActiveEmployments, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationInactiveEmployments, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationActiveEmployees, new[] { "Default" });


            AddWorkspace(relationTypes.OrganisationIncorporationDate, new[] { "Default" });




            AddWorkspace(relationTypes.OrganisationName, new[] { "Default" });

            AddWorkspace(relationTypes.OrganisationCycleOne, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationCycleMany, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationOneData, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationManyDatas, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationJustDidIt, new[] { "Default" });
            AddWorkspace(relationTypes.OrganisationCountry, new[] { "Default" });
            AddWorkspace(methodTypes.OrganisationJustDoIt, new[] { "Default" });
            AddWorkspace(methodTypes.OrganisationToggleCanWrite, new[] { "Default" });

            // BadUI;

            // Build;

            // C1;
            AddWorkspace(relationTypes.C1C1AllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.C1C1AllorsString, new[] { "Default" });
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


            // C2;
            AddWorkspace(relationTypes.C2C2AllorsDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C2Many2One, new[] { "Default" });
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
            AddWorkspace(relationTypes.C2C2C2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I1One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C1Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I12One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I2One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C2One2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C1Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C2Many2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2I2Many2One, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2C1One2Manies, new[] { "Default" });
            AddWorkspace(relationTypes.C2C2AllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.C2S1One2One, new[] { "Default" });

            // ClassWithoutRoles;

            // Data;
            AddWorkspace(relationTypes.DataCheckbox, new[] { "Default" });
            AddWorkspace(relationTypes.DataChips, new[] { "Default" });
            AddWorkspace(relationTypes.DataString, new[] { "Default" });
            AddWorkspace(relationTypes.DataDecimal, new[] { "Default" });
            AddWorkspace(relationTypes.DataDate, new[] { "Default" });
            AddWorkspace(relationTypes.DataDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.DataDateTime2, new[] { "Default" });
            AddWorkspace(relationTypes.DataFile, new[] { "Default" });
            AddWorkspace(relationTypes.DataMonth, new[] { "Default" });
            AddWorkspace(relationTypes.DataMultipleFiles, new[] { "Default" });
            AddWorkspace(relationTypes.DataRadioGroup, new[] { "Default" });
            AddWorkspace(relationTypes.DataSlider, new[] { "Default" });
            AddWorkspace(relationTypes.DataSlideToggle, new[] { "Default" });
            AddWorkspace(relationTypes.DataPlainText, new[] { "Default" });
            AddWorkspace(relationTypes.DataMarkdown, new[] { "Default" });
            AddWorkspace(relationTypes.DataHtml, new[] { "Default" });
            AddWorkspace(relationTypes.DataStatic, new[] { "Default" });
            AddWorkspace(relationTypes.DataAutocompleteFilter, new[] { "Default" });
            AddWorkspace(relationTypes.DataAutocompleteOptions, new[] { "Default" });
            AddWorkspace(relationTypes.DataAutocompleteAssignedFilter, new[] { "Default" });
            AddWorkspace(relationTypes.DataAutocompleteDerivedFilter, new[] { "Default" });
            AddWorkspace(relationTypes.DataAutocompleteAssignedOptions, new[] { "Default" });
            AddWorkspace(relationTypes.DataAutocompleteDerivedOptions, new[] { "Default" });
            AddWorkspace(relationTypes.DataSelect, new[] { "Default" });
            AddWorkspace(relationTypes.DataSelectAssigned, new[] { "Default" });
            AddWorkspace(relationTypes.DataSelectDerived, new[] { "Default" });
            AddWorkspace(relationTypes.DataLocalisedTexts, new[] { "Default" });
            AddWorkspace(relationTypes.DataLocalisedMarkdowns, new[] { "Default" });

            // Dependee;

            // Dependent;

            // Extender;

            // First;

            // Four;

            // From;

            // HomeAddress;
            AddWorkspace(relationTypes.HomeAddressStreet, new[] { "Default" });
            AddWorkspace(relationTypes.HomeAddressHouseNumber, new[] { "Default" });

            // Left;

            // MailboxAddress;
            AddWorkspace(relationTypes.MailboxAddressPoBox, new[] { "Default" });

            // MediaTyped;
            AddWorkspace(relationTypes.MediaTypedMarkdown, new[] { "Default" });

            // Middle;

            // One;

            // Order;
            AddWorkspace(relationTypes.OrderCurrentVersion, new[] { "Default" });
            AddWorkspace(relationTypes.OrderAllVersions, new[] { "Default" });

            // OrderLine;
            AddWorkspace(relationTypes.OrderLineCurrentVersion, new[] { "Default" });
            AddWorkspace(relationTypes.OrderLineAllVersions, new[] { "Default" });

            // OrderLineVersion;

            // OrderState;

            // OrderVersion;

            // Page;
            AddWorkspace(relationTypes.PageName, new[] { "Default" });
            AddWorkspace(relationTypes.PageContent, new[] { "Default" });

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
            AddWorkspace(relationTypes.UnitSampleAllorsBinary, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsDateTime, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsBoolean, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsDouble, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsInteger, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsString, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsUnique, new[] { "Default" });
            AddWorkspace(relationTypes.UnitSampleAllorsDecimal, new[] { "Default" });









            // ValidationC1;

            // ValidationC2;

            // WorkspaceNoneObject1;
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

            // WorkspaceNonObject2;

            // WorkspaceXObject1;
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

            // WorkspaceXObject2;

            // WorkspaceXYObject1;
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

            // WorkspaceXYObject2;

            // WorkspaceYObject1;
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

            // WorkspaceYObject2;

            // Grant;

            // Login;

            // CreatePermission;

            // ExecutePermission;

            // ReadPermission;

            // WritePermission;

            // Revocation;

            // Role;

            // SecurityToken;

            // UserGroup;
            AddWorkspace(relationTypes.UserGroupMembers, new[] { "Default" });
            AddWorkspace(relationTypes.UserGroupName, new[] { "Default" });

            // Cacheable;

            // Enumeration;
            AddWorkspace(relationTypes.EnumerationKey, new[] { "Default" });
            AddWorkspace(relationTypes.EnumerationLocalisedNames, new[] { "Default" });
            AddWorkspace(relationTypes.EnumerationIsActive, new[] { "Default" });

            // Period;
            AddWorkspace(relationTypes.PeriodFromDate, new[] { "Default" });
            AddWorkspace(relationTypes.PeriodThroughDate, new[] { "Default" });

            // Version;
            AddWorkspace(relationTypes.VersionDerivationTimeStamp, new[] { "Default" });

            // Versioned;

            // Printable;
            AddWorkspace(relationTypes.PrintablePrintDocument, new[] { "Default" });
            AddWorkspace(methodTypes.PrintablePrint, new[] { "Default" });

            // Localised;
            AddWorkspace(relationTypes.LocalisedLocale, new[] { "Default" });

            // Auditable;
            AddWorkspace(relationTypes.AuditableCreatedBy, new[] { "Default" });
            AddWorkspace(relationTypes.AuditableLastModifiedBy, new[] { "Default" });
            AddWorkspace(relationTypes.AuditableCreationDate, new[] { "Default" });
            AddWorkspace(relationTypes.AuditableLastModifiedDate, new[] { "Default" });

            // ApproveTask;
            AddWorkspace(relationTypes.ApproveTaskComment, new[] { "Default" });


            AddWorkspace(methodTypes.ApproveTaskApprove, new[] { "Default" });
            AddWorkspace(methodTypes.ApproveTaskReject, new[] { "Default" });

            // ObjectState;
            AddWorkspace(relationTypes.ObjectStateName, new[] { "Default" });

            // Task;
            AddWorkspace(relationTypes.TaskWorkItem, new[] { "Default" });
            AddWorkspace(relationTypes.TaskTitle, new[] { "Default" });
            AddWorkspace(relationTypes.TaskDateCreated, new[] { "Default" });
            AddWorkspace(relationTypes.TaskDateDue, new[] { "Default" });
            AddWorkspace(relationTypes.TaskDateClosed, new[] { "Default" });
            AddWorkspace(relationTypes.TaskParticipants, new[] { "Default" });
            AddWorkspace(relationTypes.TaskPerformer, new[] { "Default" });

            // Transitional;

            // TransitionalVersion;

            // User;
            AddWorkspace(relationTypes.UserNotificationList, new[] { "Default" });
            AddWorkspace(relationTypes.UserUserName, new[] { "Default" });


            AddWorkspace(relationTypes.UserUserEmail, new[] { "Default" });











            // WorkItem;
            AddWorkspace(relationTypes.WorkItemWorkItemDescription, new[] { "Default" });

            // AccessInterface;

            // Address;

            // Addressable;
            AddWorkspace(relationTypes.AddressableAddress, new[] { "Default" });

            // DerivationCounted;

            // I1;
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

            // I12;
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

            // I2;
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

            // S1;

            // Shared;

            // SyncDepthI1;

            // ValidationI12;

            // Deletable;
            AddWorkspace(methodTypes.DeletableDelete, new[] { "Default" });

            // Object;

            // UniquelyIdentifiable;
            AddWorkspace(relationTypes.UniquelyIdentifiableUniqueId, new[] { "Default" });

            // DelegatedAccessObject;

            // Permission;

            // SecurityTokenOwner;

            // UserPasswordReset;
            AddWorkspace(relationTypes.UserPasswordResetInExistingUserPassword, new[] { "Default" });
            AddWorkspace(relationTypes.UserPasswordResetInUserPassword, new[] { "Default" });

            // Classes
            AddWorkspace(meta.Media, new[] { "Default" });
            AddWorkspace(meta.MediaContent, new[] { "Default" });
            AddWorkspace(meta.MediaTyped, new[] { "Default" });
            AddWorkspace(meta.Country, new[] { "Default" });
            AddWorkspace(meta.Gender, new[] { "Default" });
            AddWorkspace(meta.Language, new[] { "Default" });
            AddWorkspace(meta.Locale, new[] { "Default" });
            AddWorkspace(meta.LocalisedText, new[] { "Default" });
            AddWorkspace(meta.Person, new[] { "Default" });
            AddWorkspace(meta.TaskAssignment, new[] { "Default" });
            AddWorkspace(meta.Employment, new[] { "Default" });
            AddWorkspace(meta.MailboxAddress, new[] { "Default" });
            AddWorkspace(meta.Organisation, new[] { "Default" });
            AddWorkspace(meta.C1, new[] { "Default" });
            AddWorkspace(meta.C2, new[] { "Default" });
            AddWorkspace(meta.Data, new[] { "Default" });
            AddWorkspace(meta.HomeAddress, new[] { "Default" });
            AddWorkspace(meta.UnitSample, new[] { "Default" });
            AddWorkspace(meta.WorkspaceXObject1, new[] { "X" });
            AddWorkspace(meta.WorkspaceXObject2, new[] { "X" });
            AddWorkspace(meta.WorkspaceXYObject1, new[] { "X", "Y" });
            AddWorkspace(meta.WorkspaceXYObject2, new[] { "X", "Y" });
            AddWorkspace(meta.WorkspaceYObject1, new[] { "Y" });
            AddWorkspace(meta.WorkspaceYObject2, new[] { "Y" });
        }
    }
}
