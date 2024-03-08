// <copyright file="Restore.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Allors.Database.Domain;
    using Allors.Database.Meta;
    using Allors.Database.Population;
    using Allors.Database.Population.Resx;
    using McMaster.Extensions.CommandLineUtils;
    using NLog;

    [Command(Description = "Print missing translations")]
    public class Missing
    {
        public Program Parent { get; set; }


        [Option("-f", Description = "File for missing translations")]
        public string FileName { get; set; }

        public Logger Logger => LogManager.GetCurrentClassLogger();

        public int OnExecute(CommandLineApplication app)
        {
            var database = this.Parent.Database;
            var m = database.Services.Get<IMetaIndex>();

            this.Logger.Info("Begin");

            var translationConfiguration = new TranslationConfiguration();

            var cultureInfos = translationConfiguration.AdditionalCultureInfos.Append(translationConfiguration.DefaultCultureInfo);
            var localeNames = new HashSet<string>(cultureInfos.Select(v => v.Name));

            using var transaction = database.CreateTransaction();

            var locales = transaction.Filter<Locale>().Where(v => localeNames.Contains(v.Key)).ToArray();

            var enumerationsByClass = transaction.Filter<Enumeration>()
                .GroupBy(v => v.Strategy.Class)
                .ToDictionary(v => v.Key, v => v);

            foreach (var (@class, enumerations) in enumerationsByClass)
            {
                foreach (var enumeration in enumerations)
                {
                    foreach (var locale in locales)
                    {
                        var localisedName = enumeration.LocalisedNames.FirstOrDefault(v => v.Locale == locale);
                        if (localisedName == null)
                        {
                            Console.WriteLine($"{@class.SingularName}.{locale.Key}[{enumeration.Key}]");
                        }
                    }
                }
            }

            this.Logger.Info("End");
            return ExitCode.Success;
        }
    }
}
