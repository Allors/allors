// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
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
            var countryByIsoCode = this.transaction.Scoped<CountryByIsoCode>();
            var genders = this.transaction.Scoped<GenderByUniqueId>();
            var medias = this.transaction.Scoped<MediaByUniqueId>();
            var roles = this.transaction.Scoped<RoleByUniqueId>();
            var securityTokens = this.transaction.Scoped<SecurityTokenByUniqueId>();
            var userGroups = this.transaction.Scoped<UserGroupByUniqueId>();

            var administratorRole = roles.Administrator;
            var administrators = userGroups.Administrators;
            var defaultSecurityToken = securityTokens.DefaultSecurityToken;

            var acl = this.transaction.Build<Grant>(v =>
            {
                v.Role = administratorRole;
                v.AddSubjectGroup(administrators);
                v.AddSecurityToken(defaultSecurityToken);
            });

            var avatar = medias.Avatar;

            var place = this.transaction.Build<Place>(v =>
            {
                v.PostalCode = "X";
                v.City = "London";
                v.Country = countryByIsoCode["GB"];
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
                    w.Country = countryByIsoCode["BE"];
                });
                v.PoBox = "P.O. Box 20";
            });

            var jane = this.transaction.Build<Person>(v =>
            {
                v.MainAddress = address;
                v.FirstName = "Jane";
                v.LastName = "Doe";
                v.UserName = "jane@example.com";
                v.Photo = avatar;
                v.Gender = genders.Female;
                v.MailboxAddress = mailboxAdress;
            });

            var john = this.transaction.Build<Person>(v =>
            {
                v.FirstName = "John";
                v.LastName = "Doe";
                v.UserName = "john@example.com";
                v.Photo = avatar;
                v.Gender = genders.Male;
                v.MailboxAddress = mailboxAdress;
            });

            var jenny = this.transaction.Build<Person>(v =>
            {
                v.FirstName = "Jenny";
                v.LastName = "Doe";
                v.UserName = "jenny@example.com";
                v.Photo = avatar;
                v.Gender = genders.Other;
                v.MailboxAddress = mailboxAdress;
            });

            jane.SetPassword("jane");
            john.SetPassword("john");
            jenny.SetPassword("jenny");

            administrators.AddMember(jane);

            userGroups.Creators.AddMember(jane);
            userGroups.Creators.AddMember(john);
            userGroups.Creators.AddMember(jenny);

            var acme = this.transaction.Build<Organisation>(v =>
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

            // Create cycles between Organisation and Person
            var cycleOrganisation1 = this.transaction.Build<Organisation>(v =>
            {
                v.Name = "Organisatin Cycle One";
                v.IncorporationDate = DateTimeFactory.CreateDate(2000, 1, 1);
            });

            var cycleOrganisation2 = this.transaction.Build<Organisation>(v =>
            {
                v.Name = "Organisatin Cycle Two";
                v.IncorporationDate = DateTimeFactory.CreateDate(2001, 1, 1);
            });

            cycleOrganisation1.AddShareholder(jane);
            cycleOrganisation2.AddShareholder(jenny);

            var cyclePerson1 = this.transaction.Build<Person>(v =>
            {
                v.FirstName = "Person Cycle";
                v.LastName = "One";
                v.UserName = "cycle1@one.org";
            });

            var cyclePerson2 = this.transaction.Build<Person>(v =>
            {
                v.FirstName = "Person Cycle";
                v.LastName = "Two";
                v.UserName = "cycle2@one.org";
            });

            // One
            cycleOrganisation1.CycleOne = cyclePerson1;
            cyclePerson1.CycleOne = cycleOrganisation1;

            cycleOrganisation2.CycleOne = cyclePerson2;
            cyclePerson2.CycleOne = cycleOrganisation2;

            // Many
            cycleOrganisation1.AddCycleMany(cyclePerson1);
            cycleOrganisation1.AddCycleMany(cyclePerson2);

            cycleOrganisation1.AddCycleMany(cyclePerson1);
            cycleOrganisation1.AddCycleMany(cyclePerson2);

            cyclePerson1.AddCycleMany(cycleOrganisation1);
            cyclePerson1.AddCycleMany(cycleOrganisation2);

            cyclePerson2.AddCycleMany(cycleOrganisation1);
            cyclePerson2.AddCycleMany(cycleOrganisation2);

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
            }
        }
    }
}
