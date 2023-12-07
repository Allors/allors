// <copyright file="DomainTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Database.Domain.Tests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Allors.Database.Adapters.Memory;
    using Allors.Database.Configuration;
    using Allors.Database.Derivations;
    using Allors.Database.Security;
    using Allors.Database.Meta;
    using Moq;
    using Allors.Population;

    public class DomainTest : IDisposable
    {
        public DomainTest(Fixture fixture, bool populate = true)
        {
            var database = new Database(
                new TestDatabaseServices(fixture.Engine),
                new Configuration
                {
                    ObjectFactory = new ObjectFactory(fixture.MetaPopulation, typeof(User)),
                });

            this.M = ((IDatabase)database).Services.Get<M>();

            this.Setup(database, populate);
        }

        public M M { get; set; }

        public virtual Config Config { get; } = new Config { SetupSecurity = false };

        public ITransaction Transaction { get; private set; }

        public ITime Time => this.Transaction.Database.Services.Get<ITime>();

        public IDerivationService DerivationService => this.Transaction.Database.Services.Get<IDerivationService>();

        public ISecurity Security => this.Transaction.Database.Services.Get<ISecurity>();

        public TimeSpan? TimeShift
        {
            get => this.Time.Shift;

            set => this.Time.Shift = value;
        }

        public Mock<IAccessControl> AclsMock
        {
            get
            {
                var aclMock = new Mock<IAccessControlList>();
                aclMock.Setup(acl => acl.CanRead(It.IsAny<IRoleType>())).Returns(true);
                var aclsMock = new Mock<IAccessControl>();
                aclsMock.Setup(acls => acls[It.IsAny<IObject>()]).Returns(aclMock.Object);
                return aclsMock;
            }
        }

        public void Dispose()
        {
            this.Transaction.Rollback();
            this.Transaction = null;
        }

        protected void Setup(IDatabase database, bool populate)
        {
            database.Init();

            var fixture = new FixtureResource(this.M).Read();
            new Setup(database, fixture, this.Config).Apply();

            this.Transaction = database.CreateTransaction();

            if (populate)
            {
                new TestPopulation(this.Transaction).Apply();
                this.Transaction.Commit();
            }
        }

        protected Stream GetResource(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream(name);
        }

        protected byte[] GetResourceBytes(string name)
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            var resource = assembly.GetManifestResourceStream(name);
            using var ms = new MemoryStream();
            resource?.CopyTo(ms);
            return ms.ToArray();
        }

        protected Permission FindPermission(IRoleType roleType, Operations operation)
        {
            var objectType = (Class)roleType.AssociationType.ObjectType;
            return this.Transaction.Scoped<PermissionByMeta>().Get(objectType, roleType, operation);
        }

        #region Builders
        protected UserGroup BuildUserGroup(string name, params User[] members) => this.Transaction.Build<UserGroup>(v =>
        {
            v.Name = name;
            v.Members = members;
        });

        protected Role BuildRole(string name, params Permission[] permissions) => this.Transaction.Build<Role>(v =>
        {
            v.Name = name;
            v.Permissions = permissions;
        });

        protected Grant BuildGrant(User subject, Role role = null) => this.Transaction.Build<Grant>(v =>
        {
            v.AddSubject(subject);
            v.Role = role;
        });

        protected Grant BuildGrant(UserGroup subjectGroup, Role role = null) => this.Transaction.Build<Grant>(v =>
        {
            v.AddSubjectGroup(subjectGroup);
            v.Role = role;
        });

        protected Revocation BuildRevocation(params Permission[] deniedPermissions) => this.Transaction.Build<Revocation>(v =>
        {
            v.DeniedPermissions = deniedPermissions;
        });

        protected SecurityToken BuildSecurityToken() => this.Transaction.Build<SecurityToken>();

        protected Person BuildPerson(string firstName, string lastName) => this.Transaction.Build<Person>(v =>
        {
            v.FirstName = firstName;
            v.LastName = lastName;
        });

        protected Person BuildPerson(string userName) => this.Transaction.Build<Person>(v =>
        {
            v.UserName = userName;
        });

        protected Organization BuildOrganization(string name) => this.Transaction.Build<Organization>(v =>
        {
            v.Name = name;
        });

        protected C1 BuildC1(params Action<C1>[] builders) => this.Transaction.Build(builders);

        protected C1 BuildC1(string c1AllorsString = null, Action<C1> builder = null) => this.Transaction.Build((v =>
        {
            v.C1AllorsString = c1AllorsString;
        }), builder);

        protected C2 BuildC2(params Action<C2>[] builders) => this.Transaction.Build(builders);

        protected C2 BuildC2(string c2AllorsString = null, Action<C2> builder = null) => this.Transaction.Build(v =>
        {
            v.C2AllorsString = c2AllorsString;
        }, builder);

        #endregion
    }
}
