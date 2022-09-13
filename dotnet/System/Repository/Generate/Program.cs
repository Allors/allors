// <copyright file="Program.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Tools.Cmd
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using NLog;
    using Repository;
    using Repository.Code;
    using Repository.Domain;
    using Repository.Generation;

    public class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static async Task<int> Main(string[] args)
        {
            try
            {
                if (args.Length < 3)
                {
                    Logger.Error("missing required arguments");
                }

                await RepositoryGenerate(args);
            }
            catch (RepositoryException e)
            {
                Logger.Error(e.Message);
                return 1;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                Logger.Info("Finished with errors");
                return 1;
            }

            Logger.Info("Finished");
            return 0;
        }

        private static async Task RepositoryGenerate(string[] args)
        {
            var projectPath = args[0];
            var template = args[1];
            var output = args[2];

            var fileInfo = new FileInfo(projectPath);

            Logger.Info("Generate " + fileInfo.FullName);
            var project = new Project(fileInfo.FullName);
            await project.InitializeAsync();

            if (project.HasErrors)
            {
                throw new RepositoryException("Repository project has errors.");
            }

            var templateFileInfo = new FileInfo(template);
            var stringTemplate = new StringTemplate(templateFileInfo);
            var outputDirectoryInfo = new DirectoryInfo(output);

            var repository = project.Repository;
            stringTemplate.Generate(repository, outputDirectoryInfo);
        }
    }
}
