// <copyright file="Program.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Meta.Generation.Storage;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Allors.Database.Meta.Configuration;
using Allors.Database.Meta;
using Allors.Meta.Generation.Model;
using Database.Population;

internal sealed class Program
{
    private static readonly MetaBuilder MetaBuilder = new();

    private static int Main()
    {
        var metaPopulation = MetaBuilder.Build();
        var model = new Model(metaPopulation, new ReadOnlyDictionary<Class, Record[]>(new Dictionary<Class, Record[]>()));

        string[,] config = { { "Templates/adapters.cs.stg", "Domain/Generated" } };

        for (var i = 0; i < config.GetLength(0); i++)
        {
            var template = config[i, 0];
            var output = config[i, 1];

            Console.WriteLine("-> " + output);

            RemoveDirectory(output);

            var log = Generate.Execute(model, template, output);
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
