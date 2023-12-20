import { Injectable } from '@angular/core';
import { Composite } from '@allors/workspace/system/meta';
import { FilterDefinition } from '../filter/filter-definition';
import { Filter } from './filter';

@Injectable()
export abstract class FilterService {
  abstract filter(composite: Composite): Filter;
}
