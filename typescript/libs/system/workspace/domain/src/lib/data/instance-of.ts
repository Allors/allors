import { ObjectType, RelationEndType } from '@allors/system/workspace/meta';
import { ParameterizablePredicateBase } from './parameterizable-predicate';

export interface Instanceof extends ParameterizablePredicateBase {
  kind: 'Instanceof';
  relationEndType?: RelationEndType;
  objectType?: ObjectType;
}
