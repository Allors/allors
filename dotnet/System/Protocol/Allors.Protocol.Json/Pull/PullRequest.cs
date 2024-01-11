// <copyright file="PullRequest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Protocol.Json.Api.Pull
{
    using Allors.Protocol.Json.Data;

    public class PullRequest : Request
    {
        /// <summary>
        ///     List of Pulls
        /// </summary>
        public Pull[] l { get; set; }
    }
}
