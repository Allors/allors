namespace Allors.Embedded.Tests
{
    using System;
    using Allors.Embedded.Meta;
    using Allors.Embedded.Tests.Domain;

    public class OneToOneTests : Tests
    {
        [Test]
        public void StaticPropertySet()
        {
            var acme = this.Population.New<Organization>();
            var gizmo = this.Population.New<Organization>();

            var jane = this.Population.New<Person>();
            var john = this.Population.New<Person>();

            acme.Owner.Value = jane;

            Assert.AreEqual(jane, acme.Owner.Value);
            Assert.AreEqual(acme, jane.OrganizationWhereOwner);

            Assert.Null(gizmo.Owner.Value);
            Assert.Null(john.OrganizationWhereOwner);

            acme.Named.Value = jane;

            Assert.AreEqual(jane, acme.Named.Value);
            Assert.AreEqual(acme, jane.OrganizationWhereNamed);

            Assert.Null(gizmo.Named.Value);
            Assert.Null(john.OrganizationWhereNamed);
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
