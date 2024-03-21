import { Class, RoleType } from '@allors/workspace-system-meta';
import {
  DatabaseRecord as SystemDatabaseRecord,
  IRange,
} from '@allors/workspace-system-adapters';
import {
  SyncResponseObject,
  SyncResponseRole,
} from '@allors/database-system-protocol-json';
import { DatabaseConnection } from './database-connection';
import { ResponseContext } from './security/response-context';
import { unitFromJson } from '../json/from-json';

export class DatabaseRecord extends SystemDatabaseRecord {
  grants: IRange<number>;
  revocations: IRange<number>;

  private _roleByRoleType?: Map<RoleType, unknown>;
  private syncResponseRoles?: SyncResponseRole[];

  constructor(
    public readonly database: DatabaseConnection,
    cls: Class,
    id: number,
    public override version: number
  ) {
    super(cls, id, version);
  }

  static fromResponse(
    database: DatabaseConnection,
    ctx: ResponseContext,
    syncResponseObject: SyncResponseObject
  ): DatabaseRecord {
    const object = new DatabaseRecord(
      database,
      database.configuration.metaPopulation.metaObjectByTag.get(
        syncResponseObject.c
      ) as Class,
      syncResponseObject.i,
      syncResponseObject.v
    );
    object.syncResponseRoles = syncResponseObject.ro;
    object.grants = ctx.checkForMissingGrants(syncResponseObject.g);
    object.revocations = ctx.checkForMissingRevocations(syncResponseObject.r);
    return object;
  }

  get roleByRoleType(): Map<RoleType, unknown> {
    if (this.syncResponseRoles != null) {
      const metaPopulation = this.database.configuration.metaPopulation;
      this._roleByRoleType = new Map(
        this.syncResponseRoles.map((v) => {
          const roleType = metaPopulation.metaObjectByTag.get(v.t) as RoleType;
          if (roleType == null) {
            throw new Error(
              'RoleType with Tag ' +
                v.t +
                ' is not present. Please regenerate your workspace.'
            );
          }

          const objectType = roleType.objectType;

          let role: unknown;

          if (objectType.isUnit) {
            role = unitFromJson(objectType.tag, v.v);
          } else {
            if (roleType.isOne) {
              role = v.o;
            } else {
              role = v.c;
            }
          }

          return [roleType, role];
        })
      );

      delete this.syncResponseRoles;
    }

    return this._roleByRoleType;
  }

  getRole(roleType: RoleType): unknown {
    return this.roleByRoleType?.get(roleType);
  }

  isPermitted(permission: number): boolean {
    if (permission == null) {
      return false;
    }

    if (this.grants == null) {
      return false;
    }

    if (
      this.revocations != null &&
      this.revocations.some((v) =>
        this.database.ranges.has(
          this.database.revocationById.get(v).permissionIds,
          permission
        )
      )
    ) {
      return false;
    }

    return this.grants.some((v) =>
      this.database.ranges.has(
        this.database.grantById.get(v).permissionIds,
        permission
      )
    );
  }
}
