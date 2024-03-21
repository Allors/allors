import { RelationEndType } from './relation-end-type';
import { RoleType } from './role-type';

export interface AssociationType extends RelationEndType {
  readonly kind: 'AssociationType';
  roleType: RoleType;
}
