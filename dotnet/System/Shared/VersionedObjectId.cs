// <copyright file="Multiplicity.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;

    public class VersionedObjectId : IEquatable<VersionedObjectId>
    {
        public VersionedObjectId(long objectId, ObjectVersion objectVersion)
        {
            this.ObjectId = objectId;
            this.ObjectVersion = objectVersion;
        }

        public long ObjectId { get; }

        public ObjectVersion ObjectVersion { get; }

        public bool Equals(VersionedObjectId other) => this.ObjectId == other?.ObjectId && this.ObjectVersion == other.ObjectVersion;

        public override bool Equals(object other) => other is VersionedObjectId otherVersionedId && this.Equals(otherVersionedId);

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.ObjectId.GetHashCode() * 397) ^ this.ObjectVersion.GetHashCode();
            }
        }
    }
}
