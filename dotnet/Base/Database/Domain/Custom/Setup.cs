// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
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

        private void CustomOnPostSetup(Config config)
        {
            #region Plurals
            var revocations = new Revocations(this.transaction);
            var people = new People(this.transaction);
            var userGroups = new UserGroups(this.transaction);
            var organizations = new Organizations(this.transaction);
            var countries = new Countries(this.transaction);
            #endregion

            #region Builders
            Person BuildPerson(string firstName, string lastName, string userName, Media photo = null, Gender gender = null, Address address = null)
            {
                void Builder(Person v)
                {
                    v.FirstName = firstName;
                    v.LastName = lastName;
                    v.Photo = photo;
                    v.Address = address;
                }

                return people.Create(Builder);
            }

            Organization BuildOrganization(string name, Action<Organization> extraBuilder = null)
            {
                void Builder(Organization v) => v.Name = name;
                return organizations.Create(Builder, extraBuilder);
            }

            Revocation BuildRevocation(params Permission[] deniedPermissions) => revocations.Create(v => v.DeniedPermissions = deniedPermissions);

            #endregion

            var avatar = new Medias(this.transaction).Avatar;

            var place = this.transaction.Build<Place>(v =>
            {
                v.PostalCode = "X";
                v.City = "London";
                v.Country = countries.CountryByIsoCode["GB"];
            });

            var address = this.transaction.Build<HomeAddress>(v =>
            {
                v.Street = "Main Street";
                v.HouseNumber = "1";
                v.Place = place;
            });

            var genders = new Genders(this.transaction);

            var jane = BuildPerson("Jane", "Doe", "jane@example.com", avatar, genders.Female, address);
            var john = BuildPerson("John", "Doe", "john@example.com", avatar, genders.Male);
            var jenny = BuildPerson("Jenny", "Doe", "jenny@example.com", avatar, genders.Other);

            jane.SetPassword("jane");
            john.SetPassword("john");
            jenny.SetPassword("jenny");

            new UserGroups(this.transaction).Administrators.AddMember(jane);
            new UserGroups(this.transaction).Creators.AddMember(jane);
            new UserGroups(this.transaction).Creators.AddMember(john);
            new UserGroups(this.transaction).Creators.AddMember(jenny);

            var acme = BuildOrganization("Acme", v =>
            {
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
            var cycleOrganization1 = BuildOrganization("Organisatin Cycle One", v => v.IncorporationDate = DateTimeFactory.CreateDate(2000, 1, 1));
            var cycleOrganization2 = BuildOrganization("Organisatin Cycle Two", v => v.IncorporationDate = DateTimeFactory.CreateDate(2001, 1, 1));

            cycleOrganization1.AddShareholder(jane);
            cycleOrganization2.AddShareholder(jenny);

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
            var mediaTyped = this.transaction.Build<MediaTyped>(v => v.Markdown = @"
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
");

            if (this.Config.SetupSecurity)
            {
                this.transaction.Database.Services.Get<IPermissions>().Sync(this.transaction);
            }
        }
    }
}
