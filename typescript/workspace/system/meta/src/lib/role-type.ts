import { AssociationType } from './association-type';
import { RelationEndType } from './relation-end-type';
import { RelationType } from './relation-type';

export interface RoleType extends RelationEndType {
  readonly kind: 'RoleType';
  singularName: string;
  associationType: AssociationType;
  relationType: RelationType;
  size?: number;
  precision?: number;
  scale?: number;
  isRequired: boolean;
  mediaType?: string;
}
