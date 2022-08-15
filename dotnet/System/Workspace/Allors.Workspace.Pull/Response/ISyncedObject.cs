namespace Allors.Workspace.Sync.Response
{
    using System.Collections.Generic;

    public interface ISyncedObject
    {
        string Tag { get; }

        long Version { get; }

        /// <summary>
        /// A comma separated, colon separated id and version of
        /// all grants and revocations, ordered by id.
        ///
        /// e.g. grant with id 10 and version 5,
        /// another grant with id 12 and version 4
        /// and finally a revocation with id 11 and version 2
        /// will result in the string value "10:5,11:2,12:4"
        /// </summary>
        string SecurityFingerprint { get; }

        IDictionary<long, ISyncedGrant> GrantById { get; }

        IDictionary<long, ISyncedRevocation> RevocationById { get; }

        IDictionary<string, ISyncedRole> RoleByTag { get; }
    }
}
