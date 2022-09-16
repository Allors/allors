// <copyright file="Workspace.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using System.Collections.Generic;
    using Meta;
    using Response;

    public abstract class Workspace : IWorkspace
    {
        private readonly Dictionary<Class, ISet<Object>> objectsByClass;

        protected Workspace(Connection connection)
        {
            this.Connection = connection;
            this.ObjectByWorkspaceId = new Dictionary<long, Object>();
            this.objectsByClass = new Dictionary<Class, ISet<Object>>();
        }

        public Connection Connection { get; }

        protected Dictionary<long, Object> ObjectByWorkspaceId { get; }

        IConnection IWorkspace.Connection => this.Connection;

        public IEnumerable<IObject> Objects => this.ObjectByWorkspaceId.Values;

        public override string ToString() => $"workspace: {base.ToString()}";

        public Object GetObject(long id)
        {
            if (id == 0)
            {
                return null;
            }

            return this.ObjectByWorkspaceId.TryGetValue(id, out var @object) ? @object : null;
        }

        protected void AddObject(Object @object)
        {
            this.ObjectByWorkspaceId.Add(@object.Id, @object);

            var @class = @object.Class;
            if (!this.objectsByClass.TryGetValue(@class, out var strategies))
            {
                this.objectsByClass[@class] = new HashSet<Object> {@object};
            }
            else
            {
                strategies.Add(@object);
            }
        }
    }
}
