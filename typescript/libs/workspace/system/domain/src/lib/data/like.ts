import { RoleType } from '@allors/workspace/system/meta';
import { ParameterizablePredicateBase } from './parameterizable-predicate';

export interface Like extends ParameterizablePredicateBase {
  kind: 'Like';
  roleType: RoleType;
  value?: string;
}
