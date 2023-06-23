namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Tests.Domain;

    public class SnapshotTests : Tests
    {
        [Test]
        public void Unit()
        {
            var john = this.Population.New<Person>();
            var jane = this.Population.New<Person>();

            john.FirstName.Value = "John";
            john.LastName.Value = "Doe";

            var snapshot1 = this.Population.Snapshot();

            jane.FirstName.Value = "Jane";
            jane.LastName.Value = "Doe";

            var changedFirstNames = snapshot1.ChangedRoles<Person>("FirstName");
            var changedLastNames = snapshot1.ChangedRoles<Person>("LastName");

            Assert.That(changedFirstNames.Keys.Count(), Is.EqualTo(1));
            Assert.That(changedLastNames.Keys.Count(), Is.EqualTo(1));
            Assert.Contains(john, changedFirstNames.Keys.ToArray());
            Assert.Contains(john, changedLastNames.Keys.ToArray());

            var snapshot2 = this.Population.Snapshot();

            changedFirstNames = snapshot2.ChangedRoles<Person>("FirstName");
            changedLastNames = snapshot2.ChangedRoles<Person>("LastName");

            Assert.That(changedFirstNames.Keys.Count(), Is.EqualTo(1));
            Assert.That(changedLastNames.Keys.Count(), Is.EqualTo(1));
            Assert.Contains(jane, changedFirstNames.Keys.ToArray());
            Assert.Contains(jane, changedLastNames.Keys.ToArray());
        }


        [Test]
        public void Composites()
        {
            var john = this.Population.New<Person>();
            var jane = this.Population.New<Person>();

            john.FirstName.Value = "John";
            john.LastName.Value = "Doe";

            jane.FirstName.Value = "Jane";
            jane.LastName.Value = "Doe";

            var acme = this.Population.New<Organization>();

            acme.Name.Value = "Acme";

            acme.Employees.Value = new[] { john, jane };

            var snapshot = this.Population.Snapshot();
            var changedEmployees = snapshot.ChangedRoles<Organization>("Employees");
            Assert.That(changedEmployees.Count, Is.EqualTo(1));

            acme.Employees.Value = new[] { jane, john };

            snapshot = this.Population.Snapshot();
            changedEmployees = snapshot.ChangedRoles<Organization>("Employees");
            Assert.IsEmpty(changedEmployees);

            acme.Employees.Value = Array.Empty<Person>();

            var x = acme.Employees;

            acme.Employees.Value = new[] { jane, john };

            snapshot = this.Population.Snapshot();
            changedEmployees = snapshot.ChangedRoles<Organization>("Employees");
            Assert.IsEmpty(changedEmployees);
        }
    }
}
