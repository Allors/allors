// <copyright file="Multiplicity.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;

    public readonly struct ObjectVersion : IEquatable<ObjectVersion>
    {
        public static readonly ObjectVersion Unknown = new ObjectVersion(0);

        public static readonly ObjectVersion WorkspaceInitial = new ObjectVersion(1);

        public static readonly ObjectVersion DatabaseInitial = new ObjectVersion(2);

        private ObjectVersion(long value) => this.Value = value;

        public long Value { get; }

        public static implicit operator long(ObjectVersion objectVersion) => objectVersion.Value;

        public static implicit operator ObjectVersion(long value) => new ObjectVersion(value);

        public static bool operator ==(ObjectVersion @this, ObjectVersion other) => @this.Equals(other);

        public static bool operator !=(ObjectVersion @this, ObjectVersion other) => !@this.Equals(other);

        public bool Equals(ObjectVersion other) => this.Value == other.Value;

        public override bool Equals(object obj) => obj is ObjectVersion other && this.Equals(other);

        public override int GetHashCode() => this.Value.GetHashCode();

        public ObjectVersion Next() => new ObjectVersion(this.Value + 1);
    }
}
