import { ObjectType } from './object-type';
import { OperandType } from './operand-type';

export interface RelationEndType extends OperandType {
  isRoleType: boolean;
  isAssociationType: boolean;
  isMethodType: boolean;

  objectType: ObjectType;
  singularName: string;
  pluralName: string;
  isOne: boolean;
  isMany: boolean;
}
