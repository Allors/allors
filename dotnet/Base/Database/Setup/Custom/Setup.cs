// <copyright file="Setup.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Linq;
    using Meta;
    using Services;

    public partial class Setup
    {
        private void CustomOnPrePrepare()
        {
        }

        private void CustomOnPostPrepare()
        {
        }

        private void CustomOnPreSetup()
        {
        }

        private void CustomOnPostSetup()
        {
            #region Plurals
            var countryByKey = this.transaction.Scoped<CountryByKey>();
            var genders = this.transaction.Scoped<GenderByKey>();
            var medias = this.transaction.Scoped<MediaByUniqueId>();
            var roles = this.transaction.Scoped<RoleByUniqueId>();
            var securityTokens = this.transaction.Scoped<SecurityTokenByUniqueId>();
            var userGroups = this.transaction.Scoped<UserGroupByUniqueId>();
            #endregion

            #region Builders
            Person BuildPerson(string firstName, string lastName, string userName, string password = null)
            {
                void Builder(Person v)
                {
                    v.FirstName = firstName;
                    v.LastName = lastName;
                    v.UserName = userName;
                    v.SetPassword(password);
                }

                return this.transaction.Build<Person>(Builder);
            }

            Organization BuildOrganization(string name, Action<Organization> extraBuilder = null)
            {
                void Builder(Organization v) => v.Name = name;
                return this.transaction.Build<Organization>(Builder, extraBuilder);
            }

            Revocation BuildRevocation(params Permission[] deniedPermissions) => this.transaction.Build<Revocation>(v => v.DeniedPermissions = deniedPermissions);

            TrimFrom BuildTrimFrom(string name, Action<TrimFrom> extraBuilder = null)
            {
                void Builder(TrimFrom v) => v.Name = name;
                return this.transaction.Build<TrimFrom>(Builder, extraBuilder);
            }

            TrimTo BuildTrimTo(string name, Action<TrimTo> extraBuilder = null)
            {
                void Builder(TrimTo v) => v.Name = name;
                return this.transaction.Build<TrimTo>(Builder, extraBuilder);
            }

            #endregion

            var avatar = medias.Avatar;

            var place = this.transaction.Build<Place>(v =>
            {
                v.PostalCode = "X";
                v.City = "London";
                v.Country = countryByKey["GB"];
            });

            var address = this.transaction.Build<HomeAddress>(v =>
            {
                v.Street = "Main Street";
                v.HouseNumber = "1";
                v.Place = place;
            });
            
            var mailboxAdress = this.transaction.Build<MailboxAddress>(v =>
            {
                v.Place = this.transaction.Build<Place>(w =>
                {
                    w.City = "De Haan";
                    w.PostalCode = "8420";
                    w.Country = countryByKey["BE"];
                });
                v.PoBox = "P.O. Box 20";
            });

            var jane = BuildPerson("Jane", "Doe", "jane@example.com", "jane");
            var john = BuildPerson("John", "Doe", "john@example.com", "john");
            var jenny = BuildPerson("Jenny", "Doe", "jenny@example.com", "jenny");

            var guest = BuildPerson("Gu", "est", "guest@example.com");

            userGroups.Administrators.AddMember(jane);
            userGroups.Creators.AddMember(jane);
            userGroups.Creators.AddMember(john);
            userGroups.Creators.AddMember(jenny);

            var acme = this.transaction.Build<Organization>(v =>
            {
                v.Name = "Acme";
                v.Owner = jane;
                v.AddEmployee(john);
                v.AddEmployee(jenny);
                v.IncorporationDate = this.transaction.Now();
            });

            acme.Owner = jenny;
            acme.Manager = jane;
            acme.AddShareholder(jane);
            acme.AddShareholder(jenny);

            this.transaction.Build<Employment>(v =>
            {
                v.Employer = acme;
                v.Employee = jane;
            });

            var now = this.transaction.Now();

            this.transaction.Build<Employment>(v =>
            {
                v.Employer = acme;
                v.Employee = john;
                v.FromDate = now.AddDays(-2);
                v.ThroughDate = now.AddDays(-1);
            });


            // Create cycles between Organization and Person
            var cycleOrganization1 = BuildOrganization("Organization Cycle One");
            var cycleOrganization2 = BuildOrganization("Organization Cycle Two");

            var cyclePerson1 = BuildPerson("Person Cycle", "One", "cycle1@one.org");
            var cyclePerson2 = BuildPerson("Person Cycle", "Two", "cycle2@one.org");

            // One
            cycleOrganization1.CycleOne = cyclePerson1;
            cyclePerson1.CycleOne = cycleOrganization1;

            cycleOrganization2.CycleOne = cyclePerson2;
            cyclePerson2.CycleOne = cycleOrganization2;

            // Many
            cycleOrganization1.AddCycleMany(cyclePerson1);
            cycleOrganization1.AddCycleMany(cyclePerson2);

            cycleOrganization1.AddCycleMany(cyclePerson1);
            cycleOrganization1.AddCycleMany(cyclePerson2);

            cyclePerson1.AddCycleMany(cycleOrganization1);
            cyclePerson1.AddCycleMany(cycleOrganization2);

            cyclePerson2.AddCycleMany(cycleOrganization1);
            cyclePerson2.AddCycleMany(cycleOrganization2);

            // MediaTyped
            var mediaTyped = this.transaction.Build<MediaTyped>(v =>
            {
                v.Markdown = @"
# Markdown
1.  List item one.

    List item one continued with a second paragraph followed by an
    Indented block.

        $ ls *.sh
        $ mv *.sh ~/tmp

    List item continued with a third paragraph.

2.  List item two continued with an open block.

    This paragraph is part of the preceding list item.

    1. This list is nested and does not require explicit item continuation.

       This paragraph is part of the preceding list item.

    2. List item b.

    This paragraph belongs to item two of the outer list.
";
            });

            if (this.Config.SetupSecurity)
            {
                this.transaction.Database.Services.Get<IPermissions>().Sync(this.transaction);

                // Denied
                var denied = this.transaction.Build<Denied>(denied =>
                {
                    denied.DatabaseProperty = "DatabaseProp";
                    denied.DefaultWorkspaceProperty = "DefaultWorkspaceProp";
                    denied.WorkspaceXProperty = "WorkspaceXProp";
                });

                var m = denied.M;

                var databaseWrite = this.transaction.Extent<Permission>().First(v => v.Operation == Operations.Write && v.OperandType.Equals(m.Denied.DatabaseProperty));
                var defaultWorkspaceWrite = this.transaction.Extent<Permission>().First(v => v.Operation == Operations.Write && v.OperandType.Equals(m.Denied.DefaultWorkspaceProperty));
                var workspaceXWrite = this.transaction.Extent<Permission>().First(v => v.Operation == Operations.Write && v.OperandType.Equals(m.Denied.WorkspaceXProperty));

                var revocation = BuildRevocation(databaseWrite, defaultWorkspaceWrite, workspaceXWrite);

                denied.AddRevocation(revocation);
            }

            // Trimming
            if (this.Config.SetupSecurity)
            {
                // Objects
                var fromTrimmed1 = BuildTrimFrom("Trimmed1");
                var fromTrimmed2 = BuildTrimFrom("Trimmed2");
                var fromUntrimmed1 = BuildTrimFrom("Untrimmed1");
                var fromUntrimmed2 = BuildTrimFrom("Untrimmed2");

                var toTrimmed = BuildTrimTo("Trimmed1");
                var toUntrimmed = BuildTrimTo("Untrimmed1");

                var m = this.transaction.Database.Services.Get<IMetaIndex>();

                // Denied Permissions
                var fromTrimPermission = this.transaction.Extent<Permission>().First(v => v.Operation == Operations.Read && v.OperandType.Equals(m.TrimFrom.Name));
                var fromRevocation = BuildRevocation(fromTrimPermission);
                fromTrimmed1.AddRevocation(fromRevocation);
                fromTrimmed2.AddRevocation(fromRevocation);

                var toTrimPermission = this.transaction.Extent<Permission>().First(v => v.Operation == Operations.Read && v.OperandType.Equals(m.TrimTo.Name));
                var toRevocation = BuildRevocation(toTrimPermission);
                toTrimmed.AddRevocation(toRevocation);

                // Relations
                fromTrimmed1.Many2One = toTrimmed;
                fromTrimmed2.Many2One = toUntrimmed;
                fromUntrimmed1.Many2One = toTrimmed;
                fromUntrimmed2.Many2One = toUntrimmed;

                fromTrimmed1.AddMany2Many(toTrimmed);
                fromTrimmed2.AddMany2Many(toUntrimmed);
                fromUntrimmed1.AddMany2Many(toTrimmed);
                fromUntrimmed2.AddMany2Many(toUntrimmed);
            }
        }

        private void CustomOnCreated(IObject @object)
        {

            
        }
    }
}
