import { Injectable } from '@angular/core';
import { Composite } from '@allors/workspace-system-meta';
import { Action } from './action';

@Injectable()
export abstract class ActionService {
  abstract action(objectType: Composite): Action[];
}
