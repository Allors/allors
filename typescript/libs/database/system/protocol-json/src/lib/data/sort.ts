import { SortDirection } from '@allors/workspace/system/domain';

export interface Sort {
  /** RoleType */
  r: string;

  /** Direction */
  d: SortDirection;
}
