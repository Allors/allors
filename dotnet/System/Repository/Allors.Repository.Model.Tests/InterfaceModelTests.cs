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
    using Allors.Repository.Domain;
    using Allors.Repository.Model;
    using Xunit;

    public class InterfaceModelTests
    {
        [Fact]
        public void InterfaceModels()
        {
            var directoryInfo = new DirectoryInfo(".");

            var repository = new Repository();

            var repositoryModel = new RepositoryModel(repository);

            var interfaceModels = repositoryModel.Domains;
        }
    }
}
