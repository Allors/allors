// <copyright file="Program.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta.Generation
{
    using System;
    using System.IO;
    using Allors.Database.Meta.Configuration;
    using Allors.Meta.Generation.Model;
    using Database.Population;
    using Resources;

    internal class Program
    {
        private static readonly MetaBuilder MetaBuilder = new MetaBuilder();

        private static int Main()
        {
            string[,] database =
                {
                    { "Database/Templates/domain.cs.stg", "Database/Domain/Generated" },
                    { "Database/Templates/setup.cs.stg", "Database/Setup/Generated" },
                    { "Database/Templates/uml.cs.stg", "Database/Domain.Diagrams/Generated" },
                };

            string[,] workspace =
            {
                { "Workspace/Templates/uml.cs.stg", "Workspace/Diagrams/Generated" },
                { "Workspace/Templates/meta.cs.stg", "Workspace/Meta.Domain/Generated" },
                { "Workspace/Templates/meta.static.cs.stg", "Workspace/Meta.Configuration/Generated" },
                { "Workspace/Templates/domain.cs.stg", "Workspace/Domain/Generated" },

                { "../../typescript/templates/workspace.meta.ts.stg", "../../typescript/libs/core/workspace/meta/src/lib/generated" },
                { "../../typescript/templates/workspace.meta.json.ts.stg", "../../typescript/libs/core/workspace/meta-json/src/lib/generated" },
                { "../../typescript/templates/workspace.domain.ts.stg", "../../typescript/libs/core/workspace/domain/src/lib/generated" },
            };

            var metaPopulation = MetaBuilder.Build();
            var fixtureFile = new FixtureFile(metaPopulation);
            var fixture = fixtureFile.Read();
            var model = new Model.Model(metaPopulation, fixture);
            model.Init();

            for (var i = 0; i < database.GetLength(0); i++)
            {
                var template = database[i, 0];
                var output = database[i, 1];

                Console.WriteLine("-> " + output);

                RemoveDirectory(output);

                var log = Generate.Execute(model, template, output);
                if (log.ErrorOccured)
                {
                    return 1;
                }
            }

            for (var i = 0; i < workspace.GetLength(0); i++)
            {
                var template = workspace[i, 0];
                var output = workspace[i, 1];

                Console.WriteLine("-> " + output);

                RemoveDirectory(output);

                var log = Generate.Execute(model, template, output, "Default");
                if (log.ErrorOccured)
                {
                    return 1;
                }
            }

            return 0;
        }

        private static void RemoveDirectory(string output)
        {
            var directoryInfo = new DirectoryInfo(output);
            if (directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Delete(true);
                }
                catch
                {
                }
            }
        }
    }
}
