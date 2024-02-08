namespace Allors.Embedded.Domain.Memory.Tests
{
    using Domain.Memory;
    
    public class SnapshotTests : Tests
    {
        private EmbeddedPopulation population = null!;

        public override EmbeddedPopulation Population => population;

        [SetUp]
        public override void SetUp()
        {
            this.population = new EmbeddedPopulation();

            base.SetUp();
        }

        [Test]
        public void Unit()
        {
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jane = this.Population.EmbeddedCreateObject<Person>();

            john.FirstName.EmbeddedValue = "John";
            john.LastName.EmbeddedValue = "Doe";

            var snapshot1 = this.Population.Snapshot();

            jane.FirstName.EmbeddedValue = "Jane";
            jane.LastName.EmbeddedValue = "Doe";

            var changedFirstNames = snapshot1.EmbeddedChangedRoles<Person>("FirstName");
            var changedLastNames = snapshot1.EmbeddedChangedRoles<Person>("LastName");

            Assert.That(changedFirstNames.Keys.Count(), Is.EqualTo(1));
            Assert.That(changedLastNames.Keys.Count(), Is.EqualTo(1));
            Assert.That(changedFirstNames.Keys.ToArray(), Does.Contain(john));
            Assert.That(changedLastNames.Keys.ToArray(), Does.Contain(john));

            var snapshot2 = this.Population.Snapshot();

            changedFirstNames = snapshot2.EmbeddedChangedRoles<Person>("FirstName");
            changedLastNames = snapshot2.EmbeddedChangedRoles<Person>("LastName");

            Assert.That(changedFirstNames.Keys.Count(), Is.EqualTo(1));
            Assert.That(changedLastNames.Keys.Count(), Is.EqualTo(1));
            Assert.That(changedFirstNames.Keys.ToArray(), Does.Contain(jane));
            Assert.That(changedLastNames.Keys.ToArray(), Does.Contain(jane));
        }


        [Test]
        public void Composites()
        {
            var john = this.Population.EmbeddedCreateObject<Person>();
            var jane = this.Population.EmbeddedCreateObject<Person>();

            john.FirstName.EmbeddedValue = "John";
            john.LastName.EmbeddedValue = "Doe";

            jane.FirstName.EmbeddedValue = "Jane";
            jane.LastName.EmbeddedValue = "Doe";

            var acme = this.Population.EmbeddedCreateObject<Organization>();

            acme.Name.EmbeddedValue = "Acme";

            acme.Employees.EmbeddedValue = new[] { john, jane };

            var snapshot = this.Population.Snapshot();
            var changedEmployees = snapshot.EmbeddedChangedRoles<Organization>("Employees");
            Assert.That(changedEmployees.Count, Is.EqualTo(1));

            acme.Employees.EmbeddedValue = new[] { jane, john };

            snapshot = this.Population.Snapshot();
            changedEmployees = snapshot.EmbeddedChangedRoles<Organization>("Employees");
            Assert.That(changedEmployees, Is.Empty);

            acme.Employees.EmbeddedValue = Array.Empty<Person>();

            var x = acme.Employees;

            acme.Employees.EmbeddedValue = new[] { jane, john };

            snapshot = this.Population.Snapshot();
            changedEmployees = snapshot.EmbeddedChangedRoles<Organization>("Employees");
            Assert.That(changedEmployees, Is.Empty);
        }
    }
}
