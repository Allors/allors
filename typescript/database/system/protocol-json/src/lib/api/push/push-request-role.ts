import { IUnit } from '@allors/workspace-system-domain';

export interface PushRequestRole {
  /** RoleType */
  t: string;

  /** SetUnitRole */
  u?: IUnit;

  /** SetCompositeRole */
  c?: number;

  /** AddCompositesRole */
  a?: number[];

  /** RemoveCompositesRole */
  r?: number[];
}
