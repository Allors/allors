import { Composite } from '@allors/workspace/system/meta';
import { Predicate } from './predicate';
import { Sort } from './sort';

export interface Filter {
  kind: 'Filter';

  objectType: Composite;

  predicate?: Predicate;

  sorting?: Sort[];
}
