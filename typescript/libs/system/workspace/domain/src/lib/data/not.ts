import { Predicate } from './predicate';

export interface Not  {
  kind: 'Not';
  operand?: Predicate;
}
