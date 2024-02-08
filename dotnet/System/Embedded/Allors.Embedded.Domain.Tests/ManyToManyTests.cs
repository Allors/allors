namespace Allors.Embedded.Domain.Tests
{
    public abstract class ManyToManyTests : Tests
    {
        [Test]
        public void AddInterface()
        {
            var c1 = this.Population.EmbeddedCreateObject<C1>();
            var c2 = this.Population.EmbeddedCreateObject<C2>();

            c1.ManyToMany.Add(c2);

            Assert.That(c1.ManyToMany.Value.Count, Is.EqualTo(1));
            Assert.That(c1.ManyToMany.Value, Does.Contain(c2));

            Assert.That(c2.Backs.Value.Count, Is.EqualTo(1));
            Assert.That(c2.Backs.Value, Does.Contain(c1));
        }

        [Test]
        public void Add()
        {
            var acme = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jenny = this.Population.EmbeddedCreateObject<Person>();

            acme.Employees.Add(jane);
            acme.Employees.Add(john);
            acme.Employees.Add(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value, Is.Empty);
        }

        [Test]
        public void Set()
        {
            var acme = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jenny = this.Population.EmbeddedCreateObject<Person>();

            acme.Employees.Value = new[] { jane };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(1));
            Assert.That(acme.Employees.Value, Does.Contain(jane));

            Assert.That(hooli.Employees.Value, Is.Empty);

            acme.Employees.Value = new[] { jane, john };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(2));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));

            Assert.That(hooli.Employees.Value, Is.Empty);

            acme.Employees.Value = new[] { jane, john, jenny };

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(3));
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
            var acme = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jenny = this.Population.EmbeddedCreateObject<Person>();

            acme.Employees.Value = new[] { jane, john, jenny };

            acme.Employees.Remove(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(2));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));

            Assert.That(hooli.Employees.Value, Is.Empty);

            acme.Employees.Remove(john);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(jenny.OrganizationsWhereEmployee.Value, Is.Empty);

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(1));
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
            var acme = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.Value = "Acme");
            var hooli = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.Value = "Hooli");

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jenny = this.Population.EmbeddedCreateObject<Person>();

            acme.Employees.Add(jane);
            acme.Employees.Add(john);
            acme.Employees.Add(jenny);

            hooli.Employees.Add(jane);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(2));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value.Count, Is.EqualTo(1));
            Assert.That(hooli.Employees.Value, Does.Contain(jane));

            hooli.Employees.Add(john);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(2));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(2));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value.Count, Is.EqualTo(2));
            Assert.That(hooli.Employees.Value, Does.Contain(jane));
            Assert.That(hooli.Employees.Value, Does.Contain(john));

            hooli.Employees.Add(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(2));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(2));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(john.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(jenny.OrganizationsWhereEmployee.Value.Count, Is.EqualTo(2));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(acme));
            Assert.That(jenny.OrganizationsWhereEmployee.Value, Does.Contain(hooli));

            Assert.That(acme.Employees.Value.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.Value, Does.Contain(jane));
            Assert.That(acme.Employees.Value, Does.Contain(john));
            Assert.That(acme.Employees.Value, Does.Contain(jenny));

            Assert.That(hooli.Employees.Value.Count, Is.EqualTo(3));
            Assert.That(hooli.Employees.Value, Does.Contain(jane));
            Assert.That(hooli.Employees.Value, Does.Contain(john));
            Assert.That(hooli.Employees.Value, Does.Contain(jenny));
        }
    }
}
