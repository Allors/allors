// <copyright file="TemplateTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Allors.Database.Domain.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using Domain;
    using Xunit;

    public class TemplateTests : DomainTest, IClassFixture<Fixture>
    {
        public TemplateTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void Render()
        {
            var media = this.Transaction.Build<Media>(v => v.InData = this.GetResourceBytes("Domain.Tests.Resources.EmbeddedTemplate.odt"));
            var templateType = new TemplateTypes(this.Transaction).OpenDocumentType;
            var template = this.Transaction.Build<Template>(v =>
            {
                v.Media = media;
                v.TemplateType = templateType;
                v.Arguments = "logo, people";
            });

            this.Transaction.Derive();

            var people = new People(this.Transaction).Extent();
            var logo = this.GetResourceBytes("Domain.Tests.Resources.logo.png");

            var data = new Dictionary<string, object>
            {
                               { "people", people },
                           };

            var images = new Dictionary<string, byte[]>
            {
                                { "logo", logo },
                            };

            var result = template.Render(data, images);

            File.WriteAllBytes("Embedded.odt", result);

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            result = template.Render(data, images);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
