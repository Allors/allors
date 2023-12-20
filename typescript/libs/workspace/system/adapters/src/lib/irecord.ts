import { RoleType } from '@allors/workspace/system/meta';

export interface IRecord {
  version: number;

  getRole(roleType: RoleType): unknown;
}
