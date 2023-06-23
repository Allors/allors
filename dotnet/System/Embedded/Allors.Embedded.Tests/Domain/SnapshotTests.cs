using System;

namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Meta;
    using Allors.Embedded.Tests.Domain;

    public class SnapshotTests : Tests
    {
        [Test]
        public void Unit()
        {
            var john = this.Population.New<Person>();
            var jane = this.Population.New<Person>();

            john.FirstName = "John";
            john.LastName = "Doe";

            var snapshot1 = this.Population.Snapshot();

            jane.FirstName = "Jane";
            jane.LastName = "Doe";

            var changedFirstNames = snapshot1.ChangedRoles<Person>("FirstName");
            var changedLastNames = snapshot1.ChangedRoles<Person>("LastName");

            Assert.AreEqual(1, changedFirstNames.Keys.Count());
            Assert.AreEqual(1, changedLastNames.Keys.Count());
            Assert.Contains(john, changedFirstNames.Keys.ToArray());
            Assert.Contains(john, changedLastNames.Keys.ToArray());

            var snapshot2 = this.Population.Snapshot();

            changedFirstNames = snapshot2.ChangedRoles<Person>("FirstName");
            changedLastNames = snapshot2.ChangedRoles<Person>("LastName");

            Assert.AreEqual(1, changedFirstNames.Keys.Count());
            Assert.AreEqual(1, changedLastNames.Keys.Count());
            Assert.Contains(jane, changedFirstNames.Keys.ToArray());
            Assert.Contains(jane, changedLastNames.Keys.ToArray());
        }


        [Test]
        public void Composites()
        {
            var john = this.Population.New<Person>();
            var jane = this.Population.New<Person>();

            john.FirstName = "John";
            john.LastName = "Doe";

            jane.FirstName = "Jane";
            jane.LastName = "Doe";

            var acme = this.Population.New<Organization>();

            acme.Name.Value = "Acme";

            acme.Employees = new[] { john, jane };

            var snapshot = this.Population.Snapshot();
            var changedEmployees = snapshot.ChangedRoles<Organization>("Employees");
            Assert.AreEqual(1, changedEmployees.Count);

            acme.Employees = new[] { jane, john };

            snapshot = this.Population.Snapshot();
            changedEmployees = snapshot.ChangedRoles<Organization>("Employees");
            Assert.IsEmpty(changedEmployees);

            acme.Employees = Array.Empty<Person>();

            var x = acme.Employees;

            acme.Employees = new[] { jane, john };

            snapshot = this.Population.Snapshot();
            changedEmployees = snapshot.ChangedRoles<Organization>("Employees");
            Assert.IsEmpty(changedEmployees);
        }
    }
}
