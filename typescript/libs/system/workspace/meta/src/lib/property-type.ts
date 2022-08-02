import { ObjectType } from './object-type';
import { OperandType } from './operand-type';
import { RelationType } from './relation-type';

export interface PropertyType extends OperandType {
  isRoleType: boolean;
  isAssociationType: boolean;
  isMethodType: boolean;

  relationType: RelationType;
  objectType: ObjectType;
  singularName: string;
  pluralName: string;
  isOne: boolean;
  isMany: boolean;
}
