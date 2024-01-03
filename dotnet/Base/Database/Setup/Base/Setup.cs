// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
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

        private void BaseOnPrePrepare()
        {
        }

        private void BaseOnPostPrepare()
        {
            foreach (var @class in this.Config.Translation.ResourceSetByCultureInfoByRoleTypeByClass.Keys)
            {
                this.AddDependency(@class, this.M.Locale);
            }
        }

        private void BaseOnPreSetup()
        {
        }

        private void BaseOnPostSetup()
        {
        }


        private void BaseOnCreated(IObject @object)
        {
            IClass @class = @object.Strategy.Class;
            if (@class.KeyRoleType != null)
            {
                if (this.Config.Translation.ResourceSetByCultureInfoByRoleTypeByClass.TryGetValue(@class, out var resourceSetByCultureInfoByRoleType))
                {
                    var key = (string)@object.Strategy.GetUnitRole(@class.KeyRoleType);

                    foreach (var (roleType, resourceSetByCultureInfo) in resourceSetByCultureInfoByRoleType)
                    {
                        foreach (var (cultureInfo, resourceSet) in resourceSetByCultureInfo)
                        {
                            var value = resourceSet?.GetString(key);

                            if (value == null)
                            {
                                continue;
                            }

                            var localeByKey = new LocaleByKey(this.transaction);
                            var locale = localeByKey[cultureInfo.TwoLetterISOLanguageName];
                            var localisedText = this.transaction.Build<LocalisedText>(v =>
                            {
                                v.Locale = locale;
                                v.Text = value;
                            });

                            @object.Strategy.AddCompositesRole(roleType, localisedText);
                        }
                    }
                }
            }
        }
    }
}
