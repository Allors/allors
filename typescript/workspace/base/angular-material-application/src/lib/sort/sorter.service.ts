import { Composite } from '@allors/workspace/system/meta';
import { Injectable } from '@angular/core';
import { Sorter } from './sorter';

@Injectable()
export abstract class SorterService {
  abstract sorter(composite: Composite): Sorter;
}
