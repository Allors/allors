﻿namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Tests.Domain;

    public class ManyToManyTests : Tests
    {
        [Test]
        public void AddSingleActiveLink()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.Employees.Add(jane);
            acme.Employees.Add(john);
            acme.Employees.Add(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, john.OrganizationsWhereEmployee.Value);

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.Contains(jane, acme.Employees.Value);
            Assert.Contains(john, acme.Employees.Value);
            Assert.Contains(jenny, acme.Employees.Value);

            Assert.IsEmpty(hooli.Employees.Value);
        }

        [Test]
        public void SetSingleActiveLink()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.Employees.Value = new[] { jane };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);

            Assert.IsEmpty(john.OrganizationsWhereEmployee.Value);

            Assert.IsEmpty(jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(1));
            Assert.Contains(jane, acme.Employees.Value);

            Assert.IsEmpty(hooli.Employees.Value);

            acme.Employees.Value = new[] { jane, john };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, john.OrganizationsWhereEmployee.Value);

            Assert.IsEmpty(jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(2));
            Assert.Contains(jane, acme.Employees.Value);
            Assert.Contains(john, acme.Employees.Value);

            Assert.IsEmpty(hooli.Employees.Value);

            acme.Employees.Value = new[] { jane, john, jenny };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, john.OrganizationsWhereEmployee.Value);

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.Contains(jane, acme.Employees.Value);
            Assert.Contains(john, acme.Employees.Value);
            Assert.Contains(jenny, acme.Employees.Value);

            Assert.IsEmpty(hooli.Employees.Value);

            acme.Employees.Value = new Person[] { };

            Assert.IsEmpty(jane.OrganizationsWhereEmployee.Value);
            Assert.IsEmpty(john.OrganizationsWhereEmployee.Value);
            Assert.IsEmpty(jenny.OrganizationsWhereEmployee.Value);

            Assert.IsEmpty(acme.Employees.Value);
            Assert.IsEmpty(hooli.Employees.Value);
        }

        [Test]
        public void RemoveSingleActiveLink()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.Employees.Value = new[] { jane, john, jenny };

            acme.Employees.Remove(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, john.OrganizationsWhereEmployee.Value);

            Assert.IsEmpty(jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(2));
            Assert.Contains(jane, acme.Employees.Value);
            Assert.Contains(john, acme.Employees.Value);

            Assert.IsEmpty(hooli.Employees.Value);

            acme.Employees.Remove(john);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);

            Assert.IsEmpty(john.OrganizationsWhereEmployee.Value);

            Assert.IsEmpty(jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(1));
            Assert.Contains(jane, acme.Employees.Value);

            Assert.IsEmpty(hooli.Employees.Value);

            acme.Employees.Remove(jane);

            Assert.IsEmpty(jane.OrganizationsWhereEmployee.Value);
            Assert.IsEmpty(john.OrganizationsWhereEmployee.Value);
            Assert.IsEmpty(jenny.OrganizationsWhereEmployee.Value);

            Assert.IsEmpty(acme.Employees.Value);
            Assert.IsEmpty(hooli.Employees.Value);
        }

        [Test]
        public void MultipeleActiveLinks()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.Employees.Add(jane);
            acme.Employees.Add(john);
            acme.Employees.Add(jenny);

            hooli.Employees.Add(jane);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);
            Assert.Contains(hooli, jane.OrganizationsWhereEmployee.Value);

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, john.OrganizationsWhereEmployee.Value);

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.Contains(jane, acme.Employees.Value);
            Assert.Contains(john, acme.Employees.Value);
            Assert.Contains(jenny, acme.Employees.Value);

            Assert.That(hooli.Employees.Value.Length, Is.EqualTo(1));
            Assert.Contains(jane, hooli.Employees.Value);

            hooli.Employees.Add(john);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);
            Assert.Contains(hooli, jane.OrganizationsWhereEmployee.Value);

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.Contains(acme, john.OrganizationsWhereEmployee.Value);
            Assert.Contains(hooli, john.OrganizationsWhereEmployee.Value);

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.Contains(jane, acme.Employees.Value);
            Assert.Contains(john, acme.Employees.Value);
            Assert.Contains(jenny, acme.Employees.Value);

            Assert.That(hooli.Employees.Value.Length, Is.EqualTo(2));
            Assert.Contains(jane, hooli.Employees.Value);
            Assert.Contains(john, hooli.Employees.Value);

            hooli.Employees.Add(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.Contains(acme, jane.OrganizationsWhereEmployee.Value);
            Assert.Contains(hooli, jane.OrganizationsWhereEmployee.Value);

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.Contains(acme, john.OrganizationsWhereEmployee.Value);
            Assert.Contains(hooli, john.OrganizationsWhereEmployee.Value);

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee.Value);
            Assert.Contains(hooli, jenny.OrganizationsWhereEmployee.Value);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.Contains(jane, acme.Employees.Value);
            Assert.Contains(john, acme.Employees.Value);
            Assert.Contains(jenny, acme.Employees.Value);

            Assert.That(hooli.Employees.Value.Length, Is.EqualTo(3));
            Assert.Contains(jane, hooli.Employees.Value);
            Assert.Contains(john, hooli.Employees.Value);
            Assert.Contains(jenny, hooli.Employees.Value);
        }
    }
}
