import { RelationEndType } from './relation-end-type';
import { RelationType } from './relation-type';
import { RoleType } from './role-type';

export interface AssociationType extends RelationEndType {
  readonly kind: 'AssociationType';
  relationType: RelationType;
  roleType: RoleType;
}
