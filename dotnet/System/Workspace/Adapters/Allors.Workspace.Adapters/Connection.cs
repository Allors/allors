// <copyright file="Workspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Meta;

    public abstract class Connection : IConnection
    {
        protected Connection(string name, MetaPopulation metaPopulation, IObjectFactory objectFactory)
        {
            this.Name = name;
            this.MetaPopulation = metaPopulation;
            this.ObjectFactory = objectFactory;
        }

        public string Name { get; }

        public MetaPopulation MetaPopulation { get; }

        public IObjectFactory ObjectFactory { get; }

        public abstract IWorkspace CreateWorkspace();

        public abstract DatabaseRecord GetRecord(long id);

        public abstract long GetPermission(Class @class, IOperandType operandType, Operations operation);
    }
}
