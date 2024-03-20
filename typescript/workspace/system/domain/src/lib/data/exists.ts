import { RelationEndType } from '@allors/workspace-system-meta';
import { ParameterizablePredicateBase } from './parameterizable-predicate';

export interface Exists extends ParameterizablePredicateBase {
  kind: 'Exists';

  relationEndType: RelationEndType;
}
