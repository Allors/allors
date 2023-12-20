import { Predicate } from './predicate';

export interface Or {
  kind: 'Or';
  operands: Predicate[];
}
