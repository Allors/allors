﻿// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Graph;
    using Allors.Database.Meta;
    using Population;

    public partial class Setup
    {
        private readonly ITransaction transaction;

        private readonly Dictionary<IObjectType, IObjects> objectsByObjectType;
        private readonly Graph<IObjects> objectsGraph;

        public Setup(IDatabase database, Config config)
        {
            this.Config = config;

            this.M = database.Services.Get<M>();

            this.transaction = database.CreateTransaction();

            this.objectsByObjectType = new Dictionary<IObjectType, IObjects>();
            foreach (var objectType in this.transaction.Database.MetaPopulation.Composites)
            {
                this.objectsByObjectType[objectType] = objectType.GetObjects(transaction);
            }

            this.objectsGraph = new Graph<IObjects>();
        }

        public Config Config { get; }

        public M M { get; set; }

        public void Apply()
        {
            this.OnPrePrepare();

            foreach (var objects in this.objectsByObjectType.Values)
            {
                objects.Prepare(this);
            }

            this.OnPostPrepare();

            this.OnPreSetup();

            this.objectsGraph.Invoke(objects => objects.Setup(this));

            this.OnPostSetup();

            this.transaction.Derive();

            if (this.Config.SetupSecurity)
            {
                new Security(this.transaction).Apply();
            }

            this.transaction.Derive();
            this.transaction.Commit();
        }

        public void Add(IObjects objects) => this.objectsGraph.Add(objects);

        /// <summary>
        /// The dependee is set up before the dependent object;.
        /// </summary>
        /// <param name="dependent"></param>
        /// <param name="dependee"></param>
        public void AddDependency(IComposite dependent, IComposite dependee)
        {
            foreach (var dependentClass in dependent.Classes)
            {
                foreach (var dependeeClass in dependee.Classes)
                {
                    this.objectsGraph.AddDependency(this.objectsByObjectType[dependentClass], this.objectsByObjectType[dependeeClass]);
                }
            }
        }

        private void CoreOnPrePrepare()
        {
        }

        private void CoreOnPostPrepare()
        {
        }

        private void CoreOnPreSetup()
        {
        }

        private void CoreOnPostSetup()
        {
        }

        private void CoreOnCreated(IObject @object)
        {
        }
    }
}
