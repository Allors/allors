import { RelationEndType } from '@allors/workspace/system/meta';
import { IObject } from '../iobject';
import { Extent } from './extent';
import { ParameterizablePredicateBase } from './parameterizable-predicate';

export interface Within extends ParameterizablePredicateBase {
  kind: 'Within';
  relationEndType: RelationEndType;
  extent?: Extent;
  objects?: Array<IObject>;
  objectIds?: Array<number>;
}
