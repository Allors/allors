// <copyright file="Pull.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Request
{
    using System.Collections.Generic;
    using Response;
    using Visitor;

    public class ProcedureCall : IInvocable
    {
        public ProcedureCall(string procedureName) => this.ProcedureName = procedureName;

        public string ProcedureName { get; }

        public IDictionary<string, IObject[]> Collections { get; set; }

        public IDictionary<string, IObject> Objects { get; set; }

        public IDictionary<string, string> Values { get; set; }

        public IDictionary<IObject, long> Pool { get; set; }

        public void Accept(IVisitor visitor) => visitor.VisitProcedureCall(this);
    }
}
