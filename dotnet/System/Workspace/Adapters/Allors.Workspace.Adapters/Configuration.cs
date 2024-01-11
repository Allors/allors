// <copyright file="Configuration.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Meta;

    public abstract class Configuration 
    {
        protected Configuration(string name, IMetaPopulation metaPopulation)
        {
            this.Name = name;
            this.MetaPopulation = metaPopulation;
        }

        public string Name { get; }

        public IMetaPopulation MetaPopulation { get; }
    }
}
