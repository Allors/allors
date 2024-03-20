import { ObjectType, RelationEndType } from '@allors/workspace-system-meta';
import { ParameterizablePredicateBase } from './parameterizable-predicate';

export interface Instanceof extends ParameterizablePredicateBase {
  kind: 'Instanceof';
  relationEndType?: RelationEndType;
  objectType?: ObjectType;
}
