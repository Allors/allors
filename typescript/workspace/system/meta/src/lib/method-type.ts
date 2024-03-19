import { Composite } from './composite';
import { OperandType } from './operand-type';

export interface MethodType extends OperandType {
  readonly kind: 'MethodType';
  objectType: Composite;
}
