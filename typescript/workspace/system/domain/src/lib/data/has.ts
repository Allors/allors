import { RelationEndType } from '@allors/workspace-system-meta';
import { IObject } from '../iobject';
import { ParameterizablePredicateBase } from './parameterizable-predicate';

export interface Has extends ParameterizablePredicateBase {
  kind: 'Has';
  relationEndType: RelationEndType;
  object?: IObject;
  objectId?: number;
}
