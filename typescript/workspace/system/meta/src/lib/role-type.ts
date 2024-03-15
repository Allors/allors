import { AssociationType } from './association-type';
import { Multiplicity } from './multiplicity';
import { RelationEndType } from './relation-end-type';

export interface RoleType extends RelationEndType {
  readonly kind: 'RoleType';
  multiplicity: Multiplicity;
  singularName: string;
  associationType: AssociationType;
  size?: number;
  precision?: number;
  scale?: number;
  isDerived: boolean;
  isRequired: boolean;
  mediaType?: string;
}
