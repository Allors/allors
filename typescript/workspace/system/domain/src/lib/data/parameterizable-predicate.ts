import { ObjectType } from '@allors/workspace/system/meta';
import { Between } from './between';
import { Within } from './within';
import { Contains } from './contains';
import { Equals } from './equals';
import { Exists } from './exists';
import { GreaterThan } from './greater-than';
import { Instanceof } from './instance-of';
import { LessThan } from './less-than';
import { Like } from './like';
import { Intersects } from './intersects';

export type ParameterizablePredicate =
  | Between
  | Within
  | Contains
  | Equals
  | Exists
  | GreaterThan
  | Instanceof
  | LessThan
  | Like
  | Intersects;

export type ParameterizablePredicateKind = ParameterizablePredicate['kind'];

export interface ParameterizablePredicateBase {
  parameter?: string;
}

export function parameterizablePredicateObjectType(
  predicate: ParameterizablePredicate
): ObjectType {
  if (predicate == null) {
    return null;
  }

  switch (predicate.kind) {
    case 'Between':
    case 'GreaterThan':
    case 'LessThan':
    case 'Like':
      return predicate.roleType.objectType;
    case 'Within':
    case 'Intersects':
    case 'Contains':
    case 'Equals':
    case 'Exists':
    case 'Instanceof':
      return predicate.relationEndType.objectType;
  }
}
