// <copyright file="FilterTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Data.Tests
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Meta;
    using Xunit;

    public class ExpressionExtensionsTests
    {
        public ExpressionExtensionsTests()
        {
            var metaBuilder = new MetaBuilder();
            this.M = metaBuilder.Build();
        }

        private MetaPopulation M { get; }

        [Fact]
        public void InterfaceAssociation()
        {
            Expression<Func<MetaUser, IPropertyType>> expression = v => v.Logins;

            var path = expression.Node(this.M);

            Assert.Equal(this.M.User.Logins, path.PropertyType);
            Assert.Empty(path.Nodes);
        }

        [Fact]
        public void ClassAssociation()
        {
            Expression<Func<MetaPerson, IPropertyType>> expression = v => v.OrganizationWhereEmployee;

            var path = expression.Node(this.M);

            Assert.Equal(this.M.Person.OrganizationWhereEmployee, path.PropertyType);
            Assert.Empty(path.Nodes);
        }

        [Fact]
        public void ClassAssociationClassRole()
        {
            Expression<Func<MetaPerson, IPropertyType>> expression = v => v.OrganizationWhereEmployee.Organization.Information;

            var path = expression.Node(this.M);

            Assert.Equal(this.M.Person.OrganizationWhereEmployee, path.PropertyType);

            var next = path.Nodes.First();

            Assert.Equal(this.M.Organization.Information, next.PropertyType);
            Assert.Empty(next.Nodes);
        }

        [Fact]
        public void ClassRole()
        {
            Expression<Func<MetaOrganization, IPropertyType>> expression = v => v.Name;

            var path = expression.Node(this.M);

            Assert.Equal(this.M.Organization.Name, path.PropertyType);
            Assert.Empty(path.Nodes);
        }

        [Fact]
        public void ClassRoleOfType()
        {
            Expression<Func<MetaUserGroup, IComposite>> expression = v => v.Members.User.AsPerson;

            var path = expression.Node(this.M);

            Assert.Equal(this.M.UserGroup.Members, path.PropertyType);
            Assert.Equal(this.M.Person, path.OfType);
            Assert.Empty(path.Nodes);
        }

        [Fact]
        public void ClassRoleClassRole()
        {
            Expression<Func<MetaOrganization, IPropertyType>> expression = v => v.Employees.Person.FirstName;

            var path = expression.Node(this.M);

            Assert.Equal(this.M.Organization.Employees, path.PropertyType);

            var next = path.Nodes.First();

            Assert.Equal(this.M.Person.FirstName, next.PropertyType);
            Assert.Empty(next.Nodes);
        }


        [Fact]
        public void ClassRoleInterfaceAsClassRole()
        {
            Expression<Func<MetaUserGroup, IPropertyType>> expression = v => v.Members.User.AsPerson.FirstName;

            var path = expression.Node(this.M);

            Assert.Equal(this.M.UserGroup.Members, path.PropertyType);

            var next = path.Nodes.First();

            Assert.Equal(this.M.Person.FirstName, next.PropertyType);
            Assert.Empty(next.Nodes);
        }
    }
}
