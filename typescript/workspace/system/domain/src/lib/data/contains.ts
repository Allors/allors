import { RelationEndType } from '@allors/workspace/system/meta';
import { IObject } from '../iobject';
import { ParameterizablePredicateBase } from './parameterizable-predicate';

export interface Contains extends ParameterizablePredicateBase {
  kind: 'Contains';
  relationEndType: RelationEndType;
  object?: IObject;
  objectId?: number;
}
