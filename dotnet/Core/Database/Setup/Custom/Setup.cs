// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Linq;
    using Allors.Database.Meta;
    using Allors.Database.Services;

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

        private void CustomOnPostSetup(Config config)
        {
            #region Plurals
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

            var jane = BuildPerson("Jane", "Doe", "jane@example.com", "jane");
            var john = BuildPerson("John", "Doe", "john@example.com", "john");
            var jenny = BuildPerson("Jenny", "Doe", "jenny@example.com", "jenny");

            var guest = BuildPerson("Gu", "est", "guest@example.com");

            userGroups.Administrators.AddMember(jane);
            userGroups.Creators.AddMember(jane);
            userGroups.Creators.AddMember(john);
            userGroups.Creators.AddMember(jenny);

            var acme = BuildOrganization("Acme", organization =>
                {
                    organization.Owner = jane;
                    organization.AddEmployee(john);
                    organization.AddEmployee(jenny);
                });

            for (var i = 0; i < 100; i++)
            {
                BuildOrganization($"Organization-{i}", organization =>
                {
                    organization.Owner = john;
                    organization.AddEmployee(jenny);
                    organization.AddEmployee(jane);
                });
            }

            // Create cycles between Organization and Person
            var cycleOrganization1 = BuildOrganization("Organisatin Cycle One");
            var cycleOrganization2 = BuildOrganization("Organisatin Cycle Two");

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

            // Security
            if (this.Config.SetupSecurity)
            {
                this.transaction.Database.Services.Get<IPermissions>().Sync(this.transaction);

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

                var m = this.transaction.Database.Services.Get<M>();

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
    }
}
