import { AssociationType } from './association-type';
import { MetaObject } from './meta-object';
import { RoleType } from './role-type';

export interface RelationType extends MetaObject {
  readonly kind: 'RelationType';
  associationType: AssociationType;
  roleType: RoleType;
}
