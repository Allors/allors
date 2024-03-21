import { RoleType } from '@allors/workspace-system-meta';
import { IObject } from '../iobject';

export interface IDiff {
  roleType: RoleType;

  assocation: IObject;
}
