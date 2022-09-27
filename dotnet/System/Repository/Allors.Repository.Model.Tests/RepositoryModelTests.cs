// <copyright file="FilterTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using System.IO;
    using System.Linq;
    using Allors.Repository.Domain;
    using Allors.Repository.Model;
    using Xunit;

    public class DomainModelTests
    {
        [Fact]
        public void DomainModel()
        {
            var directoryInfo = new DirectoryInfo(".");

            var repository = new Repository();

            var a = new Domain(repository.Objects, "A", directoryInfo);
            var b = new Domain(repository.Objects, "B", directoryInfo);
            var c = new Domain(repository.Objects, "C", directoryInfo);
            var d = new Domain(repository.Objects, "D", directoryInfo);

            a.DirectSuperdomains = new[] { b, c };
            b.DirectSuperdomains = new[] { c };
            c.DirectSuperdomains = new[] { d };

            var repositoryModel = new RepositoryModel(repository);

            var domainModels = repositoryModel.Domains.ToArray();

            Assert.Equal(4, domainModels.Length);

            Assert.Equal(d, domainModels[0].Domain);
            Assert.Equal(c, domainModels[1].Domain);
            Assert.Equal(b, domainModels[2].Domain);
            Assert.Equal(a, domainModels[3].Domain);
        }
    }
}
