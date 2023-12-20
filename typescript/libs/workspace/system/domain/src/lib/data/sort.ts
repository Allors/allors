import { RoleType } from '@allors/workspace/system/meta';
import { SortDirection } from './sort-direction';

export interface Sort {
  roleType: RoleType;
  sortDirection?: SortDirection;
}
