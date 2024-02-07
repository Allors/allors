namespace Allors.Embedded.Tests
{
    using Allors.Embedded.Tests.Domain;

    public abstract class OneToOneTests : Tests
    {
        [Test]
        public void StaticPropertySet()
        {
            var acme = this.Population.New<Organization>();
            var gizmo = this.Population.New<Organization>();

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();

            acme.Owner.Value = jane;

            Assert.That(acme.Owner.Value, Is.EqualTo(jane));
            Assert.That(jane.OrganizationWhereOwner.Value, Is.EqualTo(acme));

            Assert.That(gizmo.Owner.Value, Is.Null);
            Assert.That(john.OrganizationWhereOwner.Value, Is.Null);

            acme.Named.Value = jane;

            Assert.That(acme.Named.Value, Is.EqualTo(jane));
            Assert.That(jane.OrganizationWhereNamed.Value, Is.EqualTo(acme));

            Assert.That(gizmo.Named.Value, Is.Null);
            Assert.That(john.OrganizationWhereNamed.Value, Is.Null);
        }

        //[Test]
        //public void EmbeddedPropertySet()
        //{
        //    var acme = this.Population.New<Organization>();
        //    var gizmo = this.Population.New<Organization>();

        //    var jane = this.Population.New<Person>();
        //    var john = this.Population.New<Person>();

        //    acme.Owner = jane;

        //    Assert.AreEqual(jane, acme.Owner);
        //    Assert.AreEqual(acme, jane.OrganizationWhereOwner);
        //    Assert.AreEqual(jane, acme["Owner"]);
        //    Assert.AreEqual(acme, jane["OrganizationWhereOwner"]);
        //    Assert.AreEqual(jane, acme[owner]);
        //    Assert.AreEqual(acme, jane[property]);

        //    Assert.Null(gizmo.Owner);
        //    Assert.Null(john.OrganizationWhereOwner);
        //    Assert.Null(gizmo["Owner"]);
        //    Assert.Null(john["OrganizationWhereOwner"]);
        //    Assert.Null(gizmo[owner]);
        //    Assert.Null(john[property]);

        //    // Wrong Type
        //    Assert.Throws<ArgumentException>(() =>
        //    {
        //        acme.Owner = gizmo;
        //    });
        //}

        //[Test]
        //public void IndexByNameSet()
        //{
        //    dynamic acme = this.Population.New<Organization>();
        //    dynamic gizmo = this.Population.New<Organization>();
        //    dynamic jane = this.Population.New<Person>();
        //    dynamic john = this.Population.New<Person>();

        //    acme["Owner"] = jane;

        //    Assert.AreEqual(jane, acme.Owner);
        //    Assert.AreEqual(acme, jane.OrganizationWhereOwner);
        //    Assert.AreEqual(jane, acme["Owner"]);
        //    Assert.AreEqual(acme, jane["OrganizationWhereOwner"]);
        //    Assert.AreEqual(jane, acme[owner]);
        //    Assert.AreEqual(acme, jane[property]);

        //    Assert.Null(gizmo.Owner);
        //    Assert.Null(john.OrganizationWhereOwner);
        //    Assert.Null(gizmo["Owner"]);
        //    Assert.Null(john["OrganizationWhereOwner"]);
        //    Assert.Null(gizmo[owner]);
        //    Assert.Null(john[property]);

        //    // Wrong Type
        //    Assert.Throws<ArgumentException>(() =>
        //    {
        //        acme["Owner"] = gizmo;
        //    });
        //}

        //[Test]
        //public void IndexByRoleSet()
        //{
        //    var acme = this.Population.New<Organization>();
        //    var gizmo = this.Population.New<Organization>();
        //    var jane = this.Population.New<Person>();
        //    var john = this.Population.Populationpulation.New<Person>();

        //    acme[owner] = jane;

        //    Assert.AreEqual(jane, acme.Owner);
        //    Assert.AreEqual(acme, jane.OrganizationWhereOwner);
        //    Assert.AreEqual(jane, acme["Owner"]);
        //    Assert.AreEqual(acme, jane["OrganizationWhereOwner"]);
        //    Assert.AreEqual(jane, acme[owner]);
        //    Assert.AreEqual(acme, jane[property]);

        //    Assert.Null(gizmo.Owner);
        //    Assert.Null(john.OrganizationWhereOwner);
        //    Assert.Null(gizmo["Owner"]);
        //    Assert.Null(john["OrganizationWhereOwner"]);
        //    Assert.Null(gizmo[owner]);
        //    Assert.Null(john[property]);

        //    // Wrong Type
        //    Assert.Throws<ArgumentException>(() =>
        //    {
        //        acme[owner] = gizmo;
        //    });
        //}
    }
}
