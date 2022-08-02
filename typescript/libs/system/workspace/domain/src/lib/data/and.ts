import { Predicate } from './predicate';

export interface And {
  readonly kind: 'And';
  operands: Predicate[];
}
