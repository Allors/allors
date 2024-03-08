import { RelationEndType } from '@allors/workspace/system/meta';
import { IObject } from '../iobject';
import { Extent } from './extent';
import { ParameterizablePredicateBase } from './parameterizable-predicate';

export interface Intersects extends ParameterizablePredicateBase {
  kind: 'Intersects';
  relationEndType: RelationEndType;
  extent?: Extent;
  objects?: Array<IObject>;
  objectIds?: Array<number>;
}
