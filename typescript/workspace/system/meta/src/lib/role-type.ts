import { AssociationType } from './association-type';
import { Multiplicity } from './multiplicity';
import { RelationEndType } from './relation-end-type';

export interface RoleType extends RelationEndType {
  readonly kind: 'RoleType';
  associationType: AssociationType;

  singularName: string;
  multiplicity: Multiplicity;
  size?: number;
  precision?: number;
  scale?: number;
  isDerived: boolean;
  isRequired: boolean;
  mediaType?: string;
}
