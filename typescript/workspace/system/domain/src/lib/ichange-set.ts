import { AssociationType, RoleType } from '@allors/workspace-system-meta';
import { IObject } from './iobject';
import { ISession } from './isession';

export interface IChangeSet {
  session: ISession;

  created: Set<IObject>;

  associationsByRoleType: Map<RoleType, Set<IObject>>;

  rolesByAssociationType: Map<AssociationType, Set<IObject>>;
}
