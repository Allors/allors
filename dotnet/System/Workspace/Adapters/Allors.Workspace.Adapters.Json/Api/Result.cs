// <copyright file="RemoteResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Json
{
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Json.Api;

    public abstract class Result : IResult
    {
        private readonly Response response;

        private IDerivationError[] derivationErrors;

        protected Result(IWorkspace session, Response response)
        {
            this.Session = session;
            this.response = response;
        }

        public IWorkspace Session { get; }

        public bool HasErrors => this.response.HasErrors;

        public string ErrorMessage => this.response._e;

        public IEnumerable<IObject> VersionErrors => this.Session.Instantiate<IObject>(this.response._v);

        public IEnumerable<IObject> AccessErrors => this.Session.Instantiate<IObject>(this.response._a);

        public IEnumerable<IObject> MissingErrors => this.Session.Instantiate<IObject>(this.response._m);

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
                        .Select(v => (IDerivationError)new DerivationError(this.Session, v)).ToArray();
                }

                return this.derivationErrors;
            }
        }
    }
}
