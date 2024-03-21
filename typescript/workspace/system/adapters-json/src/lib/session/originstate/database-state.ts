import {
  PushRequestNewObject,
  PushRequestObject,
  PushRequestRole,
} from '@allors/database-system-protocol-json';
import {
  DatabaseState as SystemDatabaseState,
  DatabaseRecord,
  Strategy,
  IRange,
} from '@allors/workspace-system-adapters';
import { unitToJson } from '../../json/to-json';
import { IObject } from '@allors/workspace-system-domain';

export class DatabaseState extends SystemDatabaseState {
  constructor(public object: IObject, record: DatabaseRecord) {
    super(record);
  }

  pushNew(): PushRequestNewObject {
    return {
      w: this.id,
      t: this.class.tag,
      r: this.pushRoles(),
    };
  }

  pushExisting(): PushRequestObject {
    return {
      d: this.id,
      v: this.version,
      r: this.pushRoles(),
    };
  }

  private pushRoles(): PushRequestRole[] {
    const ranges = this.session.workspace.ranges;

    if (this.changedRoleByRoleType?.size > 0) {
      const roles: PushRequestRole[] = [];

      for (const [roleType, roleValue] of this.changedRoleByRoleType) {
        const pushRequestRole: PushRequestRole = { t: roleType.tag };

        if (roleType.objectType.isUnit) {
          pushRequestRole.u = unitToJson(roleValue);
        } else if (roleType.isOne) {
          pushRequestRole.c = (roleValue as Strategy)?.id;
        } else {
          const roleStrategies = roleValue as IRange<Strategy>;
          const roleIds = ranges.importFrom(roleStrategies?.map((v) => v.id));
          if (!this.existRecord) {
            pushRequestRole.a = ranges.save(roleIds);
          } else {
            const databaseRole = this.databaseRecord.getRole(
              roleType
            ) as IRange<number>;
            if (databaseRole == null) {
              pushRequestRole.a = ranges.save(roleIds);
            } else {
              pushRequestRole.a = ranges.save(
                ranges.difference(roleIds, databaseRole)
              );
              pushRequestRole.r = ranges.save(
                ranges.difference(databaseRole, roleIds)
              );
            }
          }
        }

        roles.push(pushRequestRole);
      }

      return roles;
    }

    return null;
  }
}
