namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Meta;
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

            acme.AddEmployee(jane);
            acme.AddEmployee(john);
            acme.AddEmployee(jenny);

            Assert.AreEqual(1, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);

            Assert.AreEqual(1, john.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, john.OrganizationsWhereEmployee);

            Assert.AreEqual(1, jenny.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(3, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);
            Assert.Contains(john, acme.Employees);
            Assert.Contains(jenny, acme.Employees);

            Assert.IsEmpty(hooli.Employees);
        }

        [Test]
        public void SetSingleActiveLink()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.Employees = new[] { jane };

            Assert.AreEqual(1, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);

            Assert.IsEmpty(john.OrganizationsWhereEmployee);

            Assert.IsEmpty(jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(1, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);

            Assert.IsEmpty(hooli.Employees);

            acme.Employees = new[] { jane, john };

            Assert.AreEqual(1, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);

            Assert.AreEqual(1, john.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, john.OrganizationsWhereEmployee);

            Assert.IsEmpty(jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(2, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);
            Assert.Contains(john, acme.Employees);

            Assert.IsEmpty(hooli.Employees);

            acme.Employees = new[] { jane, john, jenny };

            Assert.AreEqual(1, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);

            Assert.AreEqual(1, john.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, john.OrganizationsWhereEmployee);

            Assert.AreEqual(1, jenny.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(3, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);
            Assert.Contains(john, acme.Employees);
            Assert.Contains(jenny, acme.Employees);

            Assert.IsEmpty(hooli.Employees);

            acme.Employees = new Person[] { };

            Assert.IsEmpty(jane.OrganizationsWhereEmployee);
            Assert.IsEmpty(john.OrganizationsWhereEmployee);
            Assert.IsEmpty(jenny.OrganizationsWhereEmployee);

            Assert.IsEmpty(acme.Employees);
            Assert.IsEmpty(hooli.Employees);
        }

        [Test]
        public void RemoveSingleActiveLink()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.Employees = new[] { jane, john, jenny };

            acme.RemoveEmployee(jenny);

            Assert.AreEqual(1, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);

            Assert.AreEqual(1, john.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, john.OrganizationsWhereEmployee);

            Assert.IsEmpty(jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(2, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);
            Assert.Contains(john, acme.Employees);

            Assert.IsEmpty(hooli.Employees);

            acme.RemoveEmployee(john);

            Assert.AreEqual(1, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);

            Assert.IsEmpty(john.OrganizationsWhereEmployee);

            Assert.IsEmpty(jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(1, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);

            Assert.IsEmpty(hooli.Employees);

            acme.RemoveEmployee(jane);

            Assert.IsEmpty(jane.OrganizationsWhereEmployee);
            Assert.IsEmpty(john.OrganizationsWhereEmployee);
            Assert.IsEmpty(jenny.OrganizationsWhereEmployee);

            Assert.IsEmpty(acme.Employees);
            Assert.IsEmpty(hooli.Employees);
        }

        [Test]
        public void MultipeleActiveLinks()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.AddEmployee(jane);
            acme.AddEmployee(john);
            acme.AddEmployee(jenny);

            hooli.AddEmployee(jane);

            Assert.AreEqual(2, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);
            Assert.Contains(hooli, jane.OrganizationsWhereEmployee);

            Assert.AreEqual(1, john.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, john.OrganizationsWhereEmployee);

            Assert.AreEqual(1, jenny.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(3, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);
            Assert.Contains(john, acme.Employees);
            Assert.Contains(jenny, acme.Employees);

            Assert.AreEqual(1, hooli.Employees.Length);
            Assert.Contains(jane, hooli.Employees);

            hooli.AddEmployee(john);

            Assert.AreEqual(2, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);
            Assert.Contains(hooli, jane.OrganizationsWhereEmployee);

            Assert.AreEqual(2, john.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, john.OrganizationsWhereEmployee);
            Assert.Contains(hooli, john.OrganizationsWhereEmployee);

            Assert.AreEqual(1, jenny.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(3, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);
            Assert.Contains(john, acme.Employees);
            Assert.Contains(jenny, acme.Employees);

            Assert.AreEqual(2, hooli.Employees.Length);
            Assert.Contains(jane, hooli.Employees);
            Assert.Contains(john, hooli.Employees);

            hooli.AddEmployee(jenny);

            Assert.AreEqual(2, jane.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jane.OrganizationsWhereEmployee);
            Assert.Contains(hooli, jane.OrganizationsWhereEmployee);

            Assert.AreEqual(2, john.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, john.OrganizationsWhereEmployee);
            Assert.Contains(hooli, john.OrganizationsWhereEmployee);

            Assert.AreEqual(2, jenny.OrganizationsWhereEmployee.Length);
            Assert.Contains(acme, jenny.OrganizationsWhereEmployee);
            Assert.Contains(hooli, jenny.OrganizationsWhereEmployee);

            Assert.AreEqual(3, acme.Employees.Length);
            Assert.Contains(jane, acme.Employees);
            Assert.Contains(john, acme.Employees);
            Assert.Contains(jenny, acme.Employees);

            Assert.AreEqual(3, hooli.Employees.Length);
            Assert.Contains(jane, hooli.Employees);
            Assert.Contains(john, hooli.Employees);
            Assert.Contains(jenny, hooli.Employees);
        }
    }
}
