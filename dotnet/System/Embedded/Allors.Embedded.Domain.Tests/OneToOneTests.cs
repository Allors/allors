namespace Allors.Embedded.Domain.Tests
{
    public abstract class OneToOneTests : Tests
    {
        [Test]
        public void StaticPropertySet()
        {
            var acme = this.Population.EmbeddedCreateObject<Organization>();
            var gizmo = this.Population.EmbeddedCreateObject<Organization>();

            var jane = this.Population.EmbeddedCreateObject<Person>();
            var john = this.Population.EmbeddedCreateObject<Person>();

            acme.Owner.EmbeddedValue = jane;

            Assert.That(acme.Owner.EmbeddedValue, Is.EqualTo(jane));
            Assert.That(jane.OrganizationWhereOwner.EmbeddedValue, Is.EqualTo(acme));

            Assert.That(gizmo.Owner.EmbeddedValue, Is.Null);
            Assert.That(john.OrganizationWhereOwner.EmbeddedValue, Is.Null);

            acme.Named.EmbeddedValue = jane;

            Assert.That(acme.Named.EmbeddedValue, Is.EqualTo(jane));
            Assert.That(jane.OrganizationWhereNamed.EmbeddedValue, Is.EqualTo(acme));

            Assert.That(gizmo.Named.EmbeddedValue, Is.Null);
            Assert.That(john.OrganizationWhereNamed.EmbeddedValue, Is.Null);
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
