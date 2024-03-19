import { MetaObject } from "./meta-object";

export interface OperandType extends MetaObject {
  operandTag: string;
  name: string;
}
