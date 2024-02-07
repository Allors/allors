namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Tests.Domain;

    public abstract class ManyToManyTests : Tests
    {
        [Test]
        public void AddInterface()
        {
            var c1 = this.Population.New<C1>();
            var c2 = this.Population.New<C2>();
            
            c1.ManyToMany.Add(c2);

            Assert.That(c1.ManyToMany.Value.Length, Is.EqualTo(1));
            Assert.That(c1.ManyToMany.Value, Does.Contain(c2));

            Assert.That(c2.Backs.Value.Length, Is.EqualTo(1));
            Assert.That(c2.Backs.Value, Does.Contain(c1));
        }

        [Test]
        public void Add()
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
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value, Is.Empty);
        }

        [Test]
        public void Set()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.Employees.Value = new[] { jane };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(1));
            Assert.That(acme.Employees.Value, Does.Contain(jane));

            Assert.That(hooli.Employees.Value, Is.Empty);

            acme.Employees.Value = new[] { jane, john };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(2));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));

            Assert.That(hooli.Employees.Value, Is.Empty);

            acme.Employees.Value = new[] { jane, john, jenny };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value, Is.Empty);

            acme.Employees.Value = new Person[] { };

            Assert.That(jane.OrganizationsWhereEmployee.Value, Is.Empty);
            Assert.That(john.OrganizationsWhereEmployee.Value, Is.Empty);
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value, Is.Empty);
            Assert.That(hooli.Employees.Value, Is.Empty);
        }

        [Test]
        public void Remove()
        {
            var acme = this.Population.New<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.New<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();
            var jenny = this.Population.New<Person>();

            acme.Employees.Value = new[] { jane, john, jenny };

            acme.Employees.Remove(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(2));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));

            Assert.That(hooli.Employees.Value, Is.Empty);

            acme.Employees.Remove(john);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(1));
            Assert.That(acme.Employees.Value, Does.Contain(jane));

            Assert.That(hooli.Employees.Value, Is.Empty);

            acme.Employees.Remove(jane);

            Assert.That(jane.OrganizationsWhereEmployee.Value, Is.Empty);
            Assert.That(john.OrganizationsWhereEmployee.Value, Is.Empty);
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value, Is.Empty);
            Assert.That(hooli.Employees.Value, Is.Empty);
        }

        [Test]
        public void Multipele()
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
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value.Length, Is.EqualTo(1));
            Assert.That(hooli.Employees.Value, Does.Contain(jane));

            hooli.Employees.Add(john);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value.Length, Is.EqualTo(2));
            Assert.That(hooli.Employees.Value, Does.Contain(jane));
            Assert.That(hooli.Employees.Value, Does.Contain(john));

            hooli.Employees.Add(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Length, Is.EqualTo(2));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(acme.Employees.Value.Length, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value.Length, Is.EqualTo(3));
            Assert.That(hooli.Employees.Value, Does.Contain(jane));
            Assert.That(hooli.Employees.Value, Does.Contain(john));
            Assert.That(hooli.Employees.Value, Does.Contain(jenny));
        }
    }
}
