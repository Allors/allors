namespace Allors.Embedded.Domain.Tests
{
    public abstract class ManyToManyTests : Tests
    {
        [Test]
        public void AddInterface()
        {
            var c1 = this.Population.EmbeddedCreateObject<C1>();
            var c2 = this.Population.EmbeddedCreateObject<C2>();

            c1.ManyToMany.EmbeddedAdd(c2);

            Assert.That(c1.ManyToMany.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(c1.ManyToMany.EmbeddedValue, Does.Contain(c2));

            Assert.That(c2.Backs.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(c2.Backs.EmbeddedValue, Does.Contain(c1));
        }

        [Test]
        public void Add()
        {
            var acme = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.EmbeddedValue = "Acme");
            var hooli = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.EmbeddedValue = "Hooli");

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jenny = this.Population.EmbeddedCreateObject<Person>();

            acme.Employees.EmbeddedAdd(jane);
            acme.Employees.EmbeddedAdd(john);
            acme.Employees.EmbeddedAdd(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(john));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jenny));

            Assert.That(hooli.Employees.EmbeddedValue, Is.Empty);
        }

        [Test]
        public void Set()
        {
            var acme = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.EmbeddedValue = "Acme");
            var hooli = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.EmbeddedValue = "Hooli");

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jenny = this.Population.EmbeddedCreateObject<Person>();

            acme.Employees.EmbeddedValue = new[] { jane };

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));

            Assert.That(hooli.Employees.EmbeddedValue, Is.Empty);

            acme.Employees.EmbeddedValue = new[] { jane, john };

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(john));

            Assert.That(hooli.Employees.EmbeddedValue, Is.Empty);

            acme.Employees.EmbeddedValue = new[] { jane, john, jenny };

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(john));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jenny));

            Assert.That(hooli.Employees.EmbeddedValue, Is.Empty);

            acme.Employees.EmbeddedValue = new Person[] { };

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);
            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);

            Assert.That(acme.Employees.EmbeddedValue, Is.Empty);
            Assert.That(hooli.Employees.EmbeddedValue, Is.Empty);
        }

        [Test]
        public void Remove()
        {
            var acme = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.EmbeddedValue = "Acme");
            var hooli = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.EmbeddedValue = "Hooli");

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jenny = this.Population.EmbeddedCreateObject<Person>();

            acme.Employees.EmbeddedValue = new[] { jane, john, jenny };

            acme.Employees.EmbeddedRemove(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(john));

            Assert.That(hooli.Employees.EmbeddedValue, Is.Empty);

            acme.Employees.EmbeddedRemove(john);

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));

            Assert.That(hooli.Employees.EmbeddedValue, Is.Empty);

            acme.Employees.EmbeddedRemove(jane);

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);
            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Is.Empty);

            Assert.That(acme.Employees.EmbeddedValue, Is.Empty);
            Assert.That(hooli.Employees.EmbeddedValue, Is.Empty);
        }

        [Test]
        public void Multipele()
        {
            var acme = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.EmbeddedValue = "Acme");
            var hooli = this.Population.EmbeddedCreateObject<Organization>(v => v.Name.EmbeddedValue = "Hooli");

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jenny = this.Population.EmbeddedCreateObject<Person>();

            acme.Employees.EmbeddedAdd(jane);
            acme.Employees.EmbeddedAdd(john);
            acme.Employees.EmbeddedAdd(jenny);

            hooli.Employees.EmbeddedAdd(jane);

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(john));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jenny));

            Assert.That(hooli.Employees.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(hooli.Employees.EmbeddedValue, Does.Contain(jane));

            hooli.Employees.EmbeddedAdd(john);

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(hooli));

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(1));
            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(john));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jenny));

            Assert.That(hooli.Employees.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(hooli.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(hooli.Employees.EmbeddedValue, Does.Contain(john));

            hooli.Employees.EmbeddedAdd(jenny);

            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));
            Assert.That(jane.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(hooli));

            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));
            Assert.That(john.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(hooli));

            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue.Count, Is.EqualTo(2));
            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(acme));
            Assert.That(jenny.OrganizationsWhereEmployee.EmbeddedValue, Does.Contain(hooli));

            Assert.That(acme.Employees.EmbeddedValue.Count, Is.EqualTo(3));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(john));
            Assert.That(acme.Employees.EmbeddedValue, Does.Contain(jenny));

            Assert.That(hooli.Employees.EmbeddedValue.Count, Is.EqualTo(3));
            Assert.That(hooli.Employees.EmbeddedValue, Does.Contain(jane));
            Assert.That(hooli.Employees.EmbeddedValue, Does.Contain(john));
            Assert.That(hooli.Employees.EmbeddedValue, Does.Contain(jenny));
        }
    }
}
