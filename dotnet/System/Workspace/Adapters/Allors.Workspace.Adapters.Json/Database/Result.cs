// <copyright file="RemoteResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api;

    public abstract class Result : IResult
    {
        private readonly Response response;

        private IDerivationError[] derivationErrors;

        private IList<IConflict> mergeErrors;

        protected Result(IWorkspace workspace, Response response)
        {
            this.Workspace = workspace;
            this.response = response;
        }

        public IWorkspace Workspace { get; }

        public bool HasErrors => this.response.HasErrors || this.mergeErrors?.Count > 0;

        public string ErrorMessage => this.response._e;

        public IEnumerable<IObject> VersionErrors => this.response._v != null ? this.Workspace.Instantiate<IObject>(this.response._v) : Array.Empty<IObject>();

        public IEnumerable<IObject> AccessErrors => this.response._a != null ? this.Workspace.Instantiate<IObject>(this.response._a) : Array.Empty<IObject>();

        public IEnumerable<IObject> MissingErrors => this.response._m != null ? this.Workspace.Instantiate<IObject>(this.response._m) : Array.Empty<IObject>();

        public IEnumerable<IDerivationError> DerivationErrors
        {
            get
            {
                if (this.derivationErrors != null)
                {
                    return this.derivationErrors;
                }

                if (this.response._d?.Length > 0)
                {
                    return this.derivationErrors ??= this.response._d
                        .Select(v => (IDerivationError)new DerivationError(this.Workspace, v)).ToArray();
                }

                return this.derivationErrors;
            }
        }

        public IEnumerable<IConflict> MergeErrors => this.mergeErrors ?? Array.Empty<IConflict>();

        public void AddMergeError(IConflict conflict)
        {
            this.mergeErrors ??= new List<IConflict>();
            this.mergeErrors.Add(conflict);
        }
    }
}
